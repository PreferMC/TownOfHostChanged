using TownOfHost.Listener;

namespace TownOfHost.NewRole.Roles;

/*
 * 副职业赌怪
 * 开发者开发副职业可以参考这个格式
 */
public class Guesser : Role, IListener
{
    public Guesser() : base(4545112, CustomRoles.Guesser)
    {
        SubRole = true;
        Color = "#ff6666";
        DisplayName = "赌怪";
        Info = "生命就是豪赌！";
        Group = TabGroup.Addons;
    }

    public bool OnOwnerSendChat(ChatController chat)
    {
        var commandList = new[] {"/bt", "/gs", "/cc", "/guess"};
        bool isTypeCommand = false;
        foreach (var s in commandList)
            if (chat.TextArea.text.ToLower().StartsWith(s)) isTypeCommand = true;
        if (isTypeCommand)
        {
            OnPlayerSendChat(PlayerControl.LocalPlayer, chat.TextArea.text);
            if (_canceled)
            {
                chat.TextArea.Clear();
                chat.TextArea.SetText("");
                chat.quickChatMenu.ResetGlyphs();
                _canceled = false;
                return false;
            }
        }
        return true;
    }

    private bool _canceled;

    private void SendMessage(string msg, PlayerControl player) => Utils.SendMessage(msg, player.PlayerId);


    /*
    * 这是一个屎山代码，你可以改掉他，但是不建议。
    * 能跑起来为什么要改？
    */
    public void OnPlayerSendChat(PlayerControl player, string text)
    {
        string[] args = text.ToLower().Split(' ');

        switch (args[0])
        {
            case "/bt":
            case "/gs":
            case "/cc":
            case "/guess":
                _canceled = true;
                if (!GameStates.InGame)
                {
                    SendMessage("游戏开始后才能进行赌注", player);
                    return;
                }

                if (!player.IsAlive())
                {
                    SendMessage("死亡后不能进行赌注", player);
                    return;
                }

                if (!player.GetCustomSubRoles().Contains(CustomRoles.Guesser))
                {
                    SendMessage("你不是赌怪", player);
                    return;
                }

                if (args.Length != 3)
                {
                    SendMessage("错误的语法，正确语法：/guess <玩家ID> <猜测职业>" + "\n" + "获取所有玩家ID方法：/id", player);
                    return;
                }

                if (!int.TryParse(args[1], out int outNum))
                {
                    SendMessage("玩家ID必须是数字！", player);
                    return;
                }

                var target = Utils.GetPlayerById(outNum);
                if (target == null)
                {
                    SendMessage("找不到这个玩家，请输入/id检查", player);
                    return;
                }
                var typeRole = Utils.GetRoleByName(args[2]);
                if (typeRole == null)
                {
                    SendMessage("找不到这个职业，请检查拼写。", player);
                    return;
                }

                if (target.GetCustomRole() == typeRole)
                {
                    player.RpcMurderPlayer(target);
                    Utils.SendMessage(target.Data.PlayerName + " 在赌局中失利了！");
                }
                else
                {
                    player.RpcMurderPlayer(player);
                    Utils.SendMessage(player.Data.PlayerName + " 在赌局中失利了！");
                }
                return;
        }
    }
}