using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class options_set_player_view_perspective : hook_group
{
    private static set_perspective_sig original;

    protected override string NAME => "options_set_player_view_perspective";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.OPTIONS_SETPLAYERVIEWPERSPECTIVE;
    protected override void store_original(nint ptr) => original = (set_perspective_sig)ptr;

    protected override nint detour_ptr()
    {
        set_perspective_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, int value)
    {
        if (original == null) return;

        if (freelook.ACTIVE && !game_options.ALLOW_INTERNAL)
            return;

        original(self, value);
    }
}