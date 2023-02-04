using System;
using UnityEngine;

namespace Npc_AI
{
    public class CharacterStats : MonoBehaviour, IAttackable
    {
        public Stat maxHealth;
        public Stat currentHealth { get; private set; }

        public Stat Damage;
        public Stat Armor;

        public bool isDead = false;

        public event Action OnHealthValueChanged;

        protected virtual void Start()
        {
            currentHealth = new Stat();
            currentHealth.SetValue(maxHealth.GetValue());
        }

        public virtual void Attacked(GameObject attacker, Attack attack)
        {
            TakeDamage(attacker, attack.Damage);
            if (GetCurrentHealth().GetValue() <= 0)
            {
                OnDeath(attacker);
            }
        }

        void TakeDamage(GameObject attacker, float damage)
        {
            if (damage <= 0f) return;
            currentHealth.SetValue(currentHealth.GetValue() - damage);

            OnHealthValueChanged?.Invoke();
        }

        protected virtual void OnDeath(GameObject attacker)
        {
            IAttackable[] destructibles = GetComponents<IAttackable>();
            foreach (IAttackable destructible in destructibles)
            {
                destructible.OnDestruction(attacker);
            }
        }

        public Stat GetArmor()
        {
            return Armor;
        }

        public Stat GetDamage()
        {
            return Damage;
        }

        public Stat GetCurrentHealth()
        {
            return currentHealth;
        }

        public Stat GetMaxHealth()
        {
            return maxHealth;
        }

        public void OnDestruction(GameObject destroyer)
        {
            isDead = true;
        }
    }
}

