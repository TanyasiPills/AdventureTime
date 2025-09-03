using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.Searcher;
using UnityEngine;
using static UnityEditor.Rendering.FilterWindow;

public class Inventory : MonoBehaviour
{
    public int itemLimit = 100;
    public Dictionary<Item, int> items;

    protected virtual void Awake()
    {
        items = new Dictionary<Item, int>(Math.Min(100, itemLimit));
    }

    public virtual bool AddItem(Item item, int amount = 1)
    {
        if (!IsInsertable(item, amount)) return false;
        Item element = HasItem(item);
        if (element == null) items.Add(item, amount);
        else items[element] += amount;
        return true;
    }

    protected virtual void ForceAddItem(Item item, int amount = 1)
    {
        Item element = HasItem(item);
        if (element == null) items.Add(item, amount);
        else items[element] += amount;
    }

    public virtual bool RemoveItem(Item item, int amount = 1)
    {
        Item element = HasItem(item, amount);
        if (element == null) return false;

        items[element] -= amount;
        if (items[element] == 0)
        {
            items.Remove(element);
        }
        return true;
    }

    public bool MoveTo(Inventory other, Item item, int amount = 1)
    {
        if (other.IsInsertable(item, amount) && RemoveItem(item, amount))
        {
            other.AddItem(item, amount);
            return true;
        }
        return false;
    }

    public virtual bool IsInsertable(Item item, int amount = 1)
    {
        return itemLimit - items.Count >= amount;
    }

    public virtual Item HasItem(Item item, int amount = 1)
    {
        foreach (KeyValuePair<Item, int> kvp in items)
        {
            if (kvp.Key.id == item.id && kvp.Value >= amount)
            {
                return kvp.Key;
            }
        }
        return null;
    }
}
