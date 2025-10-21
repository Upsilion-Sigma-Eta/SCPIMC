namespace SCPIMCMain.Model.Service;

public class FileSaverAndLoaderService
{
    public static string Func_GetMacroFilePath(string __id, string __name)
    {
        return System.IO.Path.Combine(".", "Macro", $"{__id}|{__name}");
    }

    public static string Func_GetDeviceFilePath(string __ip, string __port)
    {
        return System.IO.Path.Combine(".", "Device", $"{__ip}|{__port}");
    }

    public static string Func_GetLogFilePath(string __logType)
    {
        return System.IO.Path.Combine(".", "Logs", $"{__logType}.log");
    }

    public static string Func_GetSettingsFilePath()
    {
        return System.IO.Path.Combine(".", "Settings", "appsettings.json");
    }
}
