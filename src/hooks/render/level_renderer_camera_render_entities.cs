using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class level_renderer_camera_render_entities : hook_group
{
    public static bool ACTIVE;

    private static bool in_entity_pass;
    private static bool creating_no_depth;
    private static bool no_depth_ready;

    private static nint no_depth_state;
    private static nint cached_ctx;

    private static render_entities_sig original_render_entities;
    private static create_depth_state_sig original_create_depth;
    private static apply_depth_state_sig original_apply_depth;

    protected override string NAME => "level_renderer_camera_render_entities";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.LEVELRENDERERCAMERA_RENDERENTITIES;
    protected override void store_original(nint ptr) => original_render_entities = (render_entities_sig)ptr;

    protected override nint detour_ptr()
    {
        render_entities_sig fn = &render_entities_detour;
        return (nint)fn;
    }

    public override bool install()
    {
        if (!base.install()) return false;

        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0)
        {
            uninstall();
            return false;
        }

        {
            nint addr = base_address + OFFSETS.FUNC.MCE_RENDERCONTEXT_CREATEDEPTHSTATE;
            create_depth_state_sig fn = &create_depth_detour;
            if (!hook.install(addr, (nint)fn, out nint orig) || orig == 0)
            {
                uninstall();
                return false;
            }
            original_create_depth = (create_depth_state_sig)orig;
        }
        {
            nint addr = base_address + OFFSETS.FUNC.MCE_RENDERCONTEXT_APPLYDEPTHSTATE;
            apply_depth_state_sig fn = &apply_depth_detour;
            if (!hook.install(addr, (nint)fn, out nint orig) || orig == 0)
            {
                uninstall();
                return false;
            }
            original_apply_depth = (apply_depth_state_sig)orig;
        }

        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint render_entities_detour(nint self, float partial)
    {
        if (original_render_entities == null) return 0;

        bool prev = in_entity_pass;
        if (ACTIVE) in_entity_pass = true;
        try { return original_render_entities(self, partial); }
        finally { in_entity_pass = prev; }
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint create_depth_detour(nint state, nint ctx, nint packet)
    {
        if (original_create_depth == null) return 0;
        return original_create_depth(state, ctx, packet);
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint apply_depth_detour(nint state, nint ctx, nint force)
    {
        if (original_apply_depth == null) return 0;

        if (!ACTIVE || !in_entity_pass || state == 0 || creating_no_depth)
            return original_apply_depth(state, ctx, force);

        if (ctx != 0 && cached_ctx == 0)
            cached_ctx = ctx;

        if (!no_depth_ready)
            ensure_no_depth_state();

        if (!no_depth_ready)
            return original_apply_depth(state, ctx, force);

        int saved_depth = *(int*)state;
        nint saved_obj = *(nint*)(state + 24);

        *(int*)state = 0;
        *(nint*)(state + 24) = no_depth_state;

        try
        {
            return original_apply_depth(state, ctx, force);
        }
        finally
        {
            *(int*)state = saved_depth;
            *(nint*)(state + 24) = saved_obj;
        }
    }

    private static unsafe void ensure_no_depth_state()
    {
        if (no_depth_ready) return;
        if (cached_ctx == 0) return;

        creating_no_depth = true;
        try
        {
            byte* saved_cache = stackalloc byte[24];
            for (int i = 0; i < 24; i++) saved_cache[i] = *(byte*)(cached_ctx + 8 + i);
            byte saved_flag = *(byte*)(cached_ctx + 117);

            *(byte*)(cached_ctx + 117) = 1;

            byte* packet = stackalloc byte[20];
            for (int i = 0; i < 20; i++) packet[i] = 0;
            packet[0] = 1;
            packet[1] = 0;
            packet[2] = 2;
            packet[3] = 2;
            packet[4] = 1; packet[5] = 1; packet[6] = 1; packet[7] = 1;
            packet[8] = 1; packet[9] = 1; packet[10] = 1;
            packet[11] = 1;
            packet[12] = 0xFF;
            packet[16] = 0xFF;

            byte* state_buf = stackalloc byte[32];
            for (int i = 0; i < 32; i++) state_buf[i] = 0;

            original_create_depth((nint)state_buf, cached_ctx, (nint)packet);

            nint obj = *(nint*)(state_buf + 24);
            if (obj != 0)
            {
                no_depth_state = obj;
                no_depth_ready = true;
            }

            for (int i = 0; i < 24; i++) *(byte*)(cached_ctx + 8 + i) = saved_cache[i];
            *(byte*)(cached_ctx + 117) = saved_flag;
        }
        finally
        {
            creating_no_depth = false;
        }
    }
}