using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Npc_AI
{
    public static class CombatAISense 
    {
        public static Transform BattleAI_Sense_Friendly_Attacked(Vector3 position, float VisionRange, LayerMask VisionLayers, List<string> tags)
        {
            Collider[] cols = Physics.OverlapSphere(position, VisionRange, VisionLayers);
            foreach (Collider col in cols)
            {
                Choose_RunAway_Point danger = col.GetComponentInParent<Choose_RunAway_Point>();
                if (danger)
                {
                    if (CheckTag(col, tags))
                    {
                        return danger.attacker.transform;
                    }

                    return null;
                }
            }

            return null;
        }

        public static Transform CheckForTargets(CombatAI enemyAI)
        {
            List<Collider> possibleTargets = PossibleTargets(enemyAI.transform.position, enemyAI.VisionRange, enemyAI.VisionLayers, enemyAI.Tags, enemyAI);
            Collider nearestTarget = NearestTarget(possibleTargets, enemyAI.transform.position);
            if (nearestTarget != null)
                return nearestTarget.transform;
            else
                return null;
        }

        static List<Collider> PossibleTargets(Vector3 position, float VisionRange, LayerMask VisionLayers, List<string> tags, CombatAI enemyAI)
        {
            List<Collider> posssibleTargets = new List<Collider>();
            Collider[] cols = Physics.OverlapBox(position, 
                new Vector3(VisionRange, VisionRange, VisionRange), enemyAI.transform.rotation, enemyAI.VisionLayers);
            foreach (Collider col in cols)
            {
                if (col.transform.parent != enemyAI.transform)
                {
                    if (Physics.Linecast(position + new Vector3(0, 1), col.transform.position + new Vector3(0, 1),
                        out RaycastHit hit, VisionLayers))
                    {
                        if (CheckTag(col, tags))
                        {
                            if (CheckAngle(col, enemyAI) < enemyAI.VisionAngle)
                                posssibleTargets.Add(col);
                        }
                    }
                }
            }

            return posssibleTargets;
        }

        static float CheckAngle(Collider col, CombatAI ai)
        {
            Vector3 targetDir = col.transform.position - ai.transform.position;
            return Vector3.Angle(targetDir, ai.transform.forward);
        }

        static bool CheckTag(Collider col, List<string> tags)
        {
            for (int i = 0; i < tags.Count; i++)
            {
                if (col.gameObject.CompareTag(tags[i]))
                {
                    return true;
                }
            }

            return false;
        }

        static Collider NearestTarget(List<Collider> possibleTargets, Vector3 position)
        {
            Collider nearestTarget = null;
            if (possibleTargets.Count > 0)
            {
                nearestTarget = possibleTargets[0];
                for (int i = 1; i < possibleTargets.Count; i++)
                {
                    if (Vector3.Distance(possibleTargets[i].transform.position, position)
                        < Vector3.Distance(nearestTarget.transform.position, position))
                        nearestTarget = possibleTargets[i];
                }
            }

            return nearestTarget;
        }

        public static bool Check_Target_Distance_And_Raycast(Transform me, Transform target, float attackDistance)
        {
            RaycastHit hit;
            Physics.Raycast(me.position + new Vector3(0, 1), target.position - me.position, out hit, Mathf.Infinity);
            if ((target.position - me.position).magnitude <= attackDistance && hit.transform == target)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}