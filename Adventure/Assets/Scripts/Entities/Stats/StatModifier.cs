using System;
using System.Threading;

public abstract class StatModifier : IDisposable
{
    public bool markedForRemoval { get; private set; }
    public event Action<StatModifier> OnDispose = delegate { };
    private int duration;

    protected StatModifier(int duration)
    {
        if (duration <= 0) this.duration = -999;
        this.duration = duration;
    }
    protected StatModifier()
    {
        duration = -999;
    }

    public void Update(int turn)
    {
        if (duration == -999) return;
        duration -= turn;
        if (duration == -1) Dispose(); 

    }
    public abstract int Handle(StatType type, int value);
    public void Dispose()
    {
        OnDispose.Invoke(this);
    }
}
