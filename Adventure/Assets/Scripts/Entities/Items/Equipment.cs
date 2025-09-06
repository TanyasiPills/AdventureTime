using System;
using System.Collections.Generic;
using UnityEngine;

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
