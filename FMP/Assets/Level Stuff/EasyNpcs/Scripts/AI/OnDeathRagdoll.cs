using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Rotation;

namespace Ragdoll
{
    public class OnDeathRagdoll : MonoBehaviour, IAttackable
    {
        Rigidbody[] rigs;
        SkinnedMeshRenderer[] skins;

        void Start()
        {
            skins = GetComponentsInChildren<SkinnedMeshRenderer>();
            rigs = GetComponentsInChildren<Rigidbody>();
            Turn_Off_ChildRigs_At_Start(true);
            GetComponent<CapsuleCollider>().enabled = true;
        }

        void Turn_Off_ChildRigs_At_Start(bool on)
        {
            foreach (Rigidbody rigidbody in rigs)
            {
                Rigidbody mainRigidBody = GetComponent<Rigidbody>();
                if (rigidbody != mainRigidBody)
                {
                    rigidbody.GetComponent<Collider>().enabled = !on;
                    rigidbody.isKinematic = on;
                }
            }
        }

        public void OnDestruction(GameObject destoyer)
        {
            DestroyRotate();
            UpdateWhenOffScreen_For_Each_Rig();

            GetComponentInChildren<Animator>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = false;
            Destroy(GetComponent<CapsuleCollider>());
            Destroy(GetComponent<Rigidbody>());

            Turn_Off_ChildRigs_At_Start(false);
        }

        void UpdateWhenOffScreen_For_Each_Rig()
        {
            foreach (SkinnedMeshRenderer skinned in skins)
            {
                skinned.updateWhenOffscreen = true;
            }
        }

        void DestroyRotate()
        {
            Rotate rotate = GetComponent<Rotate>();
            if (rotate != null)
            {
                Destroy(rotate);
            }
        }

        public void Attacked(GameObject attacker, Attack attack) { }
    }
}