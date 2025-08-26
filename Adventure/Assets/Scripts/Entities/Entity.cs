using System;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private int level = 1;
    public BaseStats baseStats;
    private Stats stats;

    public Action<int> OnLevelChange = delegate { };

    public Stats Stats { get => stats; }
    public int Level { get => level; private set {
            level = value;
            OnLevelChange.Invoke(level);
        }
    }

    void Start()
    {
        stats = new Stats(baseStats, level, OnLevelChange);
    }
}
