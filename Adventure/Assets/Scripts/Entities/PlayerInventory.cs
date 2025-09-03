using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum EquipmentType { Helmet, Chestplate, Leggings, Boots, Hand, Amulet, Ring }

public class PlayerInventory : Inventory
{
    public int consumableLimit = 12;
    public Dictionary<Consumable, int> consumables;
    public List<EquipmentSlot> equipments;
    private Entity entity;

    public Equipment cumhat;

    protected override void Awake()
    {
        base.Awake();
        consumables = new Dictionary<Consumable, int>(Math.Min(100, consumableLimit));

        equipments = new List<EquipmentSlot>(8);
        equipments.Add(new EquipmentSlot(EquipmentType.Helmet));
        equipments.Add(new EquipmentSlot(EquipmentType.Chestplate));
        equipments.Add(new EquipmentSlot(EquipmentType.Leggings));
        equipments.Add(new EquipmentSlot(EquipmentType.Boots));
        equipments.Add(new EquipmentSlot(EquipmentType.Hand));
        equipments.Add(new EquipmentSlot(EquipmentType.Amulet));
        equipments.Add(new EquipmentSlot(EquipmentType.Ring));
        equipments.Add(new EquipmentSlot(EquipmentType.Ring));

        entity = gameObject.GetComponent<Entity>();
        if (entity == null) throw new Exception("PlayerInventory's gameObject requires Entity");
    }

    [ContextMenu("Equip the Cum")]
    public void Cum()
    {
        AddItem(cumhat);
        Debug.Log(Equip(cumhat, 0));
    }

    public bool Equip(Equipment item, int slot)
    {
        if (item.type != equipments[slot].type) return false;
        if (RemoveItem(item))
        {
            if (equipments[slot].item != null)
            {
                equipments[slot].item.Remove(entity);
                ForceAddItem(equipments[slot].item);
            }
            equipments[slot].item = item;
            item.Equip(entity);
            return true;
        }
        return false;
    }

    public bool Consume(Consumable item)
    {
        if (RemoveItem(item))
        {
            item.Consume(entity);
            return true;
        }
        return false;
    }

    public override bool AddItem(Item item, int amount = 1)
    {
        if (item is Consumable)
        {
            if (!IsInsertable(item, amount)) return false;
            Consumable element = (Consumable)HasItem(item);
            if (element == null) consumables.Add((Consumable)item, amount);
            else consumables[element] += amount;
            return true;
        }

        return base.AddItem(item, amount);
    }

    protected override void ForceAddItem(Item item, int amount = 1)
    {
        if (item is Consumable)
        {
            Consumable element = (Consumable)HasItem(item);
            if (element == null) consumables.Add((Consumable)item, amount);
            else consumables[element] += amount;
        } else base.ForceAddItem(item, amount);
    }

    public override Item HasItem(Item item, int amount = 1)
    {
        if (item is Consumable)
        {
            foreach (KeyValuePair<Consumable, int> kvp in consumables)
            {
                if (kvp.Key.id == item.id && kvp.Value >= amount)
                {
                    return kvp.Key;
                }
            }
            return null;
        }
        return base.HasItem(item, amount);
    }

    public override bool IsInsertable(Item item, int amount = 1)
    {
        if (item is Consumable) return consumableLimit - consumables.Count >= amount;
        return base.IsInsertable(item, amount);
    }

    public override bool RemoveItem(Item item, int amount = 1)
    {
        if (item is Consumable)
        {
            Consumable element = (Consumable)HasItem(item, amount);
            if (element == null) return false;

            consumables[element] -= amount;
            if (consumables[element] == 0)
            {
                consumables.Remove(element);
            }
            return true;
        }
        return base.RemoveItem(item, amount);
    }
}

public class EquipmentSlot
{
    public EquipmentType type;
    public Equipment item;

    public EquipmentSlot(EquipmentType type)
    {
        this.type = type;
        this.item = null;
    }
}
