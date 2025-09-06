using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public enum EquipmentType { Helmet, Chestplate, Leggings, Boots, Hand, Amulet, Ring }

public class PlayerInventory : Inventory
{
    public int consumableLimit = 12;
    public Dictionary<Consumable, int> consumables;
    public List<EquipmentSlot> equipments;
    public EntityContainer entityContainer;
    public Loadout loadout;

    public Equipment cumhat;

    protected override void Awake() //TODO: init possesions from server
    {
        base.Awake();
        consumables = new Dictionary<Consumable, int>(Math.Min(100, consumableLimit));

        entityContainer = new EntityContainer();
        loadout = new Loadout();

        equipments = new List<EquipmentSlot>(8);
        equipments.Add(new EquipmentSlot(EquipmentType.Helmet));
        equipments.Add(new EquipmentSlot(EquipmentType.Chestplate));
        equipments.Add(new EquipmentSlot(EquipmentType.Leggings));
        equipments.Add(new EquipmentSlot(EquipmentType.Boots));
        equipments.Add(new EquipmentSlot(EquipmentType.Hand));
        equipments.Add(new EquipmentSlot(EquipmentType.Amulet));
        equipments.Add(new EquipmentSlot(EquipmentType.Ring));
        equipments.Add(new EquipmentSlot(EquipmentType.Ring));

        loadout = gameObject.GetComponent<Loadout>();
        if (loadout == null) throw new Exception("PlayerInventory's gameObject requires Loadout");
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
                equipments[slot].item.Remove(loadout.Main);
                equipments[slot].item.Remove(loadout.Support);
                ForceAddItem(equipments[slot].item);
            }
            equipments[slot].item = item;
            item.Equip(loadout.Main);
            item.Equip(loadout.Support);
            return true;
        }
        return false;
    }

    private void RemoveEquipmentEffectsFromEntity(Entity entity)
    {
        foreach (EquipmentSlot slot in equipments)
        {
            if (slot.item != null) slot.item.Remove(entity);
        }
    }

    private void AddEquipmentEffectsToEntity(Entity entity)
    {
        foreach (EquipmentSlot slot in equipments)
        {
            if (slot.item != null) slot.item.Equip(entity);
        }
    }

    public bool Consume(Consumable item, Entity target)
    {
        if (RemoveItem(item))
        {
            item.Consume(target);
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
            if (!consumables.ContainsKey((Consumable)item)) return null;
            if (consumables[(Consumable)item] >= amount) return item;
            return null;
        } else
        {
            if (!items.ContainsKey(item)) return null;
            if (items[item] >= amount) return item;
            return null;
        }
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
    
    public bool EquipEntity(Entity entity)
    {
        if (!loadout.IsInsertable()) return false;
        return ForceEquipEntity(entity);
    }

    public bool ForceEquipEntity(Entity entity)
    {
        if (!entityContainer.RemoveEntity(entity)) return false;

        loadout.ForceAdd(entity);

        AddEquipmentEffectsToEntity(entity);

        return true;
    }

    public bool UnequipEntity(Entity entity)
    {
        if (!entityContainer.IsInsertable()) return false;

        return ForceUnequipEntity(entity);
    }

    public bool UnequipEntity(int index)
    {
        if (!entityContainer.IsInsertable()) return false;

        return ForceUnequipEntity(index);
    }

    public bool ForceUnequipEntity(Entity entity)
    {
        if (!loadout.Remove(entity)) return false;
        entityContainer.ForceAddEntity(entity);
        RemoveEquipmentEffectsFromEntity(entity);
        return true;
    }

    public bool ForceUnequipEntity(int index)
    {
        Entity entity = loadout.Remove(index);
        if (entity == null) return false;
        entityContainer.ForceAddEntity(entity);
        RemoveEquipmentEffectsFromEntity(entity);
        return true;
    }

    public bool SwitchEntity(Entity oldEntity, Entity newEntity)
    {
        if (newEntity == null) return false;
        if (!loadout.HasEntity(oldEntity) || !entityContainer.HasEntity(newEntity)) return false;

        ForceUnequipEntity(oldEntity);
        ForceEquipEntity(newEntity);

        return true;
    }

    public bool SwitchEntity(int index, Entity newEntity)
    {
        Entity oldEntity = loadout.Get(index);

        return SwitchEntity(oldEntity, newEntity);
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
