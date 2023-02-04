using System.Collections.Generic;
using UnityEngine;

namespace Npc_AI
{
    public static class NpcSense
    {
        public static GameObject NPC_Sense_Attacked(Vector3 position, float VisionRange, LayerMask VisionLayers)
        {
            Collider[] cols = Physics.OverlapSphere(position, VisionRange, VisionLayers);
            foreach (Collider col in cols)
            {
                Choose_RunAway_Point choose_RunAway_Point = col.GetComponentInParent<Choose_RunAway_Point>();
                if (choose_RunAway_Point)
                {
                    return choose_RunAway_Point.attacker;
                }
            }

            return null;
        }

        public static NpcAI Sense_Nearby_Npc(Vector3 position, float VisionRange, LayerMask VisionLayers, NpcAI thisNpc)
        {
            Collider[] cols = Physics.OverlapSphere(position, VisionRange, VisionLayers);
            foreach (Collider col in cols)
            {
                if (col.GetComponent<NpcAI>())
                {
                    NpcAI npc = col.GetComponent<NpcAI>();
                    if (npc != thisNpc)
                        return npc;
                }
            }

            return null;
        }
    }
}