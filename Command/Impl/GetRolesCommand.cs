namespace TownOfHost.Command.Impl;

public class GetRolesCommand : Command
{
    public GetRolesCommand() : base("getRoles", false)
    {
        Canceled = true;
        Aliases = new[] { "gr" };
    }

    public override void OnExecute(PlayerControl player, string[] args)
    {
        if (!DebugModeManager.IsDebugMode)
        {
            HelpCommand.SendMessage("只有调试模式才能够使用这个命令", player);
            return;
        }

        if (!GameStates.InGame)
        {
            HelpCommand.SendMessage("请在游戏开始后使用", player);
            return;
        }

        string str = "";
        foreach (var target in Main.AllPlayerControls)
            str = str + "(" + target.Data.ColorName + ") " + target.Data.PlayerName + (target.IsAlive() ? "(存活)" : "(死亡)") + " —— " + Translator.GetString(target.GetCustomRole().ToString()) + "\n";
        HelpCommand.SendMessage(str, player);
    }
}