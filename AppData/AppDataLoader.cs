using System.IO;

namespace TownOfHost.AppData;

public class AppDataLoader
{
    public static AppData ConfigData = null!;
    private int _read;

    public static void Init()
    {
        ConfigData = new AppData("config.yml");
        /*if (!File.Exists(ConfigData.Path))
        {
            File.Create(ConfigData.Path);
            WriteToFileFromResource(ConfigData.Path, "config.yml");
        }*/

        Config.Config.Init();
    }
/*
    public static void WriteToFileFromResource(string path, string resource)
    {
        var assembly = System.Reflection.Assembly.GetExecutingAssembly();
        var stream = assembly.GetManifestResourceStream("TownOfHost.Resources." + resource);

        if (stream == null) return;

        // File.WriteAllBytes(path, StreamToBytes(stream));


        using (FileStream fs = new(path, FileMode.Create))
        {
            var bs = StreamToBytes(stream);
            fs.Write(bs, 0, bs.Length);
        }
    }

    public static byte[] StreamToBytes(Stream stream)
    {
        byte[] bytes = new byte[stream.Length];
        stream.Read(bytes, 0, bytes.Length);
        stream.Seek(0, SeekOrigin.Begin);
        return bytes;
    }*/
}