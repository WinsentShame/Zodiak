namespace Zodiak;

public static unsafe class entity_type
{
    private static readonly HashSet<nint> player_vtables = new();

    public static void register_player_vtable(nint vt)
    {
        if (vt != 0) player_vtables.Add(vt);
    }

    public static bool is_player(nint entity)
    {
        if (entity == 0) return false;
        nint vt = *(nint*)entity;
        return vt != 0 && player_vtables.Contains(vt);
    }
}