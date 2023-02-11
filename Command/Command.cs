namespace TownOfHost.Command;

public abstract class Command
{
    public string Name { get; }
    public bool HostOnly { get; }
    public bool Canceled { get; set; }
    public string[] Aliases { get; set; }

    public Command(string name, bool hostOnly)
    {
        Name = name;
        HostOnly = hostOnly;
        Aliases = new string[] {};
        Canceled = true;
    }

    public abstract void OnExecute(PlayerControl player, string[] args);

}