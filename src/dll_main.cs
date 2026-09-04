using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Zodiak;

public unsafe class dll_main
{
    [UnmanagedCallersOnly(EntryPoint = "DllMain", CallConvs = new[] { typeof(CallConvStdcall) })]
    public static bool DllMain(IntPtr hModule, uint reason, IntPtr reserved)
    {
        if (reason == 1)
        {
            logger.init();
            try
            {
                int initResult = native.MH_Initialize();
                if (initResult != 0 && initResult != 1)
                    logger.error("main", $"MH_Initialize failed: {initResult}");

                draw.set_window(native.FindWindow(null, "Minecraft"));
                leave_game.install();
            }
            catch (Exception ex)
            {
                logger.error("main", ex.ToString());
            }
        }
        return true;
    }
}