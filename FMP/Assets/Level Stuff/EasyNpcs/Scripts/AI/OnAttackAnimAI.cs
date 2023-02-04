using UnityEngine;
using Npc_AI;

public class OnAttackAnimAI : StateMachineBehaviour
{
    CombatAI enemyAI;
    GameObject thisNpc;
    public GameObject swordSound;
    public GameObject blockSound;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        enemyAI = animator.GetComponentInParent<CombatAI>();
        thisNpc = enemyAI.gameObject;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (enemyAI != null)
        {
            On_Ai(animator);
        }
    }

    void On_Ai(Animator animator)
    {
        if (enemyAI.currentTarget != null)
        {
            if (enemyAI.assignedWeapon == CombatAI.Weapon.melee)
            {
                MeleeAttack(animator);
            }
            else
            {
                RangedAttack();
            }
        }
    }

    void ExecuteAttack()
    {
        if (AttackManager.AttackTarget(enemyAI.gameObject, enemyAI.currentTarget.gameObject))
        {
            Instantiate(swordSound);
        }
        else
        {
            Instantiate(blockSound);
        }
    }

    void MeleeAttack(Animator animator)
    {
        if (Random.Range(0, 99) < 19)
        {
            animator.SetBool("Block", true);
        }
        else
        {
            ExecuteAttack();
        }
    }

    void RangedAttack()
    {
        Projectile projectile = Instantiate(enemyAI.projectile, thisNpc.transform.position + thisNpc.transform.forward * 1 + new Vector3(0, enemyAI.launchHight, 0), enemyAI.transform.rotation);
        projectile.Fire(thisNpc, enemyAI.currentTarget.gameObject, enemyAI.currentTarget.rotation, 10, 10);
    }
}
