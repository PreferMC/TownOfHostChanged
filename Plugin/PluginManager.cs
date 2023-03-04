using System.Collections.Generic;
using System.IO;
using TownOfHost.Plugin.Plugins;

namespace TownOfHost.Plugin;

public static class PluginManager
{
    private static readonly List<Plugin> Plugins = new();

    public static void RegisterPlugin(this Plugin plugin)
    {
        Plugins.Add(plugin);
    }

    public static List<Plugin> GetPlugins()
    {
        return Plugins;
    }

    public static void InitPluginSystem()
    {
        return;

        if (Directory.Exists("Plugins")) Directory.CreateDirectory("Plugins");

        DirectoryInfo root = new("Plugins");
        FileInfo[] files = root.GetFiles();
        foreach (var file in files)
        {
            if (!file.Name.ToLower().EndsWith(".js")) continue;

            RegisterPlugin(new JavaScriptPlugin(file, file.Name));
        }

        foreach (var plugin in Plugins)
            if (plugin is JavaScriptPlugin javaScriptPlugin)
                RunJavaScriptPlugin(javaScriptPlugin);
    }

    private static void RunJavaScriptPlugin(JavaScriptPlugin plugin)
    {
        var code = "";
        foreach (var readAllLine in File.ReadAllLines(plugin.FileInfo.FullName)) code = code + readAllLine + "\n";
/*
        using (ScriptEngine engine = new ScriptEngine("jscript"))
        {

        }*/
    }
}