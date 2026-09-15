global using static korn;

namespace Zodiak;

public unsafe class dll_main
{
    [EntryPoint]
    public static bool DllMain(nint hinstDLL, uint fdwReason, nint lpvReserved)
    {
        if (fdwReason == 1)
        {
            
        }
        return true;
    }
}
