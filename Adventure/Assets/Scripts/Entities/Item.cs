using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Item")]
public abstract class Item : ScriptableObject
{
    public int id;
    public string itemName;
    [TextArea]
    public string description;
}

public abstract class Consumable : Item
{
    public abstract void Consume(Entity entity);
}

[CreateAssetMenu(fileName = "BuffConsumable", menuName = "ScriptableObjects/Items/BuffConsumable")]
public class BuffConsumable : Consumable
{
    [SerializeReference]
    public StatModifier modifier;
    public override void Consume(Entity entity)
    {
        entity.Stats.Mediator.AddModifier(modifier);
    }
}

public enum HealType { Percent, Flat }

[CreateAssetMenu(fileName = "HealConsumable", menuName = "ScriptableObjects/Items/HealConsumable")]
public class HealConsumable : Consumable
{
    public HealType type;
    public float value;
    public override void Consume(Entity entity)
    {
        if (type == HealType.Flat) entity.Heal(value);
        else entity.HealPercent(value);
    }
}

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Items/Equipment")]
[Serializable]
public class Equipment : Item
{
    public EquipmentType type;
    [SerializeReference]
    public List<StatModifier> modifiers;

    public void Equip(Entity entity)
    {
        foreach (var modifier in modifiers)
        {
            entity.Stats.Mediator.AddModifier(modifier);
        }
    }

    public void Remove(Entity entity)
    {
        foreach (var modifier in modifiers)
        {
            modifier.Remove();
        }
    }
}
