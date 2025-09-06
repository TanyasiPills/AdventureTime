using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicAttack", menuName = "ScriptableObjects/Attacks/BasicAttack")]
[Serializable]
public class BasicAttack : Attack
{
    public float baseDamage;
    public float baseDamageScaling;
    [SerializeReference]
    public List<AttackScaling> scaling;

    public void OnEnable()
    {
        for (int i = scaling.Count - 1; i >= 0; i--)
        {
            if (scaling[i] == null) scaling.RemoveAt(i);
        }
    }
    public override float AttackEnemy(Entity attacker, Entity enemy)
    {
        float damage = baseDamage + (attacker.Level - 1) * baseDamageScaling;
        foreach (AttackScaling s in scaling)
        {
            damage = s.getDamage(attacker, damage);
        }
        damage *= element.GetModifier(enemy.element);
        enemy.Damaged(damage, element);
        return damage;
    }
}
