using UnityEngine;

namespace Systems.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 100;

        public HealthSystem HealthSystem { get; private set; }

        void Awake()
        {
            HealthSystem = new HealthSystem(maxHealth);
        }
        
        public void Damage(int value)
        {
            HealthSystem.Damage(value);
        }

        public void Heal(int value)
        {
            HealthSystem.Heal(value);
        }
    }
}