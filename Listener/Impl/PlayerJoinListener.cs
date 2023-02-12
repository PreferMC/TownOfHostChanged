
using InnerNet;
using TownOfHost.Command.Impl;

namespace TownOfHost.Listener.Impl;

public class PlayerJoinListener : IListener
{
    public void OnPlayerJoin(AmongUsClient auClient, ClientData client)
    {
        Utils.SendMessage(HelpCommand.HelpMessage, auClient.PlayerPrefab.PlayerId);
    }
}