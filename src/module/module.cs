namespace Zodiak;

public abstract class module
{
    public string NAME { get; }
    public string DESCRIPTION { get; }
    public category CATEGORY { get; }

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
        this.CATEGORY = category;
        this.NAME = name;
        this.DESCRIPTION = description;
    }

    public virtual void on_enable() { }
    public virtual void on_disable() { }
    public virtual void on_update() { }

    public virtual void on_command(string[] args)
    {
        ENABLED = !ENABLED;
    }
}