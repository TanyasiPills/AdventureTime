using System;
using System.Collections.Generic;

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
        modifiers.AddLast(modifier);

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
