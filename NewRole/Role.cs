using AmongUs.GameOptions;

namespace TownOfHost.NewRole;

public class Role
{
    public int Id { get; }
    public CustomRoles CustomRole { get; }
    public TabGroup Group { get; set; }
    public string Color { get; set; }
    public bool HasTask { get; set; }
    public RoleTypes BaseRole { get; set; }
    public string Name { get; }
    public string DisplayName { get; set; }
    public string Description { get; set; }
    public string Info { get; set; }
    public bool SubRole { get; set; }
    public bool CanKill { get; set; }
    public float CurrentKillCooldown { get; set; }
    public bool CanUseVent { get; set; }
    public bool CanSabotage { get; set; }
    public bool CanUseAbility { get; set; }
    public bool HasVision { get; set; }

    public Role(int id, CustomRoles role)
    {
        Id = id;
        CustomRole = role;
        Color = "#ffffff";
        HasTask = true;
        BaseRole = RoleTypes.Crewmate;
        SubRole = false;
        Group = SubRole ? TabGroup.Addons : TabGroup.CrewmateRoles;
        CanKill = false;
        CurrentKillCooldown = 20.0F;
        CanUseVent = false;
        CanSabotage = false;
        CanUseAbility = false;
        HasVision = false;
        Name = role.ToString();
        DisplayName = Translator.GetString(Name);
        Description = Translator.GetString(Name + "LongInfo");
        Info = Translator.GetString(Name + "Info");
    }
}