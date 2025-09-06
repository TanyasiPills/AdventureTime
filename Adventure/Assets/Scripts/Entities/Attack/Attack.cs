using System;
using UnityEngine;

[Serializable]
public abstract class Attack : ScriptableObject
{
    public string attackName;
    [TextArea] public string desripction;
    public Element element;
    public abstract float AttackEnemy(Entity attacker, Entity enemy); //TODO: add position for AOE
}

[Serializable]
public abstract class AttackScaling
{
    public abstract float getDamage(Entity attacker, float currentDamage);
}

[Serializable]
public class BasicAttackScaling : AttackScaling
{
    public StatType type;
    public float value;
    public OperatorType operatorType;

    public BasicAttackScaling() {}

    public BasicAttackScaling(StatType type, OperatorType operatorType, float value)
    {
        this.type = type;
        this.operatorType = operatorType;
        this.value = value;
    }

    public override float getDamage(Entity attacker, float currentDamage)
    {
        switch (operatorType)
        {
            case OperatorType.Add: return currentDamage + (attacker.Stats.GetStat(type) * value);
            case OperatorType.Subtract: return currentDamage - (attacker.Stats.GetStat(type) * value);
            case OperatorType.Multiply: return currentDamage * (attacker.Stats.GetStat(type) * value);
            case OperatorType.Divide: return currentDamage / (attacker.Stats.GetStat(type) * value);
        }

        return currentDamage;
    }
}
