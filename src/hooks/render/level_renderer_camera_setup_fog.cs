using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using unsafe setup_fog_sig = delegate* unmanaged[Stdcall]<nint, nint, float, nint, nint, long>;

namespace Zodiak;

public static unsafe class level_renderer_camera_setup_fog
{
    private const int fog_colour_size = 12;

    private static setup_fog_sig ORIGINAL;

    public static bool ACTIVE;
    public static float R = 1f, G = 1f, B = 1f;

    public static bool install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return false;

        nint address = base_address + OFFSETS.FUNC.LEVELRENDERERCAMERA_SETUPFOG;

        setup_fog_sig detour_fn = &detour;
        nint detour_ptr = (nint)detour_fn;

        if (!hook.install(address, detour_ptr, out nint original_ptr))
            return false;

        ORIGINAL = (setup_fog_sig)original_ptr;
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static long detour(nint self, nint a2, float a3, nint a4, nint a5)
    {
        long result = ORIGINAL(self, a2, a3, a4, a5);

        if (ACTIVE)
        {
            nint slot = self + OFFSETS.FIELD.LEVELRENDERERCAMERA_FOGCOLOUR;
            if (memory.is_readable(slot, fog_colour_size))
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