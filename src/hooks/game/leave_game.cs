using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using unsafe leave_game_sig = delegate* unmanaged[Stdcall]<nint, nint, long>;

namespace Zodiak;

public static unsafe class leave_game
{
    private static leave_game_sig ORIGINAL;
    private static nint target;

    public static bool IS_INSTALLED { get; private set; }

    public static bool install()
    {
        if (IS_INSTALLED) return true;

        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return false;

        target = base_address + OFFSETS.FUNC.CLIENTINSTANCE_LEAVEGAME;

        leave_game_sig detour_fn = &detour;
        nint detour_ptr = (nint)detour_fn;

        if (!hook.install(target, detour_ptr, out nint original_ptr))
            return false;

        ORIGINAL = (leave_game_sig)original_ptr;
        IS_INSTALLED = true;
        return true;
    }

    //public static void uninstall()
    //{
    //    if (!IS_INSTALLED) return;

    //    native_interop.mh_disable_hook(target);
    //    native_interop.mh_remove_hook(target);

    //    target = 0;
    //    ORIGINAL = null;
    //    IS_INSTALLED = false;
    //}

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static long detour(nint self, nint flag)
    {
        if (ORIGINAL == null) return 0;
        return ORIGINAL(self, flag);
    }
}