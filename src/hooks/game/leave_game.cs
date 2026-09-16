using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class leave_game
{
    private static delegate* unmanaged[Stdcall]<nint, nint, long> original;
    private static nint target;

    public static bool is_installed { get; private set; }

    public static bool install()
    {
        if (is_installed) return true;

        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) { return false; }

        target = base_address + OFFSETS.FUNC.CLIENTINSTANCE_LEAVEGAME;

        nint detour_ptr = (nint)(delegate* unmanaged[Stdcall]<nint, nint, long>)&detour;
        int st = native_interop.mh_create_hook(target, detour_ptr, out nint OriginalPtr);
        if (st != 0) { return false; }

        original = (delegate* unmanaged[Stdcall]<nint, nint, long>)OriginalPtr;

        st = native_interop.mh_enable_hook(target);
        if (st != 0)
        {
            native_interop.mh_remove_hook(target);
            original = null;
            return false;
        }

        is_installed = true;
        return true;
    }

    //public static void uninstall()
    //{
    //    if (!is_installed) return;
    //    native_interop.mh_disable_hook(_Target);
    //    native_interop.mh_remove_hook(_Target);
    //    _Target = 0;
    //    original = null;
    //    is_installed = false;
    //}

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static long detour(nint self, nint flag)
    {
        if (original == null) return 0;
        return original(self, flag);
    }
}