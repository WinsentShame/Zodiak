using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class local_player_normal_tick : hook_group
{
    private static entity_tick_sig original;

    protected override string NAME => "local_player_normal_tick";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.LOCALPLAYER_NORMALTICK;
    protected override void store_original(nint ptr) => original = (entity_tick_sig)ptr;

    protected override nint detour_ptr()
    {
        entity_tick_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self)
    {
        if (self != 0)
            entity_type.register_player_vtable(*(nint*)self);

        original(self);
    }
}