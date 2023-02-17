using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

/*
 * 职业想法来自于
 * 2287967389(QQ)
 */
public class Butcher : Role, IListener
{
    public Butcher() : base(5525, CustomRoles.Butcher)
    {
        Color = "#cd5c5c";
        SubRoleCanJoinGroups.Clear();
        SubRoleCanJoinGroups.Add(TabGroup.ImpostorRoles); // 只有内鬼阵营才能够获得这个副职业

        DisplayName = "刽子手";
        Info = "手起刀落！";
        Description = "(附加职业):\n你可以击杀一名玩家而不瞬移，只有内鬼才可能是刽子手。";
        Group = TabGroup.Addons;
        SubRole = true;
        HasTask = false;
    }

    public bool OnPlayerMurderPlayer(PlayerControl killer, PlayerControl target)
    {
        if (!killer.GetCustomSubRoles().Contains(CustomRoles.Butcher)) return true;

        target.RpcMurderPlayer(target);

        return false;
    }
}