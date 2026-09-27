namespace Zodiak;

public static unsafe class entity_type
{
    private static readonly HashSet<nint> player_vtables = new();
    private static nint local_vtable;
    private static bool resolved;

    private static bool resolve()
    {
        if (resolved) return true;
        nint ba = native_interop.get_module_handle_w(null);
        if (ba == 0) return false;

        local_vtable = ba + OFFSETS.DATA.LOCALPLAYER_VTABLE;
        resolved = true;
        return true;
    }

    public static void capture_local(nint entity)
    {
        if (entity == 0) return;
        nint vt = *(nint*)entity;
        if (vt == 0) return;
        player_vtables.Add(vt);
    }

    public static void capture_remote(nint entity)
    {
        if (entity == 0) return;
        nint vt = *(nint*)entity;
        if (vt == 0) return;
        player_vtables.Add(vt);
    }

    public static bool is_player(nint entity)
    {
        if (entity == 0) return false;

        nint vt = *(nint*)entity;
        if (vt == 0) return false;

        if (player_vtables.Contains(vt)) return true;

        if (resolve() && vt == local_vtable) return true;

        nint local = local_player.get();
        if (local != 0 && *(nint*)local == vt)
        {
            player_vtables.Add(vt);
            return true;
        }

        return false;
    }
}