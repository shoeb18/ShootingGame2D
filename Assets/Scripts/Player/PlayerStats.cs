using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        [SerializeField] private float maxHealth;
        [SerializeField] private Player player;
        [SerializeField] private HealthBarControl healthBarControl;
        private float currentHealth;


        void Start()
        {
            currentHealth = maxHealth;
            healthBarControl.SetSliderValue(currentHealth, maxHealth);
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public void DamagePlayer(float damageValue)
        {
            currentHealth -= damageValue;
            healthBarControl.SetSliderValue(currentHealth, maxHealth);

            if (currentHealth <= 0)
            {
                print("Player dead!");
            }
        }
    }
}
