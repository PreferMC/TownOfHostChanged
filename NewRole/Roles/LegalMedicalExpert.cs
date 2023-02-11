using System;
using System.Collections.Generic;
using AmongUs.GameOptions;
using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

public class LegalMedicalExpert : Role, IListener
{
    private static readonly Dictionary<byte, byte> ShieldPlayers = new(); // 第一个是给盾的人，第二个是被给盾的人
    private static readonly Dictionary<byte, long> DeadTime = new();

    public LegalMedicalExpert() : base(23333, CustomRoles.LegalMedicalExpert)
    {
        Color = "#add8e6";
        CanKill = true;
        CurrentKillCooldown = 30f;
        HasTask = false;
        DisplayName = "法医";
        Description = "(船员阵营):\n你可以通过报告来查看死亡时间，你也可以通过击杀来给你信任的船员护盾(在你死亡或者退出前该船员不会受到任何伤害，如果出现守护天使守护盾牌说明盾牌给予成功，反之的失败，失败原因可能是因为你已经给过其他船员盾牌了)。";
        Info = "让我看看新鲜的大鸡腿！";
        BaseRole = RoleTypes.Impostor;
    }

    public void OnPlayerReportBody(PlayerControl reporter, GameData.PlayerInfo target)
    {
        if (reporter == null || target == null!) return;

        if (reporter.GetCustomRole() != CustomRoles.LegalMedicalExpert) return;

        foreach (var pair in DeadTime)
            if (pair.Key == target.PlayerId)
            {
                new LateTask(() =>
                {
                    Utils.SendMessage("看来死者死亡时间大约在在 " + (DateTime.Now.Ticks - pair.Value) / 8000 + " 秒之前" , reporter.PlayerId, "★ 法医信息 ★");
                }, 5.0f, "LegalMedicalExpert Task");
                return;
            }
    }

    public bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target)
    {
        if (killer == null || target == null) return true;

        if (killer.GetCustomRole() == CustomRoles.LegalMedicalExpert)
        {
            if (!ShieldPlayers.ContainsKey(killer.PlayerId))
            {
                ShieldPlayers.Add(killer.PlayerId, target.PlayerId);
                killer.RpcGuardAndKill(target);
                return false;
            }

            return false;
        }

        if (ShieldPlayers.ContainsValue(target.PlayerId))
            foreach (var keyValuePair in ShieldPlayers) /* 这里写的有些多余了，懒得改了 */
                if (keyValuePair.Value == target.PlayerId)
                {
                    var player = Utils.GetPlayerById(keyValuePair.Key);
                    if (player != null && player.IsAlive())
                    {
                        killer.RpcGuardAndKill(target);
                        return false;
                    }
                }

        // 如果上面检测全都不成立，那么这个死者应该被加入进来
        DeadTime.Add(target.PlayerId, DateTime.Now.Ticks);
        return true;
    }

    public void OnGameStarted(AmongUsClient client)
    {
        // 游戏开始时对职业进行初始化
        ShieldPlayers.Clear();
        DeadTime.Clear();
    }
}