namespace Zodiak;

public static class module_manager
{
    private static readonly List<module> modules = new();

    public static IReadOnlyList<module> ALL => modules;
    public static int COUNT => modules.Count;

    public static T register<T>(T m) where T : module
    {
        modules.Add(m);
        return m;
    }

    public static module? find(string name)
    {
        for (int i = 0; i < modules.Count; i++)
        {
            if (string.Equals(modules[i].NAME, name, StringComparison.OrdinalIgnoreCase))
                return modules[i];
        }
        return null;
    }

    public static void update_all()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            var m = modules[i];
            if (!m.ENABLED) continue;
            m.on_update();
        }
    }

    public static void disable_all()
    {
        for (int i = 0; i < modules.Count; i++)
        {
            try { modules[i].ENABLED = false; }
            catch { }
        }
    }
}