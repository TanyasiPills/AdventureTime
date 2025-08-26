using System.Collections.Generic;
using UnityEngine;

public class StatsMediator
{
    readonly LinkedList<StatModifier> modifiers = new LinkedList<StatModifier>();

    public int PerformQuery(StatType type, int baseValue)
    {
        foreach (StatModifier modifier in modifiers)
        {
            baseValue = modifier.Handle(type, baseValue);
        }
        return baseValue;
    }

    public void AddModifier(StatModifier modifier)
    {
        if (modifiers.Count == 0) modifiers.AddFirst(modifier);
        else
        {
            var node = modifiers.First;
            while (node != null)
            {
                if (node.Value.priority >= modifier.priority)
                {
                    modifiers.AddBefore(node, modifier);
                    break;
                }
                if (node.Next == null)
                {
                    modifiers.AddAfter(node, modifier);
                    break;
                }
                node = node.Next;
            }
        }

        modifier.OnDispose += _ =>
        {
            modifiers.Remove(modifier);
        };
    }

    public void Update(int turn)
    {
        var node = modifiers.First;
        while (node != null)
        {
            StatModifier modifier = node.Value;
            modifier.Update(turn);
            node = node.Next;
        }

        node = modifiers.First;
        while (node != null)
        {
            var nextNode = node.Next;

            if (node.Value.markedForRemoval)
            {
                node.Value.Dispose();
            }

            node = nextNode;
        }
    }
}
