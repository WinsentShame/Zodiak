namespace Zodiak;

public static class module_manager
{
    private static readonly List<module> _modules = new();

    public static IReadOnlyList<module> all => _modules;
    public static int count => _modules.Count;

    public static T register<T>(T m) where T : module
    {
        _modules.Add(m);
        return m;
    }

    public static module? get(string name)
    {
        for (int i = 0; i < _modules.Count; i++)
            if (_modules[i].name == name) return _modules[i];
        return null;
    }

    public static void update_all()
    {
        for (int i = 0; i < _modules.Count; i++)
        {
            var m = _modules[i];
            if (!m.enabled) continue;
            try { m.on_update(); }
            catch (Exception ex) { logger.error("module", $"{m.name}: {ex.Message}"); }
        }
    }

    public static void disable_all()
    {
        for (int i = 0; i < _modules.Count; i++)
        {
            try { _modules[i].enabled = false; }
            catch { }
        }
    }
}