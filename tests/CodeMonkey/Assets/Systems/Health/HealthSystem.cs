using System;

namespace Systems.Health
{
    public class HealthSystem
    {
        public int MaxHealth { get; private set; }
        public int Health { get; private set; }

        public event Action OnHealthChanged;

        public HealthSystem(int maxHealth)
        {
            MaxHealth = maxHealth;
            Health = maxHealth;
        }

        public void Damage(int value)
        {
            Health -= value;
            if (Health < 0)
            {
                Health = 0;
            }

            OnHealthChanged?.Invoke();
        }

        public void Heal(int value)
        {
            Health += value;
            if (Health > MaxHealth)
            {
                Health = MaxHealth;
            }

            OnHealthChanged?.Invoke();
        }
    }
}