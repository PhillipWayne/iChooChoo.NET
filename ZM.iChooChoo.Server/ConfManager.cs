using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.BICCP;
using ZM.iChooChoo.Library.Config;
using ZM.iChooChoo.Library.Log;
using ZM.iChooChoo.Library.Modules;
using ZM.iChooChoo.Server.BICCP;

namespace ZM.iChooChoo.Server
{
    /// <summary>
    /// Class handing the configuration file.
    /// </summary>
    public class ConfManager : IConfManager
    {
        /// <summary>
        /// Gets or sets the BICCP Manager.
        /// </summary>
        private IBICCPManager Biccp { get; set; }

        /// <summary>
        /// Gets or sets the Logger.
        /// </summary>
        private ILogger Log { get; set; }

        /// <summary>
        /// Instantiates a new ConfManager.
        /// </summary>
        /// <param name="biccp">BICCP Manager.</param>
        /// <param name="log">Logger.</param>
        public ConfManager(IBICCPManager biccp, ILogger log)
        {
            if (biccp == null)
                throw new ArgumentException("ConfManager : argument biccp cannot be null.");
            else
                Biccp = biccp;

            if (log == null)
                throw new ArgumentException("ConfManager : argument log cannot be null.");
            else
                Log = log;

            Modules = new Dictionary<byte, IModule>();
            Positions = new Dictionary<int, ConfPosition>();
            Relays = new Dictionary<int, ConfRelay>();
            Sections = new Dictionary<int, ConfSection>();
            Sensors = new Dictionary<int, ConfSensor>();
            Switchs = new Dictionary<int, ConfSwitch>();
        }

        /// <summary>
        /// List of modules referenced by their address.
        /// </summary>
        public virtual Dictionary<byte, IModule> Modules { get; private set; }

        /// <summary>
        /// List of positions referenced by their ID.
        /// </summary>
        public virtual Dictionary<int, ConfPosition> Positions { get; private set; }

        /// <summary>
        /// List of relays referenced by their ID.
        /// </summary>
        public virtual Dictionary<int, ConfRelay> Relays { get; private set; }

        /// <summary>
        /// List of sections referenced by their ID.
        /// </summary>
        public virtual Dictionary<int, ConfSection> Sections { get; private set; }

        /// <summary>
        /// List of sensors referenced by their ID.
        /// </summary>
        public virtual Dictionary<int, ConfSensor> Sensors { get; private set; }

        /// <summary>
        /// List of switches referenced by their ID.
        /// </summary>
        public virtual Dictionary<int, ConfSwitch> Switchs { get; private set; }

        /// <summary>
        /// Gets or sets if a bus rescan is pending.
        /// </summary>
        public virtual bool RescanPending { get; set; }

        /// <summary>
        /// Actual event handler for BusRescanned public event.
        /// </summary>
        protected event EventHandler _busRescanned;

        /// <summary>
        /// Event fired when the I2C bus is rescanned.
        /// </summary>
        public event EventHandler BusRescanned
        {
            // Prevents a delegate to subscribe to the same event twice
            add
            {
                bool exists = false;
                if (_busRescanned != null)
                {
                    foreach (Delegate existingHandler in _busRescanned.GetInvocationList())
                    {
                        if (Delegate.Equals(existingHandler, value))
                            exists = true;
                    }
                }
                if (!exists)
                    _busRescanned += value;
            }
            remove { _busRescanned -= value; }
        }

        /// <summary>
        /// Raises the BusScanned event.
        /// </summary>
        protected virtual void RaiseBusRescanned()
        {
            if (_busRescanned != null)
                _busRescanned(this, new EventArgs());
        }

        /// <summary>
        /// Clears the modules list.
        /// </summary>
        private void ClearModules()
        {
            Modules.Clear();
        }

        /// <summary>
        /// Clears the configurations.
        /// </summary>
        private void ClearConfig()
        {
            Positions.Clear();
            Relays.Clear();
            Sections.Clear();
            Sensors.Clear();
            Switchs.Clear();
        }

