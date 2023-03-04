using System.IO;

namespace TownOfHost.AppData;

public class AppDataLoader
{
    public static AppDataLoader Loader { get; } = new();
    private static readonly string AppDataDir = "TownOfHostChanged";

    public void Init()
    {
        Directory.CreateDirectory(AppDataDir);
    }
}