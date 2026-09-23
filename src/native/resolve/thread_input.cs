using System.Runtime.InteropServices;

namespace Zodiak;

public static class thread_input
{
    private const uint TH32CS_SNAPTHREAD = 0x00000004;

    private static bool attached;

    public static void attach()
    {
        if (attached) return;
        attached = true;

        uint self_pid = (uint)Environment.ProcessId;
        uint self = native_interop.get_current_thread_id();

        nint snapshot = native_interop.create_toolhelp32_snapshot(TH32CS_SNAPTHREAD, 0);
        if (snapshot == 0) return;

        var entry = new native_interop.THREADENTRY32();
        entry.dwSize = (uint)Marshal.SizeOf<native_interop.THREADENTRY32>();

        int ok = 0;
        if (native_interop.thread32_first(snapshot, ref entry))
        {
            do
            {
                if (entry.th32OwnerProcessID != self_pid) continue;
                if (entry.th32ThreadID == self) continue;

                if (native_interop.attach_thread_input(self, entry.th32ThreadID, true))
                    ok++;
            }
            while (native_interop.thread32_next(snapshot, ref entry));
        }
        native_interop.close_handle(snapshot);
    }

    public static bool is_down(int vk)
        => (native_interop.get_async_key_state(vk) & 0x8000) != 0;

    private static readonly bool[] prev = new bool[256];

    public static bool was_pressed(int vk)
    {
        if (vk <= 0 || vk >= 256) return false;
        bool now = (native_interop.get_async_key_state(vk) & 0x8000) != 0;
        bool pressed = now && !prev[vk];
        prev[vk] = now;
        return pressed;
    }
}