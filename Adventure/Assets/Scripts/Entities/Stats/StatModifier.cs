using System;
using System.Threading;

public enum Source { Equipment, Consumable }
[Serializable]
public abstract class StatModifier
{
    public bool markedForRemoval { get; private set; }
    public event Action<StatModifier> OnRemove = delegate { };
    public int duration;
    public int currentTime;
    public int priority = 0;
    public Source source;

    protected StatModifier()
    {
        this.duration = 0;
        this.priority = 0;
    }
    protected StatModifier(Source source, int duration = 0)
    {
        if (duration < 0) this.duration = 0;
        else
        {
            this.duration = duration;
            this.currentTime = duration;
        }
        this.source = source;
    }

    public virtual void applied() 
    {
        this.currentTime = duration;
        this.markedForRemoval = false;
    }

    public void Update(int turn)
    {
        if (duration == 0) return;
        currentTime -= turn;
        if (currentTime == -1) Remove();
    }
    public abstract float Handle(StatType type, float value);
    public void Remove()
    {
        OnRemove.Invoke(this);
    }
}
