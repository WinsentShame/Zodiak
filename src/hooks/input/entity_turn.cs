using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class entity_turn : hook_group
{
    private static entity_turn_sig original;

    protected override string NAME => "entity_turn";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.ENTITY_TURN;
    protected override void store_original(nint ptr) => original = (entity_turn_sig)ptr;

    protected override nint detour_ptr()
    {
        entity_turn_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, nint delta, byte fast)
    {
        if (original == null) return;

        if (!freelook.ACTIVE || self != freelook.LOCAL_PLAYER || !entity_check.is_valid(self))
        {
            original(self, delta, fast);
            return;
        }

        entity_check.write_rotation(self, freelook.CAM_YAW, freelook.CAM_PITCH);

        original(self, delta, fast);

        freelook.CAM_YAW = entity_check.get_yaw(self);
        freelook.CAM_PITCH = entity_check.get_pitch(self);

        entity_check.write_rotation(self, freelook.FROZEN_YAW, freelook.FROZEN_PITCH);
    }
}