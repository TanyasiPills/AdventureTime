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

    public float Hp
    {
        get => mediator.PerformQuery(StatType.Hp, baseStats.Hp(level));
    }

    public float Armor
    {
        get => mediator.PerformQuery(StatType.Armor, baseStats.Armor(level));
    }

    public float AttackDamage
    {
        get => mediator.PerformQuery(StatType.AttackDamage, baseStats.AttackDamge(level));
    }

    public float ArmorPen
    {
        get => mediator.PerformQuery(StatType.ArmorPen, baseStats.ArmorPen(level));
    }
}
