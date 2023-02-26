using InnerNet;

namespace TownOfHost.Listener;

/*
 * 这种监听器写法多少有点烂了
 * 下一步打算用注解式(Java里面这样叫，C#我不知道)
 * 暂且用着吧 (
 */
public interface IListener
{
    bool OnPlayerReportBody(PlayerControl reporter, GameData.PlayerInfo target) { return true;}

    bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target) { return true; }

    void OnGameStarted(AmongUsClient client) { }

    void OnPlayerSendChat(PlayerControl player, string text) { }

    bool OnOwnerSendChat(ChatController chat) { return true; }

    void OnPlayerJoin(AmongUsClient auClient, ClientData client) { }

    void OnPlayerLeft(AmongUsClient client, ClientData data, DisconnectReasons reason) { }

    void OnCreatePlayer(AmongUsClient auClient, ClientData client) { }

    void OnPlayerFixedUpdate(PlayerControl player) { }

    void OnOptionHolderRegister() { }

    void OnPlayerShapeShift(PlayerControl shapeShifter, PlayerControl target) { }

    void OnPlayerExiled(GameData.PlayerInfo exiled) { }
}
