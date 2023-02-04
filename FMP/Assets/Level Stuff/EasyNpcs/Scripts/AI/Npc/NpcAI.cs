using System;
using UnityEngine;

namespace Npc_AI
{
    public class NpcAI : NpcBase, IAttackable
    {
        public float movementSpeed;
        public float scaredRunningSpeed;
        public float runningDistance;
        public float runningTime;

        DayAndNightControl dayAndNightControl;
        Behaviour workScript;
        public Transform home;
        public Transform work;

        protected override void Start()
        {
            base.Start();
            DayAndNightCycle_Initialize();
            workScript = GetComponent<Work>();
        }

        void DayAndNightCycle_Initialize()
        {
            dayAndNightControl = FindObjectOfType<DayAndNightControl>();

            if (dayAndNightControl != null)
            {
                dayAndNightControl.OnMorningHandler += GoToWork;
                dayAndNightControl.OnEveningHandler += GoToHome;
            }
            else
            {
                Debug.Log("Add in dayAndNight control to scene for use of npc's life cycle");
            }
        }

        void Update()
        {
            WatchEnvironment_EveryUpdate();
        }

        GameObject attacker;

        void WatchEnvironment_EveryUpdate()
        {
            attacker = NpcSense.NPC_Sense_Attacked(transform.position, VisionRange, VisionLayers);
            if (attacker != null)
            {
                if (currentState != NpcState.Scared)
                    ChangeState(NpcState.Scared);
            }
            else
            {
                TriggerConversation(NpcSense.Sense_Nearby_Npc(transform.position, 5, VisionLayers, this));
            }
        }

        protected override void TurnOffBehaviour(NpcState prevState)
        {
            base.TurnOffBehaviour(prevState);
            switch (prevState)
            {
                case NpcState.GoingToWork:
                    Destroy(GetComponent<LifeCycle>());
                    break;

                case NpcState.GoingHome:
                    Destroy(GetComponent<LifeCycle>());
                    break;

                case NpcState.Working:
                    if (workScript != null)
                        workScript.enabled = false;
                    break;
            }
        }

        protected override void OnIdle()
        {
            if (dayAndNightControl != null)
            {
                float time = dayAndNightControl.currentTime;
                if (time > .3f && time < .7f)
                {
                    GoToWork();
                }
                else
                {
                    GoToHome();
                }
            }
        }

        void GoToWork()
        {
            ChangeState(NpcState.GoingToWork);
        }

        protected override void OnGoingToWork()
        {
            Set_Cycle_Class().Start_GOTOWork();
        }

        void GoToHome()
        {
            ChangeState(NpcState.GoingHome);
            Set_Cycle_Class().Start_GOTOHome();
        }

        LifeCycle Set_Cycle_Class()
        {
            LifeCycle lifeCycle = gameObject.AddComponent<LifeCycle>();
            lifeCycle.Set(this);

            return lifeCycle;
        }

        protected override void OnWorking()
        {
            if (workScript != null)
                workScript.enabled = true;
        }

        public void Attacked(GameObject attacker, Attack attack)
        {
            this.attacker = attacker;
            ChangeState(NpcState.Scared);
        }

        protected override void OnScared()
        {
            Choose_RunAway_Point choose_RunAway_Point = gameObject.AddComponent<Choose_RunAway_Point>();
            if (!GetComponent<CharacterStats>().isDead)
                StartCoroutine(choose_RunAway_Point.Run(attacker));
            else
                choose_RunAway_Point.attacker = attacker;
        }

        [Range(0, 5)]
        public int converChoose = 0;

        void TriggerConversation(NpcAI npc)
        {
            if (CheckConditions_ForTalk(npc))
            {
                RunConversation runConversation = gameObject.AddComponent<RunConversation>();
                runConversation.Set(npc, true);
            }
        }

        bool CheckConditions_ForTalk(NpcAI npc)
        {
            if (npc == null)
                return false;
            if (currentState == NpcState.Talking || npc.currentState == NpcState.Talking)
                return false;
            if (UnityEngine.Random.Range(0, 10000) > converChoose)
                return false;
            if (GetInstanceID() < npc.GetInstanceID())
                return false;

            return true;
        }
    }
}