        /// <summary>
        /// Display configuration on standard output.
        /// </summary>
        /// <param name="s">Stream on which output is sent.</param>
        public virtual void Display(StreamWriter s)
        {
            s.WriteLine("ICHOOCHOO_CONF V1");
            s.WriteLine();

            s.WriteLine("#POSITION ID DESCRIPTION");
            foreach(var v in Positions)
            {
                s.WriteLine(string.Format("POSITION {0} {1}", v.Value.ID.ToString("X").ToUpper(), v.Value.Description));
            }
            s.WriteLine();

            s.WriteLine("#SECTION ID MODULE_ID OUTPUT DESCRIPTION");
            foreach (var v in Sections)
            {
                s.WriteLine(string.Format("SECTION {0} {1} {2} {3}", v.Value.ID.ToString("X").ToUpper(), v.Value.ModuleID.ToString("X").ToUpper(), v.Value.IOPort.ToString("X").ToUpper(), v.Value.Description));
            }
            s.WriteLine();

            s.WriteLine("#SWITCH ID MODULE_ID OUTPUT STRAIGHTVALUE DESCRIPTION");
            foreach (var v in Switchs)
            {
                s.WriteLine(string.Format("SWITCH {0} {1} {2} {3} {4}", v.Value.ID.ToString("X").ToUpper(), v.Value.ModuleID.ToString("X").ToUpper(), v.Value.IOPort.ToString("X").ToUpper(), v.Value.StraightValue.ToString(), v.Value.Description));
            }
            s.WriteLine();

            s.WriteLine("#RELAY ID MODULE_ID OUTPUT DESCRIPTION");
            foreach (var v in Relays)
            {
                s.WriteLine(string.Format("RELAY {0} {1} {2} {3}", v.Value.ID.ToString("X").ToUpper(), v.Value.ModuleID.ToString("X").ToUpper(), v.Value.IOPort.ToString("X").ToUpper(), v.Value.Description));
            }
            s.WriteLine();

            s.WriteLine("#SENSOR ID MODULE_ID INPUT TYPE DESCRIPTION");
            foreach (var v in Sensors)
            {
                s.WriteLine(string.Format("SENSOR {0} {1} {2} {3} {4}", v.Value.ID.ToString("X").ToUpper(), v.Value.ModuleID.ToString("X").ToUpper(), v.Value.IOPort.ToString("X").ToUpper(), v.Value.Type.ToString("X").ToUpper(), v.Value.Description));
            }
        }

        /// <summary>
        /// Get module identification.
        /// </summary>
        /// <param name="addr">Address of the module.</param>
        /// <returns>A correctly typed Module instance.</returns>
        private IModule GetModuleIdent(byte addr)
        {
            IModule module = null;
            var answer = new BICCPData();

            if (Biccp.RequestToModule(addr, answer, ICCConstants.BICCP_GRP_CONF, ICCConstants.BICCP_CMD_CONF_VERSION))
            {
                int iMajor = answer.Data[0];
                int iMinor = answer.Data[1];
                int iBuild = answer.Data[2];
                if (Biccp.RequestToModule(addr, answer, ICCConstants.BICCP_GRP_CONF, ICCConstants.BICCP_CMD_CONF_IDENT))
                {
                    module = ModuleFactory.CreateInstance(answer.Data[1]);
                    module.Major = iMajor;
                    module.Minor = iMinor;
                    module.Build = iBuild;

                    module.Description = string.Empty;
                    for (int i = 0; i < ICCConstants.DESCSIZE; i++)
                    {
                        if (answer.Data[i + 2] != 0)
                            module.Description += (char)answer.Data[i + 2];
                    }
                }
            }

            Log.LogMessage((module != null), "Module identification", addr);

            return module;
        }

