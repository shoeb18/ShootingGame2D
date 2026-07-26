using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private Player player;
    private float currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public void DamagePlayer(float damageValue)
    {
        currentHealth -= damageValue;

        if (currentHealth <= 0)
        {
            print("Player dead!");
        }
    }
}
