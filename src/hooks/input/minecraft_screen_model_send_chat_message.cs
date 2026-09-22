using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public sealed unsafe class minecraft_screen_model_send_chat_message : hook_group
{
    private static send_chat_sig original;

    protected override string NAME => "minecraft_screen_model_send_chat_message";
    protected override nint TARGET_OFFSET => OFFSETS.FUNC.MINECRAFTSCREENMODEL_SENDCHATMESSAGE;
    protected override void store_original(nint ptr) => original = (send_chat_sig)ptr;

    protected override nint detour_ptr()
    {
        send_chat_sig fn = &detour;
        return (nint)fn;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, nint message_ptr)
    {
        string? msg = msvc_string.read(message_ptr);

        if (msg != null && msg.Length > 0 && msg[0] == '.')
        {
            chat_commands.dispatch(msg);
            return;
        }

        original(self, message_ptr);
    }
}