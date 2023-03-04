using System;
using YamlDotNet.RepresentationModel;

namespace TownOfHost.AppData.Config;

public class Config
{
    public static bool Experiment = false;

    public static void Init()
    {
        YamlStream yaml = new();
        yaml.Load(AppDataLoader.ConfigData.ReadText());

        var mapping = (YamlMappingNode)yaml.Documents[0].RootNode;
        foreach (var entry in mapping.Children)
        {
            // Main.Logger.LogInfo(((YamlScalarNode)entry.Key).Value);
            var key = ((YamlScalarNode)entry.Key);
            var value = ((YamlScalarNode)entry.Value);

            if (key.Value == null || value.Value == null) continue;

            if (key.Value.Equals("Experiment")) Experiment = Boolean.Parse(value.Value);
        }
    }
}