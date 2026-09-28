namespace Zodiak;

public static unsafe class hitbox_renderer
{
    private const float half_w = 0.3f;
    private const float height = 1.8f;
    private const int edge_count = 12;

    private static readonly int[] edges_a = { 0, 1, 2, 3, 4, 5, 6, 7, 0, 1, 2, 3 };
    private static readonly int[] edges_b = { 1, 2, 3, 0, 5, 6, 7, 4, 4, 5, 6, 7 };

    private static nint tessellator;
    private static bool resolved;

    private static bool resolve()
    {
        if (resolved) return tessellator != 0;
        nint ba = native_interop.get_module_handle_w(null);
        if (ba == 0) return false;
        tessellator = ba + OFFSETS.FUNC.G_TESSELLATOR;
        resolved = true;
        return true;
    }

    public static void draw_all(float partial)
    {
        if (!resolve())
        {
            entity_cache.clear();
            return;
        }

        if (!memory.is_readable(tessellator, 0x140))
        {
            entity_cache.clear();
            return;
        }

        nint ba = native_interop.get_module_handle_w(null);
        if (ba == 0)
        {
            entity_cache.clear();
            return;
        }

        nint player = local_player.get();
        if (!local_player.is_valid(player))
        {
            entity_cache.clear();
            return;
        }

        if (partial < 0f) partial = 0f;
        if (partial > 1f) partial = 1f;

        float* origin = (float*)(ba + OFFSETS.FUNC.G_RENDER_ORIGIN);
        float cx = origin[0], cy = origin[1], cz = origin[2];

        var begin = (tessellator_begin_sig)(ba + OFFSETS.FUNC.TESSELLATOR_BEGIN);
        var vertex = (tessellator_vertex_sig)(ba + OFFSETS.FUNC.TESSELLATOR_VERTEX);
        var colour = (tessellator_colour_sig)(ba + OFFSETS.FUNC.TESSELLATOR_COLOUR);
        var end = (tessellator_end_sig)(ba + OFFSETS.FUNC.TESSELLATOR_END);
        var draw = (tessellator_draw_sig)(ba + OFFSETS.FUNC.TESSELLATOR_DRAW);

        nint ctx = ba + OFFSETS.FUNC.RENDER_CTX;

        float* shader_color = (float*)(ba + OFFSETS.FUNC.SHADER_COLOR);
        byte* shader_flag = (byte*)(ba + OFFSETS.FUNC.SHADER_COLOR_SET);

        byte* tess = (byte*)tessellator;
        if (tess[0x170] != 0 || tess[0x125] != 0 || *(int*)(tess + 0x168) != 0)
        {
            byte* flush_buf = stackalloc byte[0x200];
            for (int i = 0; i < 0x200; i++) flush_buf[i] = 0;
            end(tessellator, (nint)flush_buf, 0, 0);
        }

        byte flag_saved = *shader_flag;

        *shader_flag = 1;
        shader_color[0] = 1f;
        shader_color[1] = 0f;
        shader_color[2] = 1f;
        shader_color[3] = 1f;

        foreach (nint entity in entity_cache.all())
        {
            if (entity == 0) continue;
            if (entity == player) continue;

            if (!memory.is_readable(entity + OFFSETS.FIELD.ENTITY_POS_OLD_X, 24)) continue;
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

            float ex = px - cx;
            float ey = py - cy - 1.62f;
            float ez = pz - cz;

            float x0 = ex - half_w, x1 = ex + half_w;
            float y0 = ey, y1 = ey + height;
            float z0 = ez - half_w, z1 = ez + half_w;

            float* corners_x = stackalloc float[8] { x0, x1, x1, x0, x0, x1, x1, x0 };
            float* corners_y = stackalloc float[8] { y0, y0, y0, y0, y1, y1, y1, y1 };
            float* corners_z = stackalloc float[8] { z0, z0, z1, z1, z0, z0, z1, z1 };

            begin(tessellator, 4, edge_count * 2);
            colour(tessellator, 255, 0, 255, 255);

            for (int e = 0; e < edge_count; e++)
            {
                int a = edges_a[e];
                int b = edges_b[e];
                vertex(tessellator, corners_x[a], corners_y[a], corners_z[a]);
                vertex(tessellator, corners_x[b], corners_y[b], corners_z[b]);
            }
            draw(tessellator, ctx, 0);
        }

        *shader_flag = flag_saved;

        byte* final_buf = stackalloc byte[0x200];
        for (int i = 0; i < 0x200; i++) final_buf[i] = 0;
        end(tessellator, (nint)final_buf, 0, 0);

        entity_cache.clear();
    }
}