using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library;
using ZM.iChooChoo.Library.Actions;
using ZM.iChooChoo.Library.BICCP;
using ZM.iChooChoo.Library.Log;
using ZM.iChooChoo.Library.Modules;
using ZM.iChooChoo.Server.BICCP;

namespace ZM.iChooChoo.Server.Daemon
{
    class Program
    {
        public static int EXIT_SUCCESS = 0;

        private static IConfManager Conf { get; set; }
        private static IBICCPManager Biccp { get; set; }
        private static ILogger Log { get; set; }

        static int Main(string[] args)
        {
            Log = new Logger(LogLevel.Debug);
            Log.AddLogManager(new ScreenLogManager());
            Log.AddLogManager(new ServerLogManager(Log));
            Biccp = new BICCPManager(Log);
            Conf = new ConfManager(Biccp, Log);

            if (Conf.ReadConf(ICCConstants.SERVER_CONF_PATH))
            {
                int iReturnCode = ProcessArguments(args);
                if (iReturnCode == -1)
                {
                    PrintVersion();
                    PrintHelp(System.Reflection.Assembly.GetExecutingAssembly().GetName().Name);
                    return EXIT_SUCCESS;
                }
                else if (iReturnCode == 0)
                    return 0;
                else
                    return EXIT_SUCCESS;
            }
            else
            {
                Console.WriteLine("Error detected while reading configuration file, exiting...\n");
                return 0;
            }
        }

