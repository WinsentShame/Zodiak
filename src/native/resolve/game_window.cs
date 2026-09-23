using System.Runtime.InteropServices;

namespace Zodiak;

public static class game_window
{
    private const uint TH32CS_SNAPTHREAD = 0x00000004;

    private static uint cached_thread;

    public static uint thread_id()
    {
        if (cached_thread != 0) return cached_thread;

        uint self_pid = (uint)Environment.ProcessId;
        uint self_tid = native_interop.get_current_thread_id();

        nint snapshot = native_interop.create_toolhelp32_snapshot(TH32CS_SNAPTHREAD, 0);
        if (snapshot == 0) return 0;

        var entry = new native_interop.THREADENTRY32();
        entry.dwSize = (uint)Marshal.SizeOf<native_interop.THREADENTRY32>();

        if (native_interop.thread32_first(snapshot, ref entry))
        {
            do
            {
                if (entry.th32OwnerProcessID != self_pid) continue;
                if (entry.th32ThreadID == self_tid) continue;

                if (native_interop.attach_thread_input(self_tid, entry.th32ThreadID, true))
                {
                    native_interop.attach_thread_input(self_tid, entry.th32ThreadID, false);
                    cached_thread = entry.th32ThreadID;
                    break;
                }
            }
            while (native_interop.thread32_next(snapshot, ref entry));
        }
        native_interop.close_handle(snapshot);

        return cached_thread;
    }
}