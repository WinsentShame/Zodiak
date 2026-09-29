namespace Zodiak;

public static class hook_registry
{
    private static readonly List<hook_group> hooks = new();

    public static void register(hook_group h) => hooks.Add(h);

    public static void uninstall_all()
    {
        for (int i = hooks.Count - 1; i >= 0; i--)
        {
            try { hooks[i].uninstall(); }
            catch { }
        }
        hooks.Clear();
    }
}