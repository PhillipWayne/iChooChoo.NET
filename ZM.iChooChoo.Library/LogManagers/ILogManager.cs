using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZM.iChooChoo.Library.LogManagers
{
    public enum LogLevel : int
    {
        Information = 0,
        Warning = 1,
        Debug = 2
    }

    public delegate void LogEventHandler(object sender, LogEventArgs e);

    public interface ILogManager
    {
        int MaxLogEntries { get; set; }

        LogLevel LogLevel { get; set; }

        string[] GetLogContent();

        void LogMessage(bool status, string message, byte address, LogLevel level = LogLevel.Information);

        void LogMessageToHex(bool status, string message, byte address, int data, LogLevel level = LogLevel.Information);

        void LogMessageToDec(bool status, string message, byte address, int data, LogLevel level = LogLevel.Information);

        void LogMessageText(bool status, string message, byte address, string data, LogLevel level = LogLevel.Information);

        void LogText(string message, LogLevel level = LogLevel.Information);
    }
}
