using System.Collections.Generic;

namespace TownOfHost.Listener;

public static class ListenerManager
{
    private static readonly List<IListener> Listeners = new();

    public static void RegisterListener(this IListener listener)
    {
        Listeners.Add(listener);
    }

    public static List<IListener> GetListeners()
    {
        return Listeners;
    }
}