using UnityEngine;

[CreateAssetMenu(fileName = "BuffConsumable", menuName = "ScriptableObjects/Items/BuffConsumable")]
public class BuffConsumable : Consumable
{
    [SerializeReference]
    public StatModifier modifier;
    public override void Consume(Entity entity)
    {
        entity.Stats.Mediator.AddModifier(modifier);
    }
}
