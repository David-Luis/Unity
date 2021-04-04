using System.Collections;
using Systems.Health;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Tests.EditMode
{
    public class HealthSystemTests
    {
        // A Test behaves as an ordinary method
        [Test]
        public void MaxHealth_AfterCreation_NotChanged()
        {
            var healthComponent = new HealthSystem(100);

            Assert.AreEqual(healthComponent.MaxHealth, 100);
        }
        
        [Test]
        public void MaxHealth_AfterDamage_NotChanged()
        {
            var healthComponent = new HealthSystem(100);
            healthComponent.Damage(50);
            Assert.AreEqual(healthComponent.MaxHealth, 100);
        }
        
        [Test]
        public void MaxHealth_AfterHeal_NotChanged()
        {
            var healthComponent = new HealthSystem(100);
            healthComponent.Heal(50);
            Assert.AreEqual(healthComponent.MaxHealth, 100);
        }
        
        [Test]
        public void Health_AfterCreation_NotChanged()
        {
            var healthComponent = new HealthSystem(100);
            Assert.AreEqual(healthComponent.Health, 100);
        }
        
        [TestCase(100, 0, 100)]
        [TestCase(100, 100, 0)]
        [TestCase(100, 50, 50)]
        [TestCase(0, 50, 0)]
        public void Health_AfterDamage_IsCorrect(int maxHealth, int damage, int expected)
        {
            var healthComponent = new HealthSystem(maxHealth);  
            healthComponent.Damage(damage);
            Assert.AreEqual(healthComponent.Health, expected);
        }
        
        [TestCase(100, 100, 0, 100)]
        [TestCase(100, 0, 100, 100)]
        [TestCase(100, 0, 50, 50)]
        [TestCase(0, 0, 100, 0)]
        public void Health_AfterHeal_IsCorrect(int maxHealth, int currentHealth, int heal, int expected)
        {
            var healthComponent = new HealthSystem(maxHealth);  
            healthComponent.Damage(maxHealth-currentHealth);
            healthComponent.Heal(heal);
            Assert.AreEqual(healthComponent.Health, expected);
        }
    }
}
