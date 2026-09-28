namespace Zodiak;

public static unsafe class hitbox_state
{
    private static readonly Dictionary<nint, (float w, float h)> saved = new();

    public static void apply(nint entity, float w_mult, float h_mult)
    {
        if (entity == 0) return;
        if (!memory.is_readable(entity + OFFSETS.FIELD.ENTITY_HITBOX_WIDTH, 8)) return;

        if (!saved.TryGetValue(entity, out var orig))
        {
            float w = *(float*)(entity + OFFSETS.FIELD.ENTITY_HITBOX_WIDTH);
            float h = *(float*)(entity + OFFSETS.FIELD.ENTITY_HITBOX_HEIGHT);

            if (w < 0.01f || w > 10f || h < 0.01f || h > 10f) return;

            orig = (w, h);
            saved[entity] = orig;
        }

        *(float*)(entity + OFFSETS.FIELD.ENTITY_HITBOX_WIDTH) = orig.w * w_mult;
        *(float*)(entity + OFFSETS.FIELD.ENTITY_HITBOX_HEIGHT) = orig.h * h_mult;
    }

    public static void restore_all()
    {
        foreach (var kvp in saved)
        {
            nint e = kvp.Key;
            if (!memory.is_readable(e + OFFSETS.FIELD.ENTITY_HITBOX_WIDTH, 8)) continue;
            *(float*)(e + OFFSETS.FIELD.ENTITY_HITBOX_WIDTH) = kvp.Value.w;
            *(float*)(e + OFFSETS.FIELD.ENTITY_HITBOX_HEIGHT) = kvp.Value.h;
        }
        saved.Clear();
    }

    public static void forget() => saved.Clear();
}