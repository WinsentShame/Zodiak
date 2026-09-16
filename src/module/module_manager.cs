namespace Zodiak;

public static class module_manager
{
    private static readonly List<module> _Modules = new();

    public static IReadOnlyList<module> all => _Modules;
    public static int count => _Modules.Count;

    public static T register<T>(T M) where T : module
    {
        _Modules.Add(M);
        return M;
    }

    public static module? find(string name)
    {
        for (int I = 0; I < _Modules.Count; I++)
        {
            if (string.Equals(_Modules[I].name, name, StringComparison.OrdinalIgnoreCase))
                return _Modules[I];
        }
        return null;
    }

    public static void update_all()
    {
        for (int I = 0; I < _Modules.Count; I++)
        {
            var M = _Modules[I];
            if (!M.enabled) continue;
            M.on_update(); 
        }
    }

    public static void disable_all()
    {
        for (int I = 0; I < _Modules.Count; I++)
        {
            try { _Modules[I].enabled = false; }
            catch { }
        }
    }
}