using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Zodiak;

public static unsafe class chat_hook
{
    private const int MAX_MESSAGE_SIZE = 4096;
    private const int SSO_THRESHOLD = 16;
    private const int STRING_SIZE_OFFSET = 16;

    private static delegate* unmanaged[Stdcall]<nint, nint, void> original;

    public static bool install()
    {
        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0) {  return false; }

        nint address = base_address + OFFSETS.FUNC.MINECRAFTSCREENMODEL_SENDCHATMESSAGE;
        nint detour_ptr = (nint)(delegate* unmanaged[Stdcall]<nint, nint, void>)&hook;

        int st = native_interop.mh_create_hook(address, detour_ptr, out nint orig);
        if (st != 0) {  return false; }

        original = (delegate* unmanaged[Stdcall]<nint, nint, void>)orig;

        st = native_interop.mh_enable_hook(address);
        if (st != 0) { return false; }

        return true;
    }

    [UnmanagedCallersOnly(CallConvs = new[] { typeof(CallConvStdcall) })]
    private static void hook(nint self, nint message_ptr)
    {
        string? msg = read_string(message_ptr);

        if (msg != null && msg.Length > 0 && msg[0] == '.')
        {
            chat_commands.dispatch(msg);
            return;
        }

        if (original != null)
            original(self, message_ptr);
    }

    private static string? read_string(nint ptr)
    {
        if (ptr == 0) return null;
        if (!memory.is_readable(ptr, 32)) return null;

        long size = *(long*)(ptr + STRING_SIZE_OFFSET);
        if (size <= 0 || size > MAX_MESSAGE_SIZE) return null;

        nint data = size < SSO_THRESHOLD ? ptr : *(nint*)ptr;
        if (!memory.is_readable(data, (nuint)size)) return null;

        byte[] bytes = new byte[size];
        for (int I = 0; I < size; I++) bytes[I] = *(byte*)(data + I);

        return Encoding.UTF8.GetString(bytes);
    }
}