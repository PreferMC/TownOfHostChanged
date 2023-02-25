using System.Collections.Generic;
using TownOfHost.Listener;

namespace TownOfHost.NewRole;

public static class RoleManager
{
    private static readonly List<Role> Roles = new();

    public static void RegisterRole(this Role role)
    {
        // role.OptionItems = new();
        // Options.CustomRoleSpawnChances.TryGetValue(role.CustomRole, out StringOptionItem outValue);
        // if (outValue != null) role.MainOption = outValue;
        Roles.Add(role);
    }

    public static List<Role> GetRoles()
    {
        return Roles;
    }

    public static bool IsNewRole(this CustomRoles customRole)
    {
        foreach (var role in Roles)
            if (role.CustomRole == customRole)
                return true;
        return false;
    }

    public static Role? GetRoleByCustomRole(this CustomRoles customRole)
    {
        foreach (var role in Roles)
            if (role.CustomRole == customRole)
                return role;
        return null;
    }

    public static void RegisterRoleWithListener(this object role)
    {
        RegisterRole((role as Role)!);
        (role as IListener)!.RegisterListener();
    }
}