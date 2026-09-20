using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

using unsafe send_chat_sig = delegate* unmanaged[Stdcall]<nint, nint, void>;

namespace Zodiak;

public static unsafe class chat_hook
{
    private const int max_message_size = 4096;
    private const int sso_threshold = 16;
    private const int string_size_offset = 16;

    private static send_chat_sig ORIGINAL;

    public static bool install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) return false;

        nint address = base_address + OFFSETS.FUNC.MINECRAFTSCREENMODEL_SENDCHATMESSAGE;

        send_chat_sig detour_fn = &detour;
        nint detour_ptr = (nint)detour_fn;

        if (!hook.install(address, detour_ptr, out nint original_ptr))
            return false;

        ORIGINAL = (send_chat_sig)original_ptr;
        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void detour(nint self, nint message_ptr)
    {
        string? msg = read_string(message_ptr);

        if (msg != null && msg.Length > 0 && msg[0] == '.')
        {
            chat_commands.dispatch(msg);
            return;
        }

        if (ORIGINAL != null)
            ORIGINAL(self, message_ptr);
    }

    private static string? read_string(nint ptr)
    {
        if (ptr == 0) return null;
        if (!memory.is_readable(ptr, 32)) return null;

        long size = *(long*)(ptr + string_size_offset);
        if (size <= 0 || size > max_message_size) return null;

        nint data = size < sso_threshold ? ptr : *(nint*)ptr;
        if (!memory.is_readable(data, (nuint)size)) return null;

        byte[] bytes = new byte[size];
        for (int i = 0; i < size; i++) bytes[i] = *(byte*)(data + i);

        return Encoding.UTF8.GetString(bytes);
    }
}