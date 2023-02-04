using UnityEngine;
using Npc_AI;

public static class AttackManager
{
    public static bool AttackTarget(GameObject attacker, GameObject target)
    {
        Attack attack = CreateAttack(attacker.GetComponent<CharacterStats>(), target.GetComponent<CharacterStats>());

        if (NotBlocked(target.GetComponentInChildren<Animator>()))
        {
            var attackables = target.GetComponentsInChildren(typeof(IAttackable));
            foreach (IAttackable attackable in attackables)
            {
                attackable.Attacked(attacker, attack);
            }

            return true;
        }

        return false;
    }

    static bool NotBlocked(Animator anim)
    {
        if(anim.GetBool("Block"))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public static Attack CreateAttack(CharacterStats attacker, CharacterStats defender)
    {
        float baseDamage = attacker.GetDamage().GetValue();

        if (defender != null)
            baseDamage -= defender.GetArmor().GetValue();

        if (baseDamage < 0)
            baseDamage = 0;
        return new Attack((int)baseDamage);
    }
}
