using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private int _level = 10;
    public BaseStats baseStats;
    private Stats stats;

    public Action<int> OnLevelChange = delegate { };

    public Stats Stats { get => stats; }
    public int Level { get => _level; private set {
            _level = value;
            OnLevelChange.Invoke(_level);
        }
    }

    void Start()
    {
        stats = new Stats(baseStats, Level, OnLevelChange);
        Debug.Log(stats.Hp);
        stats.Mediator.AddModifier(new BasicModifier(StatType.Hp, BasicModifier.OperatorType.Multiply, 2));
        stats.Mediator.AddModifier(new BasicModifier(StatType.Hp, BasicModifier.OperatorType.Add, 15));
        stats.Mediator.AddModifier(new BasicModifier(StatType.Hp, BasicModifier.OperatorType.Subtract, 5));
        Debug.Log(stats.Hp);
    }
}
    