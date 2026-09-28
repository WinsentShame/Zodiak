using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class level_renderer_player_move_camera_to_player : hook_group
{
    private static move_camera_to_player_sig original;

    protected override string NAME => "level_renderer_player_move_camera_to_player";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.LEVEL_RENDERER_PLAYER_MOVE_CAMERA_TO_PLAYER;
    protected override void store_original(nint ptr) => original = (move_camera_to_player_sig)ptr;

    protected override nint detour_ptr()
    {
        move_camera_to_player_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static nint detour(nint self, nint matrix, float partial)
    {
        if (original == null) return 0;

        nint player = freelook.LOCAL_PLAYER;
        if (!freelook.ACTIVE || !entity_check.is_valid(player))
            return original(self, matrix, partial);

        float saved_yaw = entity_check.get_yaw(player);
        float saved_pitch = entity_check.get_pitch(player);

        float saved_old_yaw = entity_check.get_old_yaw(player);
        float saved_old_pitch = entity_check.get_old_pitch(player);

        entity_check.write_rotation(player, freelook.CAM_YAW, freelook.CAM_PITCH);

        nint result = original(self, matrix, partial);

        entity_check.write_rotation(player, saved_yaw, saved_pitch);
        *(float*)(player + OFFSETS.FIELD.ENTITY_ROT_OLD_YAW) = saved_old_yaw;
        *(float*)(player + OFFSETS.FIELD.ENTITY_ROT_OLD_PITCH) = saved_old_pitch;

        return result;
    }
}