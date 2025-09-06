using System.Collections.Generic;
using UnityEngine;

public class EntityContainer : MonoBehaviour
{
    public int entityLimit = 12;
    public List<Entity> entities;

    public int IndexOfEntity(Entity entity)
    {
        return entities.IndexOf(entity);
    }

    public bool AddEntity(Entity entity)
    {
        if (entities.Count >= entityLimit) return false;

        entities.Add(entity);
        return true;
    }

    public void ForceAddEntity(Entity entity)
    {
        entities.Add(entity);
    }

    public bool IsInsertable(int amount = 1)
    {
        return entities.Count + amount <= entityLimit;
    }

    public bool RemoveEntity(Entity entity)
    {
        return entities.Remove(entity);
    }

    public bool MoveEntityTo(EntityContainer other, Entity entity)
    {
        if (other == null) return false;
        if (!other.IsInsertable()) return false;

        if (!this.RemoveEntity(entity)) return false;
        other.AddEntity(entity);
        return true;
    }

    public bool HasEntity(Entity entity)
    {
        return entities.Contains(entity);
    }
}
