namespace Zodiak;

public static class entity_cache
{
    private static readonly List<nint> entities = new();
    private static readonly HashSet<nint> seen = new();

    public static void push(nint entity)
    {
        if (entity == 0) return;
        if (!seen.Add(entity)) return;
        entities.Add(entity);
    }

    public static IReadOnlyList<nint> all() => entities;

    public static void clear()
    {
        entities.Clear();
        seen.Clear();
    }
}