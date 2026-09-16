using System.Runtime.InteropServices;
using System.Text;

namespace Zodiak;

// Гавно собачье
public static unsafe class msvc_string
{
    public const int SIZE = 0x20;

    private const int SSO_THRESHOLD = 16;
    private const int SSO_CAPACITY = 15;
    private const int OFFSET_LENGTH = 16;
    private const int OFFSET_CAPACITY = 24;

    public static void write(nint dest, string s)
    {
        byte[] raw = Encoding.UTF8.GetBytes(s);
        int len = raw.Length;

        for (int i = 0; i < SIZE; i++) *(byte*)(dest + i) = 0;

        if (len < SSO_THRESHOLD)
        {
            for (int i = 0; i < len; i++) *(byte*)(dest + i) = raw[i];
            *(byte*)(dest + len) = 0;
            *(long*)(dest + OFFSET_LENGTH) = len;
            *(long*)(dest + OFFSET_CAPACITY) = SSO_CAPACITY;
        }
        else
        {
            nint heap = Marshal.AllocHGlobal(len + 1);
            for (int i = 0; i < len; i++) *(byte*)(heap + i) = raw[i];
            *(byte*)(heap + len) = 0;
            *(nint*)dest = heap;
            *(long*)(dest + OFFSET_LENGTH) = len;
            *(long*)(dest + OFFSET_CAPACITY) = len;
        }
    }

    public static void free(nint dest)
    {
        if (dest == 0) return;

        long capacity = *(long*)(dest + OFFSET_CAPACITY);
        if (capacity < SSO_THRESHOLD) return;

        nint heap = *(nint*)dest;
        if (heap != 0) Marshal.FreeHGlobal(heap);
    }
}