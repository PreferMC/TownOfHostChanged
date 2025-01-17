namespace TownOfHost.Command.Impl;

public class HelpCommand : Command
{
    public static readonly string HelpMessage = "本房间安装了Town Of Host Changed模组"
                                                + "\n版本：" + Main.PluginVersion
                                                + "\n模组QQ交流群：250164479"
                                                + "\n命令列表："
                                                + "\n/h contributor(con) —— 查看模组贡献者列表"
                                                + "\n/h newRole(nr) —— 查看所有添加的新职业(不包含TOH原生职业)"
                                                + "\n/role(r) <职业> —— 查看职业信息"
                                                + "\n/id —— 查看所有玩家ID"
                                                + "\n/n r —— 查看所有启用的职业"
                                                + "\n/m —— 查看当前职业介绍";

    public override void OnExecute(PlayerControl player, string[] args)
    {
        if (args.Length <= 0)
        {
            var helpMsg = HelpMessage;

            if (player.PlayerId == PlayerControl.LocalPlayer.PlayerId)
                helpMsg = helpMsg
                          + "\n/rn <名称> —— 更改名字"
                          + "\n/hn —— 隐藏名称";
            SendMessage(helpMsg, player);
            return;
        }

        for (var i = 0; i < args.Length; i++) args[i] = args[i].ToLower();

        switch (args[0])
        {
            case "con":
            case "contributor":
                SendMessage(ContributorMessage, player);
                break;
            default:
                SendMessage("找不到这个子命令，输入/h查看帮助", player);
                break;
            case "nr":
            case "newRole":
                string text = "";
                foreach (var role in NewRole.RoleManager.GetRoles())
                    text = text + Translator.GetString(role.CustomRole.ToString()) + GetStringOfGroup(role.Group) + " —— " +
                           Translator.GetString(role.CustomRole + "Info") + "\n";
                SendMessage(text, player);
                break;
        }
    }

    private static string GetStringOfGroup(TabGroup group)
    {
        switch (group)
        {
            case TabGroup.Addons:
                return "(副职业)";
            case TabGroup.CrewmateRoles:
                return "(船员)";
            case TabGroup.ImpostorRoles:
                return "(内鬼)";
            case TabGroup.NeutralRoles:
                return "(中立)";
            default:
                return "(未知)";
        }
    }

    public static void SendMessage(string msg, PlayerControl player) => Utils.SendMessage(msg, player.PlayerId);

    public HelpCommand() : base("help", false)
    {
        Aliases = new[] { "h" };
    }

    private static readonly string ContributorMessage =
        "commandf1 —— 项目发起者" + "\n" +
        "KARPED1EM —— 翻译文件支持 & 技术支持" + "\n";
}