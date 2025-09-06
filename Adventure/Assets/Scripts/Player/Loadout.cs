using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Loadout : MonoBehaviour
{
    public int size = 4;
    private List<Entity> entities;
    private Entity main;
    private Entity support;

    public Entity Main { get; }
    public Entity Support { get; }

    public void Awake()
    {
        entities = new List<Entity>();
    }

    public bool ChangeMain(Entity entity)
    {
        if (!HasEntity(entity)) return false;
        if (support == entity) return false;

        main = entity;
        return true;
    }

    public bool ChangeMain(int index)
    {
        if (entities.Count <= index || index < 0) return false;
        if (support == entities[index]) return false;

        main = entities[index];
        return true;
    }

    public bool ChangeSupport(Entity entity)
    {
        if (!HasEntity(entity)) return false;
        if (main == entity) return false;

        support = entity;
        return true;
    }

    public bool ChangeSupport(int index)
    {
        if (entities.Count <= index || index < 0) return false;
        if (main == entities[index]) return false;

        support = entities[index];
        return true;
    }

    public bool Add(Entity entity)
    {
        if (entity == null) return false;
        if (entities.Count >= size) return false;

        entities.Add(entity);
        return true;
    }

    public void ForceAdd(Entity entity)
    {
        if (entity == null) return;
        entities.Add(entity);
    }

    public bool IsInsertable(int amount = 1)
    {
        return entities.Count + amount <= size;
    }

    public bool Remove(Entity entity)
    {
        return entities.Remove(entity);
    }

    public Entity Remove(int index)
    {
        if (entities.Count <= index || index < 0) return null;
        Entity entity = entities[index];
        entities.RemoveAt(index);

        return entity;
    }

    public Entity Get(int index)
    {
        if (index < 0 || index >= entities.Count) return null;
        return entities[index];
    }

    public bool HasEntity(Entity entity)
    {
        for (int i = 0; i < entities.Count; i++)
        {
            if (entities[i] == entity) return true;
        }
        return false;
    }

    public bool HasEntity(int index)
    {
        return (index >= 0  && index < entities.Count);
    }
}
