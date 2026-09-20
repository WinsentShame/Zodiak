namespace Zodiak;

public static class hook
{
    public static bool install(nint address, nint detour, out nint original)
    {
        original = 0;

        if (native_interop.mh_create_hook(address, detour, out original) != 0)
            return false;

        if (native_interop.mh_enable_hook(address) != 0)
        {
            native_interop.mh_remove_hook(address);
            original = 0;
            return false;
        }

        return true;
    }
}