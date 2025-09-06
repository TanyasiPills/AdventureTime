using System;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private float _health;
    private int _level = 1;
    private Stats stats;

    [SerializeReference]
    public BaseStats baseStats;
    public Element element;
    [SerializeReference]
    public List<Attack> attacks;

    public Action<Entity> OnDeath = delegate { };
    public Action<int> OnLevelChange = delegate { };

    public Stats Stats { get => stats; }
    public int Level { get => _level; private set {
            _level = value;
            OnLevelChange.Invoke(_level);
        }
    }

    public float Health { get => _health; private set
        {
            _health = Mathf.Min(value, Stats.Hp);
            if (_health <= 0)
            {
                _health = 0;
                OnDeath.Invoke(this);
                return;
            }
        } 
    }


    public void Heal(float value)
    {
        Health += value;
    }

    public void HealPercent(float percent)
    {
        Health = Health + Stats.Hp * percent;
    }

    public void Damaged(float value, Element element)
    {
        Health -= value;
    }

    void Start()
    {
        stats = new Stats(baseStats, Level, OnLevelChange);
        _health = stats.Hp;
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
    