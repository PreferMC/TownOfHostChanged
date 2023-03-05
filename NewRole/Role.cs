using System.Collections.Generic;
using AmongUs.GameOptions;

namespace TownOfHost.NewRole;

public class Role
{
    public int Id { get; } // 职业唯一ID
    public CustomRoles CustomRole { get; } // 职业在CustomRoles枚举类的Value
    public TabGroup Group { get; set; } // 职业阵营
    public string Color { get; set; } // 职业颜色
    public bool HasTask { get; set; } // 职业是否拥有任务
    public RoleTypes BaseRole { get; set; } // 职业的基础职业
    public string Name { get; } // 职业名称
    public string DisplayName { get; set; } // 职业显示名称
    public string Description { get; set; } // 职业介绍
    public string Info { get; set; } // 职业开始游戏显示介绍
    public bool SubRole { get; set; } // 是否为副职业
    public bool CanKill { get; set; } // 是否可以击杀
    public float CurrentKillCooldown { get; set; } // 击杀冷却时间
    public bool CanUseVent { get; set; } // 是否可以使用管道
    public bool CanSabotage { get; set; } // 是否能够破坏
    public bool CanUseAbility { get; set; } // 是否能够使用能力
    public bool HasVision { get; set; } // 是否拥有内鬼视野
    public bool IsMadMate { get; set; } // 是否为叛徒(理论上已经开发完成)
    public List<TabGroup> SubRoleCanJoinGroups { get; } // 副职业可以被赋予职业的阵营
    public bool GiveRoleOnStart { get; set; } // 是否在开始游戏时给予这个职业

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
        CurrentKillCooldown = 20.0f;
        CanUseVent = false;
        CanSabotage = false;
        CanUseAbility = false;
        HasVision = false;
        IsMadMate = false;
        GiveRoleOnStart = true;
        SubRoleCanJoinGroups = new()
        {
            TabGroup.CrewmateRoles,
            TabGroup.ImpostorRoles,
            TabGroup.NeutralRoles,
        };
        // CustomNames = new();
        Name = role.ToString();
        DisplayName = Translator.GetString(Name);
        Description = Translator.GetString(Name + "LongInfo");
        Info = Translator.GetString(Name + "Info");
    }

    /*
     * 对于随时可以变的量
     */
    public virtual float GetCurrentKillCooldown()
    {
        return CurrentKillCooldown;
    }

    public virtual void Init() { }

    public virtual void SetupOptions()
    {
        Options.SetupRoleOptions(Id, Group, CustomRole);
    }

    public virtual string TargetMark(PlayerControl seer, PlayerControl target)
    {
        return "";
    }
}
