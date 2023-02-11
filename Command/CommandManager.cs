using System.Collections.Generic;

namespace TownOfHost.Command;

public static class CommandManager
{
    private static readonly List<Command> Commands = new();

    public static void RegisterCommand(this Command command)
    {
        Commands.Add(command);
    }

    public static List<Command> GetCommands()
    {
        return Commands;
    }

    public static bool IsArgsContainsString(string str, string[] args)
    {
        foreach (var s in args)
            if (str.ToLower().Equals(s.ToLower()))
                return true;

        return false;
    }

    public static string DeleteCommandSymbol(string commandStr)
    {
        return commandStr.Replace("/", "");
    }

    public static string[] DeleteFirstArg(string[] args)
    {
        string[] toReturn = new string[args.Length - 1];
        // for (var i = 0; i < toReturn.Length; i++) toReturn[i] = args[i + 1];
        for (var i = 1; i < args.Length; i++)
        {
            toReturn[i - 1] = args[i];
        }

        return toReturn;
    }
}