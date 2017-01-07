using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ZM.iChooChoo.Library.Log;

namespace ZM.iChooChoo.Library
{
    /// <summary>
    /// Static class containing ICC constants.
    /// </summary>
    public static class ICCConstants
    {
        /// <summary>
        /// End Of Line char, terminates all TCP messages.
        /// </summary>
        public const char EOL = '\n';

        /// <summary>
        /// TCP Command port.
        /// </summary>
        public const int TCP_CMD_PORT = 1600;

        /// <summary>
        /// TCP Log server port.
        /// </summary>
        public const int TCP_LOG_PORT = 1601;

        /// <summary>
        /// Default log level of logger.
        /// </summary>
        public const LogLevel DEFAULT_LOG_LEVEL = LogLevel.Information;

        /// <summary>
        /// TCP Command Poll interval. Used to keep connection alive.
        /// </summary>
        public const int TCP_POLL = 60;

        /// <summary>
        /// TCP Timeout. Server closes connection if no command received since more than timeout value.
        /// </summary>
        public const int TCP_TIMEOUT = 120;

        public const int CLIENT_DEFAULT_TIMEOUT = 15;

        /// <summary>
        /// Server configuration file name and path.
        /// </summary>
        public const string SERVER_CONF_PATH = "/var/local/iChooChoo/iChooChoo.conf";

        // Version of application.
        public const int PROG_VERSION_MAJ = 0;
        public const int PROG_VERSION_MIN = 40;
        public const int PROG_VERSION_BLD = 0;

        // Data size is Packet size - 4
        public const int PACKETSIZE = 20;
        public const int DATASIZE = 16;
        public const int DESCSIZE = 14;

        /// <summary>
        /// Maximum number of outputs on module.
        /// </summary>
        public const int MAX_OUTPUTS = 16;

        /// <summary>
        /// Minimum valid address value on I2C bus.
        /// </summary>
        public const byte ADDR_MIN = 0x08;

        /// <summary>
        /// Maximum valid address value on I2C bus.
        /// </summary>
        public const byte ADDR_MAX = 0x77;

        // Group values.
        public const byte BICCP_GRP_NEW = 0x00;
        public const byte BICCP_GRP_CONF = 0x01;
        public const byte BICCP_GRP_TRACTION = 0x10;
        public const byte BICCP_GRP_GENPURP = 0x20;
        public const byte BICCP_GRP_LIGHTING = 0x21;
        public const byte BICCP_GRP_UNKNOWN = 0xFF;

        // Configuration commands.
        public const byte BICCP_CMD_CONF_IDENT = 0x01;
        public const byte BICCP_CMD_CONF_VERSION = 0x02;
        public const byte BICCP_CMD_CONF_ADDR = 0x10;
        public const byte BICCP_CMD_CONF_TYPE = 0x11;
        public const byte BICCP_CMD_CONF_DESC = 0x12;
        public const byte BICCP_CMD_CONF_SOFTRST = 0xFE;
        public const byte BICCP_CMD_CONF_HARDRST = 0xFF;

        // General Purpose commands.
        public const byte BICCP_CMD_GENPURP_IDENT = 0x01;
        public const byte BICCP_CMD_GENPURP_PRESET = 0x10;
        public const byte BICCP_CMD_GENPURP_OUT_STR = 0x20;
        public const byte BICCP_CMD_GENPURP_OUT_END = 0x2F;

        // Lighting commands.
        public const byte BICCP_CMD_LIGHT_IDENT = 0x01;
        public const byte BICCP_CMD_LIGHT_PRESET = 0x10;
        public const byte BICCP_CMD_LIGHT_OUT_STR = 0x20;
        public const byte BICCP_CMD_LIGHT_OUT_DIMSTR = 0x2A;
        public const byte BICCP_CMD_LIGHT_OUT_END = 0x2F;
        public const byte BICCP_CMD_LIGHT_SCENSTART = 0x30;
        public const byte BICCP_CMD_LIGHT_SCENSTOP = 0x31;

        // Lighting scenarios.
        public const byte BICCP_SCN_LIGHT_PROGCHANGE = 0x01;
        public const byte BICCP_SCN_LIGHT_TUNGON = 0x02;
        public const byte BICCP_SCN_LIGHT_TUNGOFF = 0x03;
        public const byte BICCP_SCN_LIGHT_BLKLED = 0x10;
        public const byte BICCP_SCN_LIGHT_BLKOLD = 0x11;
        public const byte BICCP_SCN_LIGHT_TRFCFRA = 0x12;
        public const byte BICCP_SCN_LIGHT_TRFCDEU = 0x13;
        public const byte BICCP_SCN_LIGHT_CHASER = 0x14;
        public const byte BICCP_SCN_LIGHT_ARCWELDING = 0x15;
        public const byte BICCP_SCN_LIGHT_CAMERAFLASH = 0x16;
        public const byte BICCP_SCN_LIGHT_FIRE = 0x17;

        /// <summary>
        /// ICCP Commands enumeration.
        /// </summary>
        public enum ICCP_COMMANDS
        {
            DO_HARDRESET,
            DO_RESCAN,
            DO_SCENSTART,
            DO_SCENSTOP,
            DO_SETDIMOUT,
            DO_SETOUT,
            DO_SOFTRESET,
            SET_ADDR,
            SET_DESC,
            SET_TYPE
        }

        /// <summary>
        /// List of type string description.
        /// </summary>
        private static Dictionary<byte, string> _typeDescriptions;

        /// <summary>
        /// Get module type string description.
        /// </summary>
        /// <param name="type">Type of module.</param>
        /// <returns>String description.</returns>
        public static string GetTypeDescription(byte type)
        {
            PopulateTypes();

            if (_typeDescriptions.ContainsKey(type))
                return _typeDescriptions[type];
            else
                return null;
        }

        /// <summary>
        /// Gets the types string description dictionnary.
        /// </summary>
        public static Dictionary<byte, string> Types
        {
            get
            {
                PopulateTypes();
                return _typeDescriptions;
            }
        }

        /// <summary>
        /// Populates the types string description dictionnary.
        /// </summary>
        private static void PopulateTypes()
        {
            if (_typeDescriptions == null)
            {
                _typeDescriptions = new Dictionary<byte, string>();
                _typeDescriptions.Add(BICCP_GRP_NEW, "(new module)");
                _typeDescriptions.Add(BICCP_GRP_TRACTION, "Traction");
                _typeDescriptions.Add(BICCP_GRP_GENPURP, "General Purpose");
                _typeDescriptions.Add(BICCP_GRP_LIGHTING, "Lighting");
                _typeDescriptions.Add(BICCP_GRP_UNKNOWN, "(unknown module)");
            }
        }

        /// <summary>
        /// Get module types.
        /// </summary>
        /// <returns>List of module types.</returns>
        private static List<FieldInfo> GetModuleTypes()
        {
            return typeof(ICCConstants)
                .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.Name.StartsWith("BICCP_GRP_") && fi.Name != "BICCP_GRP_CONF")
                .ToList();
        }

        /// <summary>
        /// Checks if module type exists.
        /// </summary>
        /// <param name="type">Module type.</param>
        /// <returns>True if type exists, false otherwise.</returns>
        public static bool CheckIfTypeExists(byte type)
        {
            return GetModuleTypes().Select(x => (byte)(x.GetRawConstantValue())).Contains(type);
        }
    }
}
