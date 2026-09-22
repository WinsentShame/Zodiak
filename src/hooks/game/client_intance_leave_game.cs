using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Zodiak;

public sealed unsafe class client_intance_leave_game : hook_group
{
    private static leave_game_sig original;

    protected override string NAME => "client_instance_leave_game";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.CLIENTINSTANCE_LEAVEGAME;
    protected override void store_original(nint ptr) => original = (leave_game_sig)ptr;

    protected override nint detour_ptr()
    {
        leave_game_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static long detour(nint self, nint flag)
    {
        context.NETWORK_PEER = 0;
        context.CLIENT_INSTANCE = 0;
        context.MINECRAFT_GAME = 0;
        return original(self, flag);
    }
}