
using InnerNet;
using TownOfHost.Command.Impl;

namespace TownOfHost.Listener.Impl;

public class PlayerJoinListener : IListener
{
    public void OnCreatePlayer(AmongUsClient auClient, ClientData client)
    {
        new LateTask(() =>
        {
            Utils.SendMessage(HelpCommand.HelpMessage, client.Character.PlayerId);
        }, 3f, "PlayerJoinSendMessage");
    }
}