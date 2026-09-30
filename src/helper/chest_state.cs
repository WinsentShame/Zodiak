namespace Zodiak;

public static class chest_cache
{
    private static readonly Dictionary<(int x, int y, int z), byte> entries = new();

    public static void mark(int x, int y, int z)
    {
        entries[(x, y, z)] = 1;
    }

    public static void prune(float player_x, float player_y, float player_z,
                             float hard_radius)
    {
        float r2 = hard_radius * hard_radius;
        var to_remove = new List<(int, int, int)>();

        foreach (var pos in entries.Keys)
        {
            float dx = pos.x - player_x;
            float dy = pos.y - player_y;
            float dz = pos.z - player_z;

            if (dx * dx + dy * dy + dz * dz > r2)
                to_remove.Add(pos);
        }

        for (int i = 0; i < to_remove.Count; i++)
            entries.Remove(to_remove[i]);
    }

    public static IReadOnlyList<(int x, int y, int z)> all()
    {
        var list = new List<(int x, int y, int z)>(entries.Count);
        foreach (var k in entries.Keys)
            list.Add(k);
        return list;
    }

    public static void clear() => entries.Clear();
}

public static unsafe class chest_scanner
{
    private const int SOFT_RADIUS_XZ = 24;
    private const int SOFT_RADIUS_UP = 6;
    private const int SOFT_RADIUS_DOWN = 6;
    private const float HARD_RADIUS = 64f;
    private const int SCAN_INTERVAL = 20;

    private const byte CHEST_ID = 54;
    private const byte TRAPPED_CHEST_ID = 146;

    private static int tick_counter;

    public static void tick()
    {
        if (!esp.ACTIVE || !esp.CHEST_MODE) return;

        nint player = local_player.get();
        if (!local_player.is_valid(player)) return;
        if (!memory.is_readable(player + OFFSETS.FIELD.ENTITY_POS_X, 12)) return;

        float px = *(float*)(player + OFFSETS.FIELD.ENTITY_POS_X);
        float py = *(float*)(player + OFFSETS.FIELD.ENTITY_POS_Y);
        float pz = *(float*)(player + OFFSETS.FIELD.ENTITY_POS_Z);

        tick_counter++;
        if (tick_counter < SCAN_INTERVAL) return;
        tick_counter = 0;

        if (!memory.is_readable(player + OFFSETS.FIELD.ENTITY_BLOCK_SOURCE, 8)) return;

        nint block_source = *(nint*)(player + OFFSETS.FIELD.ENTITY_BLOCK_SOURCE);
        if (block_source == 0) return;

        nint ba = native_interop.get_module_handle_w(null);
        if (ba == 0) return;

        var get_block_id = (get_block_id_sig)(ba + OFFSETS.FUNC.BLOCK_SOURCE_GET_BLOCK_ID);

        int cx = (int)MathF.Floor(px);
        int cy = (int)MathF.Floor(py);
        int cz = (int)MathF.Floor(pz);

        int* pos = stackalloc int[3];
        byte* id_out = stackalloc byte[1];

        for (int x = cx - SOFT_RADIUS_XZ; x <= cx + SOFT_RADIUS_XZ; x++)
        {
            for (int z = cz - SOFT_RADIUS_XZ; z <= cz + SOFT_RADIUS_XZ; z++)
            {
                for (int y = cy - SOFT_RADIUS_DOWN; y <= cy + SOFT_RADIUS_UP; y++)
                {
                    pos[0] = x;
                    pos[1] = y;
                    pos[2] = z;

                    *id_out = 0;
                    get_block_id(block_source, (nint)id_out, (nint)pos);

                    byte id = *id_out;
                    if (id == CHEST_ID || id == TRAPPED_CHEST_ID)
                        chest_cache.mark(x, y, z);
                }
            }
        }

        chest_cache.prune(px, py, pz, HARD_RADIUS);
    }

    public static void clear()
    {
        tick_counter = 0;
        chest_cache.clear();
    }
}