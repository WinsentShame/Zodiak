namespace Zodiak;

public abstract class module
{
    public string NAME { get; }
    public virtual string DESCRIPTION => raw_description ?? "";
    public keybind BIND { get; set; } = new();
    public category CATEGORY { get; }

    private readonly string? raw_description;
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

    protected module(category category, string name, string description)
    {
        CATEGORY = category;
        NAME = name;
        raw_description = description;
    }

    public virtual void on_enable() { }
    public virtual void on_disable() { }
    public virtual void on_update() { }

    public virtual void on_command(string[] args)
    {
        ENABLED = !ENABLED;
    }
}