using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

public class Aggressor : Role, IListener
{
    public Aggressor() : base(2355, CustomRoles.Aggressor)
    {
        Color = "#7fffaa";
        DisplayName = "挑衅者";
        Description = "(船员阵营):\n当你被击杀后，你会和击杀者同归于尽。";
        Info = "你将会倒大霉！";
    }

    public bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target)
    {
        if (target.GetCustomRole() == CustomRole) killer.RpcMurderPlayer(killer);

        return true;
    }
}