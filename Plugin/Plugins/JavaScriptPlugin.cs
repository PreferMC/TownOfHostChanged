using System.IO;

namespace TownOfHost.Plugin.Plugins;

public class JavaScriptPlugin : Plugin
{
    public FileInfo FileInfo { get; }

    public string Name { get; }

    public JavaScriptPlugin(FileInfo fileInfo, string name)
    {
        FileInfo = fileInfo;
        Name = name;
    }
}