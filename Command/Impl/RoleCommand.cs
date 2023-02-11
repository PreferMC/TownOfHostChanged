namespace TownOfHost.Command.Impl;

public class RoleCommand : Command
{
    public RoleCommand() : base("role", false)
    {
        Aliases = new[] { "r" };
    }

    public override void OnExecute(PlayerControl player, string[] args)
    {
        if (args.Length <= 0)
        {
            HelpCommand.SendMessage(
                "查看职业介绍/r <职业>" + "\n" + "例如: /r 失忆者"
                , player);
            return;
        }

        for (var i = 0; i < args.Length; i++) args[i] = args[i].ToLower();

        var typeRole = Utils.GetRoleByName(args[0]);
        if (typeRole == null)
        {
            HelpCommand.SendMessage("找不到这个职业", player);
            return;
        }

        HelpCommand.SendMessage(
            Translator.GetString(typeRole.ToString()) + "\n" + Translator.GetString(typeRole + "InfoLong")
            , player);
    }
}