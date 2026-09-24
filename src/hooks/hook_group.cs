namespace Zodiak;

public static class hook
{
    public static bool install(nint address, nint detour, out nint original)
    {
        original = 0;

        int r = native_interop.mh_create_hook(address, detour, out original);

        if (r == 3) 
        {
            native_interop.mh_disable_hook(address);
            native_interop.mh_remove_hook(address);

            r = native_interop.mh_create_hook(address, detour, out original);
        }

        if (r != 0)
        {
            original = 0;
            return false;
        }

        if (original == 0)
        {
            native_interop.mh_remove_hook(address);
            return false;
        }

        if (native_interop.mh_enable_hook(address) != 0)
        {
            native_interop.mh_remove_hook(address);
            original = 0;
            return false;
        }

        return true;
    }
}
public abstract unsafe class hook_group
{
    public bool INSTALLED { get; private set; }
    protected nint target;

    protected abstract string NAME { get; }
    protected abstract nint TARGET_OFFSET { get; }
    protected abstract nint detour_ptr();
    protected abstract void store_original(nint ptr);

    protected virtual void on_installed() { }
    protected virtual void on_uninstalled() { }

    public virtual bool install()
    {
        if (INSTALLED) return true;

        nint base_address = native_interop.get_module_handle_w(null);
        if (base_address == 0)
            return false;
        

        target = base_address + TARGET_OFFSET;

        if (!hook.install(target, detour_ptr(), out nint original_ptr))
            return false;
        

        if (original_ptr == 0)
            return false;
        

        store_original(original_ptr);
        INSTALLED = true;

        on_installed();
        return true;
    }

    public void uninstall()
    {
        if (!INSTALLED) return;

        native_interop.mh_disable_hook(target);
        native_interop.mh_remove_hook(target);
        INSTALLED = false;

        on_uninstalled();
    }

    public void reinstall()
    {
        if (INSTALLED) uninstall();
        install();
    }
}