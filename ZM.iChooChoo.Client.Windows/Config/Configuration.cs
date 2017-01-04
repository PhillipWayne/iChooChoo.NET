using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ZM.iChooChoo.Client.Windows.Config
{
    /// <summary>
    /// Class containing application configuration and methods to read from and save to file in JSon format.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// Gets the actual application configuration.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public static Configuration Config { get; private set; } = new Configuration();

        /// <summary>
        /// Gets the file name including path of the configuration file.
        /// </summary>
        [System.Xml.Serialization.XmlIgnore]
        public static string FileName { get { return Environment.ExpandEnvironmentVariables(@"%USERPROFILE%\AppData\Local\iChooChoo\iChooChoo.conf"); } }

        /// <summary>
        /// Reads configuration from file.
        /// </summary>
        public static void ReadFromFile()
        {
            var fi = new FileInfo(FileName);
            if (fi.Exists)
            {
                var sr = new StreamReader(FileName);
                string s = sr.ReadToEnd();
                sr.Close();
                Config = fastJSON.JSON.ToObject<Configuration>(s);
            }
        }

        /// <summary>
        /// Save configuration to file.
        /// </summary>
        public static void SaveToFile()
        {
            var fi = new FileInfo(FileName);
            if (!fi.Directory.Exists)
                fi.Directory.Create();

            var p = new fastJSON.JSONParameters();
            p.UseExtensions = false;
            string s = fastJSON.JSON.ToJSON(Config, p);
            var sw = new StreamWriter(FileName);
            sw.Write(fastJSON.JSON.Beautify(s));
            sw.Close();
        }

        /// <summary>
        /// Instantiates a new Configuration.
        /// </summary>
        public Configuration()
        {
            ServerSettings = new ServerSettings();
            MainWindowSettings = new MainWindowSettings();
        }

        #region Settings

        /// <summary>
        /// Gets or sets the Server Settings.
        /// </summary>
        public virtual ServerSettings ServerSettings { get; set; }

        /// <summary>
        /// Gets or sets the Main Window Settings.
        /// </summary>
        public virtual MainWindowSettings MainWindowSettings { get; set; }

        #endregion
    }
}
