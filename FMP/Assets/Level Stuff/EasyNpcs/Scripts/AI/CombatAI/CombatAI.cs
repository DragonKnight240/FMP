using System;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;
using Rotation;
using UnityEditor;

namespace Npc_AI
{
    public class CombatAI : NpcBase, IAttackable
    {
        [TagSelector] public List<string> Tags;
        public List<string> Protects;

        public Transform currentTarget;

        public float AttackDistance;
        public enum Weapon { melee, ranged};
        public Weapon assignedWeapon; 
        public Projectile projectile;
        public float launchHight;

        public Transform guardPost;
        public List<Transform> patrolSpots;
        int spotNum = 0;

        protected override void OnIdle()
        {
            agent.speed = walkSpeed;
            ChangeState(NpcState.Patrol);
        }

        [DrawGizmo( GizmoType.Selected | GizmoType.NonSelected, typeof( CombatAI ) )] //Jesse
        public static void DrawGizmo( CombatAI npcAI, GizmoType type ) {
            Gizmos.color = Color.green;
            CapsuleCollider collider = npcAI.GetComponent<CapsuleCollider>();
            Vector3 npcPos = npcAI.transform.position;
            Gizmos.matrix = Matrix4x4.TRS( new Vector3(npcPos.x, collider == null ? npcPos.y : npcPos.y + collider.height*0.9f, npcPos.z), 
                npcAI.transform.rotation, npcAI.transform.lossyScale );
            if(Selection.Contains (npcAI.gameObject))
                Gizmos.DrawFrustum( Vector3.zero, npcAI.VisionAngle, npcAI.VisionRange, 0f, 2.5f );
        }

        protected override void OnPatrol()
        {
            if (guardPost == null)
            {
                if (spotNum == patrolSpots.Count)
                {
                    agent.SetDestination(patrolSpots[0].position);
                    spotNum = 0;
                    return;
                }

                agent.SetDestination(patrolSpots[spotNum].position);
                spotNum++;
            }
            else
            {
                agent.SetDestination(guardPost.position);
            }
        }

        protected override void OnAttack()
        {
            Rotate rotate = gameObject.AddComponent<Rotate>();
            rotate.RotateTo(currentTarget);
        }

        void Update()
        {
            State_On_Update();
        }

        void State_On_Update()
        {
            switch (currentState)
            {
                case NpcState.Patrol:
                    Update_OnPatrol();
                    break;

                case NpcState.Chase:
                    Update_OnChase();
                    break;

                case NpcState.Attack:
                    Update_OnAttack();
                    break;

                default:
                    break;
            }
        }

        public float walkSpeed = 2;

        void Update_OnPatrol()
        {
            if (currentTarget == null)
            {
                TryToFindTarget();
            }
            else
            {
                ChangeState(NpcState.Chase);
            }
        }

        void TryToFindTarget()
        {
            Transform target = CombatAISense.CheckForTargets(this); 
            if (target != null)
            {
                currentTarget = target;
                ChangeState(NpcState.Chase);

                return;
            }
            
            if (Protect() == null)
                No_Target_Available();
        }

        private Transform Protect()
        {
            currentTarget = CombatAISense.BattleAI_Sense_Friendly_Attacked(transform.position, VisionRange, VisionLayers, Protects);
            return currentTarget;
        }

        void No_Target_Available()
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                ChangeState(NpcState.Idle);
            }
        }

        public float runSpeed = 4;

        void Update_OnChase()
        {
            agent.speed = runSpeed;
            if (currentTarget.GetComponent<CharacterStats>().isDead == false)
            {
                if (CombatAISense.Check_Target_Distance_And_Raycast(transform, currentTarget, AttackDistance))
                {
                    ChangeState(NpcState.Attack);
                }
                else
                {
                    SetDestination_For_Chase(currentTarget);
                }
            }
        }

        void SetDestination_For_Chase(Transform target)
        {
            if (agent.destination != currentTarget.position)
            {
                currentTarget = target;
                agent.SetDestination(target.position);
            }
        }

        void Update_OnAttack()
        {
            agent.SetDestination(transform.position);
            OnTargetDead();

            if (CombatAISense.Check_Target_Distance_And_Raycast(transform, currentTarget, AttackDistance))
            {
                Trigger_Attack_Anim(currentTarget.gameObject);
            }
            else
            {
                ChangeState(NpcState.Chase);
            }
        }

        void OnTargetDead()
        {
            if (currentTarget.GetComponent<CharacterStats>().isDead == true)
            {
                currentTarget = null;
                ChangeState(NpcState.Idle);

                return;
            }
        }

        void Trigger_Attack_Anim(GameObject target)
        {
            anim.SetTrigger("Attack");
        }

        public void Attacked(GameObject attacker, Attack attack)
        {
            currentTarget = attacker.transform;
        }
    }
}