        /// <summary>
        /// Reads the configuration file.
        /// </summary>
        /// <param name="path">File name and path.</param>
        /// <returns>True on success, false otherwise.</returns>
        public virtual bool ReadConf(string path)
        {
            bool bReturn = true;
            //string myLine, myMotionLine;
            int iLine = 0;
            var nfi = new System.Globalization.NumberFormatInfo();

            ScanBus();
            ClearConfig();

            var fi = new FileInfo(ICCConstants.SERVER_CONF_PATH);
            if (fi.Exists)
            {
                using (var sr = new StreamReader(ICCConstants.SERVER_CONF_PATH))
                {
                    bool bHeaderFound = false;
                    while (!sr.EndOfStream && !bHeaderFound)
                    {
                        var line = sr.ReadLine().Trim();
                        iLine++;
                        if (line.Length > 0 && line[0] != '#')
                            if (line.Substring(0, 17) == "ICHOOCHOO_CONF V1")
                                bHeaderFound = true;
                    }
                    if (bHeaderFound)
                    {
                        while (!sr.EndOfStream && bReturn)
                        {
                            var line = sr.ReadLine().Trim();
                            iLine++;
                            if (line.Length > 0 && line[0] != '#')
                            {
                                var terms = line.Split(' ');
                                if (terms.Length > 0)
                                {
                                    if (terms[0] == "POSITION")
                                    {
                                        if (terms.Length == 3)
                                        {
                                            byte bID = 0xFF;
                                            if (byte.TryParse(terms[1], NumberStyles.HexNumber, nfi, out bID))
                                            {
                                                if (!Positions.ContainsKey(bID))
                                                    Positions.Add(bID, new ConfPosition() { ID = bID, Description = terms[2] });
                                                else
                                                    bReturn = false;
                                            }
                                            else
                                                bReturn = false;
                                        }
                                        else
                                            bReturn = false;
                                    }
                                    else if (terms[0] == "SECTION")
                                    {
                                        if (terms.Length == 5)
                                        {
                                            int iID = -1;
                                            int iModuleAddr = -1;
                                            int iOutput = -1;
                                            if (int.TryParse(terms[1], NumberStyles.HexNumber, nfi, out iID))
                                            {
                                                if (int.TryParse(terms[2], NumberStyles.HexNumber, nfi, out iModuleAddr))
                                                {
                                                    if (int.TryParse(terms[3], NumberStyles.HexNumber, nfi, out iOutput))
                                                    {
                                                        if (!Sections.ContainsKey(iID))
                                                            Sections.Add(iID, new ConfSection() { ID = iID, ModuleID = iModuleAddr, IOPort = iOutput, Description = terms[4] });
                                                        else
                                                            bReturn = false;
                                                    }
                                                    else
                                                        bReturn = false;
                                                }
                                                else
                                                    bReturn = false;
                                            }
                                            else
                                                bReturn = false;
                                        }
                                        else
                                            bReturn = false;
                                    }
                                    else if (terms[0] == "SWITCH")
                                    {
                                        if (terms.Length == 6)
                                        {
                                            int iID = -1;
                                            int iModuleAddr = -1;
                                            int iOutput = -1;
                                            int iStraightValue = -1;
                                            if (int.TryParse(terms[1], NumberStyles.HexNumber, nfi, out iID))
                                            {
                                                if (int.TryParse(terms[2], NumberStyles.HexNumber, nfi, out iModuleAddr))
                                                {
                                                    if (int.TryParse(terms[3], NumberStyles.HexNumber, nfi, out iOutput))
                                                    {
                                                        if (int.TryParse(terms[4], NumberStyles.Integer, nfi, out iStraightValue))
                                                        {
                                                            if (!Switchs.ContainsKey(iID))
                                                                Switchs.Add(iID, new ConfSwitch() { ID = iID, ModuleID = iModuleAddr, IOPort = iOutput, StraightValue = iStraightValue, Description = terms[5] });
                                                            else
                                                                bReturn = false;
                                                        }
                                                        else
                                                            bReturn = false;
                                                    }
                                                    else
                                                        bReturn = false;
                                                }
                                                else
                                                    bReturn = false;
                                            }
                                            else
                                                bReturn = false;
                                        }
                                        else
                                            bReturn = false;
                                    }
                                    else if (terms[0] == "RELAY")
                                    {
                                        if (terms.Length == 5)
                                        {
                                            int iID = -1;
                                            int iModuleAddr = -1;
                                            int iOutput = -1;
                                            if (int.TryParse(terms[1], NumberStyles.HexNumber, nfi, out iID))
                                            {
                                                if (int.TryParse(terms[2], NumberStyles.HexNumber, nfi, out iModuleAddr))
                                                {
                                                    if (int.TryParse(terms[3], NumberStyles.HexNumber, nfi, out iOutput))
                                                    {
                                                        if (!Relays.ContainsKey(iID))
                                                            Relays.Add(iID, new ConfRelay() { ID = iID, ModuleID = iModuleAddr, IOPort = iOutput, Description = terms[4] });
                                                        else
                                                            bReturn = false;
                                                    }
                                                    else
                                                        bReturn = false;
                                                }
                                                else
                                                    bReturn = false;
                                            }
                                            else
                                                bReturn = false;
                                        }
                                        else
                                            bReturn = false;
                                    }
                                    else if (terms[0] == "SENSOR")
                                    {
                                        if (terms.Length == 6)
                                        {
                                            int iID = -1;
                                            int iModuleAddr = -1;
                                            int iOutput = -1;
                                            int iType = -1;
                                            if (int.TryParse(terms[1], NumberStyles.HexNumber, nfi, out iID))
                                            {
                                                if (int.TryParse(terms[2], NumberStyles.HexNumber, nfi, out iModuleAddr))
                                                {
                                                    if (int.TryParse(terms[3], NumberStyles.HexNumber, nfi, out iOutput))
                                                    {
                                                        if (int.TryParse(terms[4], NumberStyles.HexNumber, nfi, out iType))
                                                        {
                                                            if (!Sensors.ContainsKey(iID))
                                                                Sensors.Add(iID, new ConfSensor() { ID = iID, ModuleID = iModuleAddr, IOPort = iOutput, Type = iType, Description = terms[5] });
                                                            else
                                                                bReturn = false;
                                                        }
                                                        else
                                                            bReturn = false;
                                                    }
                                                    else
                                                        bReturn = false;
                                                }
                                                else
                                                    bReturn = false;
                                            }
                                            else
                                                bReturn = false;
                                        }
                                        else
                                            bReturn = false;
                                    }
                                    else if (terms[0] == "MOTION")
                                    {
                                        bool bInMotion = true;
                                        while (!sr.EndOfStream && bInMotion && bReturn)
                                        {
                                            var motionLine = sr.ReadLine().Trim();
                                            iLine++;
                                            if (line.Length > 0 && line[0] != '#')
                                            {
                                                var motionTerms = line.Split(' ');
                                                if (motionTerms.Length > 0)
                                                {
                                                    if (motionTerms[0] == "ENDMOTION")
                                                    {
                                                        bInMotion = false;
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    if (!bReturn)
                                        Log.LogText(string.Format("Configuration error: syntax error at line {0} '{1}'", iLine, line));
                                }
                            }
                        }
                    }
                    else
                        bReturn = false;

                    sr.Close();
                }
            }
            else
                Log.LogText("No configuration file found.", LogLevel.Information);

            return bReturn;
        }

        /// <summary>
        /// Scans the I2C bus.
        /// </summary>
        /// <returns>QUantity of modules scanned.</returns>
        public virtual int ScanBus()
        {
            RescanPending = true;

            ClearModules();

            byte[] moduleList = new byte[0x78];
            int iNbModules = Biccp.BICCP_ScanBus(ref moduleList);

            int i = 0;
            for (byte l = 0; l < 0x78; l++)
            {
                if (moduleList[l] > 0)
                {
                    var ccMod = GetModuleIdent(l);
                    if (ccMod == null)
                    {
                        System.Threading.Thread.Sleep(10);
                        ccMod = GetModuleIdent(l);
                    }
                    if (ccMod != null)
                    {
                        ccMod.ID = l;
                        Modules.Add(l, ccMod);
                    }
                    else
                    {
                        ccMod = ModuleFactory.CreateInstance(ICCConstants.BICCP_GRP_UNKNOWN);
                        ccMod.ID = l;
                        Modules.Add(l, ccMod);
                    }
                    i++;
                }
            }

            RaiseBusRescanned();

            RescanPending = false;

            return i;
        }
    }
}
