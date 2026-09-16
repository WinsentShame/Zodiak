using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class level_renderer_camera__setup_fog
{
    private const int FOG_COLOUR_SIZE = 12;

    private static delegate* unmanaged[Stdcall]<nint, nint, float, nint, nint, long> original;

    public static bool active;
    public static float r = 1f, g = 1f, b = 1f;

    public static bool install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) { return false; }

        nint address = base_address + OFFSETS.FUNC.LEVELRENDERERCAMERA_SETUPFOG;
        nint detour_ptr = (nint)(delegate* unmanaged[Stdcall]<nint, nint, float, nint, nint, long>)&hook;

        int st = native_interop.mh_create_hook(address, detour_ptr, out nint orig);
        if (st != 0) { return false; }

        original = (delegate* unmanaged[Stdcall]<nint, nint, float, nint, nint, long>)orig;

        st = native_interop.mh_enable_hook(address);
        if (st != 0) { return false; }

        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static long hook(nint self, nint A2, float A3, nint A4, nint A5)
    {
        long result = original(self, A2, A3, A4, A5);

        if (active)
        {
            nint slot = self + OFFSETS.FIELD.LEVELRENDERERCAMERA_FOGCOLOUR;
            if (memory.is_readable(slot, FOG_COLOUR_SIZE))
            {
                float* P = (float*)slot;
                P[0] = r;
                P[1] = g;
                P[2] = b;
            }
        }

        return result;
    }
}