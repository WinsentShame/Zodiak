namespace Zodiak;

public abstract class module
{
    public string name { get; }
    public string description { get; }
    public category category { get; }

    private bool _enabled;
    public bool enabled
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
        this.category = category;
        this.name = name;
        this.description = description;
    }

    public virtual void on_enable() { }
    public virtual void on_disable() { }
    public virtual void on_update() { }

    public virtual void on_command(string[] args)
    {
        enabled = !enabled;
    }
}