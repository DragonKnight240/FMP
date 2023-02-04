using UnityEngine;

namespace Npc_AI
{
    public static class CheckState
    {
        public static bool Check_CharacterManager(GameObject npc)
        {
            if (npc.GetComponent<CharacterStats>() != null)
            {
                if (!npc.GetComponent<CharacterStats>().isDead)
                {
                    return true;
                }

                Debug.Log("Npc is dead");
                return false;
            }
            else
            {
                return false;
            }
        }

        public static bool Check_State(GameObject npc)
        {
            if (npc.GetComponent<NpcAI>() != null)
            {
                NpcAI npcAI = npc.GetComponent<NpcAI>();
                if (npcAI.enabled)
                {
                    return State_NotScared(npcAI);
                }

                Debug.Log("NpcAI of" + npc + "is not enabled");
                return false;
            }
            else if (npc.GetComponent<CombatAI>() != null)
            {
                CombatAI enemyAI = npc.GetComponent<CombatAI>();
                if (enemyAI.enabled)
                {
                    return State_NotScared(enemyAI);
                }

                return false;
            }

            return false;
        }

        static bool State_NotScared(NpcAI npcAI)
        {
            if (npcAI.currentState == NpcState.Scared)
            {
                Debug.Log("The npc's current state blocks interaction");
                return false;
            }
            else
            {
                npcAI.enabled = false;
                return true;
            }
        }

        static bool State_NotScared(CombatAI enemyAI)
        {
            if (enemyAI.currentState == NpcState.Chase || enemyAI.currentState == NpcState.Attack)
            {
                Debug.Log("The npc's current state blocks interaction");
                return false;
            }
            else
            {
                enemyAI.enabled = false;
                return true;
            }
        }
    }
}
