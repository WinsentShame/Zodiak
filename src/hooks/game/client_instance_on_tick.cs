using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class client_instance_on_tick : hook_group
{
    private static on_tick_sig original;

    protected override string NAME => "client_instance_on_tick";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.CLIENTINSTANCE_ONTICK;
    protected override void store_original(nint ptr) => original = (on_tick_sig)ptr;

    protected override nint detour_ptr()
    {
        on_tick_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, int a2, int a3)
    {
        if (self != 0 && context.CLIENT_INSTANCE != self)
            context.CLIENT_INSTANCE = self;

        original(self, a2, a3);

        keybind_manager.tick();
    }
}