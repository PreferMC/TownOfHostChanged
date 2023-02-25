using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

public class Undercover : Role, IListener
{
    public Undercover() : base(16645, CustomRoles.Undercover)
    {
        Color = "#b22222";
        CanUseVent = true;
        DisplayName = "卧底";
        Description = "(船员阵营):\n内鬼会看到你是他们中的一员，但是内鬼靠近你后会显示击杀框。";
        Info = "对不起，我是警察！";
    }

    public override void Init()
    {
        CustomNames.Clear();
        foreach (var role in CustomRolesHelper.GetImpostorRoles()) CustomNames.Add(role, "<color=#ff0000>[Name]</color>");
    }
}