namespace TownOfHost.Listener;

public interface IListener
{
    void OnPlayerReportBody(PlayerControl reporter, GameData.PlayerInfo target) { }

    bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target) { return true; }

    void OnGameStarted(AmongUsClient client) { }

    void OnPlayerSendChat(PlayerControl player, string text) { }

    bool OnOwnerSendChat(ChatController chat) { return true; }
}
