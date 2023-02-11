using InnerNet;
using TownOfHost.Command;
using TownOfHost.Command.Impl;

namespace TownOfHost.Listener.Impl;

public class PlayerJoinListener : IListener
{
    public void OnPlayerJoin(AmongUsClient auClient, ClientData client)
    {
        foreach (var command in CommandManager.GetCommands())
            if (command is HelpCommand) command.OnExecute(auClient.PlayerPrefab, new string[] {});
    }
}