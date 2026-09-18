using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public static unsafe class minecraft_game
{
    private static delegate* unmanaged[Stdcall]<nint, nint, void> original;

    public static nint pointer { get; private set; }

    public static bool install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) {  return false; }

        nint address = base_address + OFFSETS.FUNC.MINECRAFTGAME_UPDATEGRAPHICS;
        nint detour_ptr = (nint)(delegate* unmanaged[Stdcall]<nint, nint, void>)&detour;

        int st = native_interop.mh_create_hook(address, detour_ptr, out nint orig);
        if (st != 0) {  return false; }

        original = (delegate* unmanaged[Stdcall]<nint, nint, void>)orig;

        st = native_interop.mh_enable_hook(address);
        if (st != 0) { return false; }

        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, nint a2)
    {
        original(self, a2);

        if (pointer == 0)
        {
            pointer = self;
        }
    }
}