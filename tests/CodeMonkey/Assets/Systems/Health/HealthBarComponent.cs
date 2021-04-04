using UnityEngine;

namespace Systems.Health
{
    public class HealthBarComponent : MonoBehaviour
    {
        [SerializeField] private GameObject fill;

        [SerializeField] private HealthComponent healthComponent;

        private void Start()
        {
            healthComponent.HealthSystem.OnHealthChanged += UpdateFillBar;
        }

        private void UpdateFillBar()
        {
            fill.transform.localScale =
                new Vector3(healthComponent.HealthSystem.Health / (float) healthComponent.HealthSystem.MaxHealth, 1f, 1f);
        }
    }
}