        protected static int ProcessArguments(string[] args)
        {
            int iReturn = -1; // -1=Nothing done; 0=Failed; 1=Success

            bool bVersion = false;
            bool bHelp = false;
            bool bScan = false;
            bool bDumpConfig = false;
            bool bDaemon = false;
            byte iSoftReset = 0;
            byte iHardReset = 0;
            byte iGetIdent = 0;
            byte iSetAddr = 0;
            byte iSetType = 0;
            byte iSetDesc = 0;
            string sSetDesc = null;

            if (args.Length > 0)
            {
                int i = 0;
                int l = 0;
                while (i < args.Length)
                {
                    if (args[i] == "--version")
                    {
                        bVersion = true;
                    }
                    else if (args[i] == "--help")
                    {
                        bVersion = true;
                        bHelp = true;
                    }
                    else if (args[i] == "--scan")
                    {
                        bScan = true;
                    }
                    else if (args[i] == "--dumpconfig")
                    {
                        bDumpConfig = true;
                    }
                    else if (args[i] == "--daemon")
                    {
                        bDaemon = true;
                    }
                    else if ((l = CheckArgumentGetByte(args, i, "--ident", 1, ref iGetIdent)) != 0)
                    {
                        if (l > 0) i++;
                        else return 0;
                    }
                    else if ((l = CheckArgumentGetByte(args, i, "--setaddr", 1, ref iSetAddr)) != 0)
                    {
                        if (l > 0) i++;
                        else return 0;
                    }
                    else if ((l = CheckArgumentGetByte(args, i, "--settype", 1, ref iSetType)) != 0)
                    {
                        if (l > 0) i++;
                        else return 0;
                    }
                    else if ((l = CheckArgumentGetByte(args, i, "--setdesc", 2, ref iSetDesc)) != 0)
                    {
                        if (l > 0)
                        {
                            sSetDesc = args[i + 2];
                            if (sSetDesc.Length > ICCConstants.DESCSIZE)
                            {
                                Console.WriteLine(string.Format("Description too long in argument passed to {0} {1} {2}\n", args[i], args[i + 1], args[i + 2]));
                                return 0;
                            }
                            i += 2;
                        }
                        else return 0;
                    }
                    else if ((l = CheckArgumentGetByte(args, i, "--softreset", 1, ref iSoftReset)) != 0)
                    {
                        if (l > 0) i++;
                        else return 0;
                    }
                    else if ((l = CheckArgumentGetByte(args, i, "--hardreset", 1, ref iHardReset)) != 0)
                    {
                        if (l > 0) i++;
                        else return 0;
                    }
                    else
                    {
                        Console.WriteLine("Unknown argument %s\n", args[i]);
                        return 0;
                    }
                    i++;
                }
            }

            if (bVersion && args.Length > 2)
            {
                Console.WriteLine("Argument --version should be used alone.\n");
                return 0;
            }
            if (bHelp && args.Length > 2)
            {
                Console.WriteLine("Argument --help should be used alone.\n");
                return 0;
            }
            if (bScan && args.Length > 2)
            {
                Console.WriteLine("Argument --scan should be used alone.\n");
                return 0;
            }
            if (bDumpConfig && args.Length > 2)
            {
                Console.WriteLine("Argument --dumpconfig should be used alone.\n");
                return 0;
            }
            if (bDaemon && args.Length > 2)
            {
                Console.WriteLine("Argument --daemon should be used alone.\n");
                return 0;
            }

            if (bVersion)
            {
                PrintVersion();
                iReturn = 1;
            }

            if (bHelp)
            {
                PrintHelp(args[0]);
                iReturn = 1;
            }

            if (bScan)
            {
                PrintModuleList();
                iReturn = 1;
            }

            if (bDumpConfig)
            {
                var sw = new StreamWriter(Console.OpenStandardOutput());
                sw.AutoFlush = true;
                Console.SetOut(sw);
                Conf.Display(sw);
                iReturn = 1;
            }

            if (bDaemon)
            {
                var d = new DaemonManager(Biccp, Conf, Log);
                return d.Start() ? 1 : 0;
            }

            if (iGetIdent != 0)
            {
                if (Conf.Modules.ContainsKey(iGetIdent))
                {
                    var ccMod = Conf.Modules[iGetIdent];
                    Console.WriteLine("\n");
                    Console.WriteLine(string.Format("    Version     : {0}.{1}.{2}", ccMod.Major, ccMod.Minor, ccMod.Build));
                    Console.WriteLine(string.Format("    Address     : 0x{0:x2}", iGetIdent));
                    Console.WriteLine(string.Format("    Type        : 0x{0:x2} - {1}", ccMod.Type, ccMod.TypeDescription));
                    Console.WriteLine(string.Format("    Description : {0}", ccMod.Description));
                    Console.WriteLine("\n");
                }
                iReturn = 1;
            }
            if (iSetAddr != 0)
            {
                var mod = new NewModule() { ID = 0x77, Log = Log };
                mod.ActionRaised += Mod_ActionRaised;
                mod.SetAddress(iSetAddr);
                iReturn = 1;
            }
            if (iSetType != 0)
            {
                var mod = new NewModule() { ID = 0x77, Log = Log };
                mod.ActionRaised += Mod_ActionRaised;
                mod.SetType(iSetType);
                iReturn = 1;
            }
            if (iSetDesc != 0)
            {
                var mod = new NewModule() { ID = iSetDesc, Log = Log };
                mod.ActionRaised += Mod_ActionRaised;
                mod.SetDescription(sSetDesc);
                iReturn = 1;
            }
            if (iSoftReset != 0)
            {
                var mod = new NewModule() { ID = iSoftReset, Log = Log };
                mod.ActionRaised += Mod_ActionRaised;
                mod.SoftReset();
                iReturn = 1;
            }
            if (iHardReset != 0)
            {
                var mod = new NewModule() { ID = iHardReset, Log = Log };
                mod.ActionRaised += Mod_ActionRaised;
                mod.HardReset();
                iReturn = 1;
            }

            return iReturn;
        }

        private static void Mod_ActionRaised(object sender, ActionEventArgs e)
        {
            var answer = new BICCPData();
            e.Action.ToBICCP(Biccp, answer);
        }

