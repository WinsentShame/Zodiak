namespace Zodiak;

public static unsafe class esp_renderer
{
    private const float EYE_OFFSET = 1.62f;
    private const float PLAYER_PADDING = 0.03f;
    private const float CHEST_MARGIN = 0.0625f;
    private const int edge_count = 12;

    private static readonly int[] edges_a = { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3 };
    private static readonly int[] edges_b = { 1, 2, 3, 0, 5, 6, 7, 4, 4, 5, 6, 7 };

    public static void draw(float partial)
    {
        nint ba = native_interop.get_module_handle_w(null);
        nint player = local_player.get();

        if (ba == 0
            || !tessellator.resolve()
            || !tessellator.valid()
            || !local_player.is_valid(player))
        {
            entity_cache.clear();
            chest_cache.clear();
            return;
        }

        if (partial < 0f) partial = 0f;
        if (partial > 1f) partial = 1f;

        float* origin = (float*)(ba + OFFSETS.FUNC.G_RENDER_ORIGIN);
        float cx = origin[0], cy = origin[1], cz = origin[2];

        nint ctx = ba + OFFSETS.FUNC.RENDER_CTX;

        float* shader_color = (float*)(ba + OFFSETS.FUNC.SHADER_COLOR);
        byte* shader_flag = (byte*)(ba + OFFSETS.FUNC.SHADER_COLOR_SET);

        if (tessellator.needs_flush())
            tessellator.end_flush();

        byte flag_saved = *shader_flag;

        *shader_flag = 1;
        shader_color[0] = 1f;
        shader_color[1] = 1f;
        shader_color[2] = 1f;
        shader_color[3] = 1f;

        if (esp.PLAYER_MODE)
            draw_players(player, partial, cx, cy, cz, ctx);

        if (esp.CHEST_MODE)
            draw_chests(cx, cy, cz, ctx);

        *shader_flag = flag_saved;

        tessellator.end_flush();

        entity_cache.clear();
        //chest_cache.clear();
    }

    private static void draw_players(nint player, float partial,
                                     float cx, float cy, float cz, nint ctx)
    {
        foreach (nint entity in entity_cache.all())
        {
            if (entity == 0) continue;
            if (entity == player) continue;

            if (!memory.is_readable(entity + OFFSETS.FIELD.ENTITY_POS_OLD_X, 24)) continue;
            if (!memory.is_readable(entity + OFFSETS.FIELD.ENTITY_HITBOX_WIDTH, 8)) continue;
            if (!entity_check.is_real_player(entity)) continue;

            float px_old = *(float*)(entity + OFFSETS.FIELD.ENTITY_POS_OLD_X);
            float py_old = *(float*)(entity + OFFSETS.FIELD.ENTITY_POS_OLD_Y);
            float pz_old = *(float*)(entity + OFFSETS.FIELD.ENTITY_POS_OLD_Z);

            float px_new = *(float*)(entity + OFFSETS.FIELD.ENTITY_POS_X);
            float py_new = *(float*)(entity + OFFSETS.FIELD.ENTITY_POS_Y);
            float pz_new = *(float*)(entity + OFFSETS.FIELD.ENTITY_POS_Z);

            float px = px_old + (px_new - px_old) * partial;
            float py = py_old + (py_new - py_old) * partial;
            float pz = pz_old + (pz_new - pz_old) * partial;

            float full_w = *(float*)(entity + OFFSETS.FIELD.ENTITY_HITBOX_WIDTH);
            float full_h = *(float*)(entity + OFFSETS.FIELD.ENTITY_HITBOX_HEIGHT);

            if (full_w < 0.01f || full_w > 20f) continue;
            if (full_h < 0.01f || full_h > 20f) continue;

            full_w += PLAYER_PADDING;
            full_h += PLAYER_PADDING;

            float half_w = full_w * 0.5f;

            float ex = px - cx;
            float ey = py - cy - EYE_OFFSET - PLAYER_PADDING * 0.5f;
            float ez = pz - cz;

            float x0 = ex - half_w, x1 = ex + half_w;
            float y0 = ey, y1 = ey + full_h;
            float z0 = ez - half_w, z1 = ez + half_w;

            emit_box(x0, y0, z0, x1, y1, z1, ctx);
        }
    }

    private static void draw_chests(float cx, float cy, float cz, nint ctx)
    {
        foreach (var (bx, by, bz) in chest_cache.all())
        {
            float ex = bx - cx;
            float ey = by - cy;
            float ez = bz - cz;

            float x0 = ex + CHEST_MARGIN, x1 = ex + 1f - CHEST_MARGIN;
            float y0 = ey, y1 = ey + 1f;
            float z0 = ez + CHEST_MARGIN, z1 = ez + 1f - CHEST_MARGIN;

            emit_box(x0, y0, z0, x1, y1, z1, ctx);
        }
    }

    private static void emit_box(float x0, float y0, float z0,
                                 float x1, float y1, float z1, nint ctx)
    {
        float* corners_x = stackalloc float[8] { x0, x1, x1, x0, x0, x1, x1, x0 };
        float* corners_y = stackalloc float[8] { y0, y0, y0, y0, y1, y1, y1, y1 };
        float* corners_z = stackalloc float[8] { z0, z0, z1, z1, z0, z0, z1, z1 };

        tessellator.begin(4, edge_count * 2);
        tessellator.colour(255, 255, 255, 255);

        for (int e = 0; e < edge_count; e++)
        {
            int a = edges_a[e];
            int b = edges_b[e];
            tessellator.vertex(corners_x[a], corners_y[a], corners_z[a]);
            tessellator.vertex(corners_x[b], corners_y[b], corners_z[b]);
        }

        tessellator.draw(ctx, 0);
    }
}