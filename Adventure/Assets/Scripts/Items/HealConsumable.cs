using UnityEngine;

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
