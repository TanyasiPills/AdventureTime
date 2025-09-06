using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Items/Item")]
public class Item : ScriptableObject
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
