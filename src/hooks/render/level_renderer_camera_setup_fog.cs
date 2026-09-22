using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class level_renderer_camera_setup_fog : hook_group
{
    private static setup_fog_sig original;

    public static bool ACTIVE;

    public static float R = 1f, G = 1f, B = 1f;

    protected override string NAME => "level_renderer_camera_setup_fog";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.LEVELRENDERERCAMERA_SETUPFOG;
    protected override void store_original(nint ptr) => original = (setup_fog_sig)ptr;

    protected override nint detour_ptr()
    {
        setup_fog_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static long detour(nint self, nint a2, float a3, nint a4, nint a5)
    {
        long result = original(self, a2, a3, a4, a5);

        if (ACTIVE)
        {
            nint slot = self + OFFSETS.FIELD.LEVELRENDERERCAMERA_FOGCOLOUR;
            if (memory.is_readable(slot, 12))
            {
                float* p = (float*)slot;
                p[0] = R;
                p[1] = G;
                p[2] = B;
            }
        }

        return result;
    }
}