using InnerNet;

namespace TownOfHost.Listener;

public interface IListener
{
    void OnPlayerReportBody(PlayerControl reporter, GameData.PlayerInfo target) { }

    bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target) { return true; }

    void OnGameStarted(AmongUsClient client) { }

    void OnPlayerSendChat(PlayerControl player, string text) { }

    bool OnOwnerSendChat(ChatController chat) { return true; }

    void OnPlayerJoin(AmongUsClient auClient, ClientData client) { }

    void OnPlayerLeft(AmongUsClient client, ClientData data, DisconnectReasons reason) { }

    void OnGameJoin(AmongUsClient client) { }
}