        /// <summary>
        /// Checks a CLI argument
        /// </summary>
        /// <param name="args">argument list</param>
        /// <param name="argi">index to check</param>
        /// <param name="argname">name of argument to check</param>
        /// <param name="nparam">number of parameters required</param>
        /// <returns>-1 if there are missing parameters, 1 if ok, 0 otherwise</returns>
        protected static int CheckArgument(string[] args, int argi, string argname, int nparam)
        {
            if (args[argi] == argname)
            {
                if (args.Length < argi + nparam + 1)
                {
                    Console.WriteLine("Argument %s needs %d parameter(s), please use %s --help\n", argname, nparam, args[0]);
                    return -1;
                }
                for (int i = 1; i <= nparam; i++)
                {
                    if (args[argi + i].Length > 2)
                        if (args[argi + i][0] == '-' && args[argi + i][1] == '-')
                        {
                            Console.WriteLine("Argument %s needs %d parameter(s), please use %s --help\n", argname, nparam, args[0]);
                            return -1;
                        }
                }
                return 1;
            }
            else
                return 0;
        }

        protected static int CheckArgumentGetByte(string[] args, int argi, string argname, int nparam, ref byte data)
        {
            int iReturn = CheckArgument(args, argi, argname, nparam);
            if (iReturn > 0)
            {
                data = Convert.ToByte(args[argi + 1], 16);
                if (data == 0)
                {
                    Console.WriteLine("Invalid argument passed to %s %s\n", args[argi], args[argi + 1]);
                    return -1;
                }
            }
            return iReturn;
        }

        protected static void PrintModuleList()
        {
            Console.WriteLine();
            Console.WriteLine(string.Format("Bus scan complete: {0} modules found.", Conf.Modules.Count));
            Console.WriteLine();
            Console.WriteLine("+---------+----------+------+----------------+");
            Console.WriteLine("| Address | Version  | Type | Description    |");
            Console.WriteLine("+---------+----------+------+----------------+");

            foreach(var m in Conf.Modules)
            {
                var ccMod = m.Value;

                if (ccMod.Type != 0)
                {
                    string sDescription = ccMod.ID == 0x77 ? "(new module)" : ccMod.Description;
                    string sLine = string.Format("| 0x{0:x2}    | {1, -8} | 0x{2:x2} | {3, -14} |", ccMod.ID, ccMod.Version, ccMod.Type, sDescription);
                    Console.WriteLine(sLine);
                }
                else
                {
                    Console.WriteLine(string.Format("| 0x{0:x2}    |    --    |  --  | (unidentified) |", ccMod.ID));
                }
            }
            Console.WriteLine("+---------+----------+------+----------------+");
        }

        protected static int PrintVersion()
        {
            Console.WriteLine(string.Format("iChooChoo version {0}.{1}.{2}", ICCConstants.PROG_VERSION_MAJ, ICCConstants.PROG_VERSION_MIN, ICCConstants.PROG_VERSION_BLD));
            return -1;
        }

        protected static int PrintHelp(string executableName)
        {
            Console.WriteLine("");
            Console.WriteLine(string.Format("    Usage: {0} [options]", executableName));
            Console.WriteLine("");
            Console.WriteLine("  --version             Print version info");
            Console.WriteLine("  --help                Print this help text");
            Console.WriteLine("  --scan                Scans the i2c bus and reports the list of modules detected");
            Console.WriteLine("  --daemon              Starts the daemon mode");
            Console.WriteLine("  --dumpconfig          Dumps the configuration file to the screen (throws an error if the configuration file is wrong");
            Console.WriteLine("  --ident 0xHH          Print module identification on 0xHH address");
            Console.WriteLine("  --setaddr 0xHH        Set new module* address to 0xHH");
            Console.WriteLine("  --settype 0xHH        Set new module* type to 0xHH");
            Console.WriteLine("  --setdesc 0xHH desc   Set description desc to 0xHH address");
            Console.WriteLine("  --softreset 0xHH      Sends a soft reset signal to 0xHH address");
            Console.WriteLine("  --hardreset 0xHH      Sends a hard reset** signal to 0xHH address");
            Console.WriteLine("");
            Console.WriteLine("*  new module is only on address 0x77.");
            Console.WriteLine("** be sure that no other new module is connected on the bus before using hard reset.");
            return -1;
        }
    }
}
