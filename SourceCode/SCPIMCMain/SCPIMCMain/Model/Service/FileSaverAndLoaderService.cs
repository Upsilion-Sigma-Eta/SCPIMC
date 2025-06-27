using System;

namespace SCPIMCMain.Model.Service;

public class FileSaverAndLoaderService
{
    public static string GetMacroFilePath(string id, string name)
    {
        return System.IO.Path.Combine(".", "Macro", $"{id}|{name}");
    }

    public static string GetDeviceFilePath(string ip, string port)
    {
        return System.IO.Path.Combine(".", "Device", $"{ip}|{port}");
    }

    public static string GetLogFilePath(string logType)
    {
        return System.IO.Path.Combine(".", "Logs", $"{logType}.log");
    }

    public static string 
}
