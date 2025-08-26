using System;
using UnityEngine;

public enum StatType { Hp, Armor, AttackDamage, ArmorPen }

public class Stats
{
    public int level;
    public BaseStats baseStats;
    private readonly StatsMediator mediator;

    public Stats(BaseStats baseStats, int level, Action<int> OnLevelChange)
    {
        this.level = level;
        this.baseStats = baseStats;
        mediator = new StatsMediator();
        OnLevelChange += newLevel => this.level = newLevel;
    }

    public StatsMediator Mediator { get => mediator; }

    public int Hp
    {
        get => mediator.PerformQuery(StatType.Hp, baseStats.Hp(level));
    }

    public int Armor
    {
        get;
    }

    public int AttackDamage
    {
        get;
    }

    public int ArmorPen
    {
        get;
    }
}
