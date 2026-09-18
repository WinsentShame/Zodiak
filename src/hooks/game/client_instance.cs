using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class client_instance
{
    private static delegate* unmanaged[Stdcall]<nint, int, int, void> original;

    public static nint Pointer { get; private set; }

    public static bool install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) { return false; }

        nint address = base_address + OFFSETS.FUNC.CLIENTINSTANCE_ONTICK;
        nint detour_ptr = (nint)(delegate* unmanaged[Stdcall]<nint, int, int, void>)&hook;

        int st = native_interop.mh_create_hook(address, detour_ptr, out nint Orig);
        if (st != 0) { return false; }

        original = (delegate* unmanaged[Stdcall]<nint, int, int, void>)Orig;

        st = native_interop.mh_enable_hook(address);
        if (st != 0) {  return false; }

        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void hook(nint self, int a2, int a3)
    {
        if (Pointer == 0)
        {
            Pointer = self;
        }

        if (original != null)
            original(self, a2, a3);
    }
}