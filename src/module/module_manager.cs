namespace Zodiak;

public static class module_manager
{
    private static readonly List<module> modules = new();

    public static IReadOnlyList<module> all => modules;
    public static int count => modules.Count;

    public static T register<T>(T M) where T : module
    {
        modules.Add(M);
        return M;
    }

    public static module? find(string name)
    {
        for (int I = 0; I < modules.Count; I++)
        {
            if (string.Equals(modules[I].name, name, StringComparison.OrdinalIgnoreCase))
                return modules[I];
        }
        return null;
    }

    public static void update_all()
    {
        for (int I = 0; I < modules.Count; I++)
        {
            var M = modules[I];
            if (!M.enabled) continue;
            M.on_update(); 
        }
    }

    public static void disable_all()
    {
        for (int I = 0; I < modules.Count; I++)
        {
            try { modules[I].enabled = false; }
            catch { }
        }
    }
}