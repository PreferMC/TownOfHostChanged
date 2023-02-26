namespace TownOfHost.Listener.Impl;

public class MeetingStartListener : IListener
{
    public bool OnPlayerReportBody(PlayerControl reporter, GameData.PlayerInfo target)
    {
        if (target != null!) return true; // 判断是否是启动会议，而不是报告尸体

        foreach (var playerControl in Main.AllAlivePlayerControls)
            if (playerControl.shapeshifting) // 如果这个人正在变形期间
              FixShapeShiftBug(playerControl);

        return true;
    }

    private void FixShapeShiftBug(PlayerControl player)
    {
        player.RpcRevertShapeshift(false); // 应该重新设置回原型，并且没有动画
        Main.Logger.LogWarning(player._cachedData.PlayerName + " 触发了 变形BUG 已经修复");
    }
}