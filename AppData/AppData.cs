using System.IO;

namespace TownOfHost.AppData;

public class AppData
{
    public string Path { get; }

    public AppData(string path)
    {
        Path = @"./TOH_DATA/" + path;
    }

    public StreamReader ReadText()
    {
        return File.OpenText(Path);
    }
}