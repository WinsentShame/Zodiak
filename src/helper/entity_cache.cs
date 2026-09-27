using System.Collections.Generic;

namespace Zodiak;

public static class entity_cache
{
    private static readonly List<nint> entities = new();

    public static void push(nint entity)
    {
        if (entity != 0) entities.Add(entity);
    }

    public static IReadOnlyList<nint> all() => entities;

    public static void clear() => entities.Clear();
}