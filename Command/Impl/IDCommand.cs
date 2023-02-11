namespace TownOfHost.Command.Impl;

public class IDCommand : Command
{
    public IDCommand() : base("id", false)
    {
        Canceled = true;
    }

    public override void OnExecute(PlayerControl player, string[] args)
    {
        if (!GameStates.InGame)
        {
            HelpCommand.SendMessage("请在游戏开始后使用", player);
            return;
        }

        string toSend = "";
        foreach (var playerControl in Main.AllAlivePlayerControls) toSend = toSend + playerControl.Data.PlayerName + " —— " + playerControl.PlayerId + "\n";
        HelpCommand.SendMessage(toSend, player);
    }
}