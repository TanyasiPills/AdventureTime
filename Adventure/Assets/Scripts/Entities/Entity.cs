using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public float hp;
    private int _level = 10;
    [SerializeReference]
    public BaseStats baseStats;
    private Stats stats;

    public Action<int> OnLevelChange = delegate { };

    public Stats Stats { get => stats; }
    public int Level { get => _level; private set {
            _level = value;
            OnLevelChange.Invoke(_level);
        }
    }

    public void Heal(float hp)
    {
        this.hp = Mathf.Min(this.hp + hp, Stats.Hp);
    }

    public void HealPercent(float percent)
    {
        float maxhp = Stats.Hp;
        this.hp = Mathf.Min(this.hp + maxhp * percent, maxhp);
    }

    void Start()
    {
        stats = new Stats(baseStats, Level, OnLevelChange);
        Debug.Log(stats.Hp);
        stats.Mediator.AddModifier(new BasicModifier(StatType.Hp, OperatorType.Multiply, 2.3f, Source.Equipment));
        stats.Mediator.AddModifier(new BasicModifier(StatType.Hp, OperatorType.Add, 15f, Source.Equipment));
        stats.Mediator.AddModifier(new BasicModifier(StatType.Hp, OperatorType.Subtract, 5f, Source.Equipment));
        Debug.Log(stats.Hp);

        this.hp = stats.Hp;
    }

    [ContextMenu("Log Stats")]
    public void LogStats()
    {
        Debug.Log(
            $"{gameObject.name}\n" +
            $"hp: {Stats.Hp}\n" +
            $"armor: {Stats.Armor}\n" +
            $"attack damage: {Stats.AttackDamage}\n" +
            $"armor pen: {Stats.ArmorPen}"
        );
    }
}
    