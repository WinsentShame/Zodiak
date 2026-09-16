using System.Runtime.InteropServices;

namespace Zodiak;

public static class Program
{
    [UnmanagedCallersOnly(EntryPoint = "DllProcessAttach")]
    public static void OnDllProcessAttach(nint hModule)
    {
        logger.init();
        logger.info("init", "Zodiak loading");

        int st = native_interop.MH_Initialize();
        if (st != 0) { logger.error("init", $"MH_Initialize: {st}"); return; }

        draw.Install();       
        overlay.Install();    
        leave_game.Install();
        gui.Install();

        //module_manager.register(new fly());

        logger.info("init", $"Zodiak ready ({module_manager.count} modules)");
    }
}