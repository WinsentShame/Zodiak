namespace Zodiak;

public abstract class module
{
    public string NAME { get; }
    public category CATEGORY { get; }
    public bool BINDABLE { get; }

    private readonly string _description;
    public virtual string DESCRIPTION => _description;

    private readonly string _usage;
    public virtual string USAGE => _usage;

    public keybind BIND { get; set; } = new();

    private bool _enabled;
    public bool ENABLED
    {
        get => _enabled;
        set
        {
            if (_enabled == value) return;
            _enabled = value;

            if (value) on_enable();
            else on_disable();
        }
    }

    protected module(category category, string name, string usage, string description, bool bindable)
    {
        CATEGORY = category;
        NAME = name;
        _usage = usage;
        _description = description;
        BINDABLE = bindable;
    }

    public virtual void on_enable() { }
    public virtual void on_disable() { }
    public virtual void on_update() { }

    public virtual void on_command(string[] args)
    {
        ENABLED = !ENABLED;
    }
}