using UnityEngine;
using System.Collections.Generic;

public class MeleeHitbox : MonoBehaviour
{
    public int damage = 40;
    
    // This acts as our "already hit" list for the current swing
    private HashSet<Collider2D> hitEnemies = new HashSet<Collider2D>();

    void OnEnable()
    {
        // Wipe the memory clean every time the hitbox is turned on
        hitEnemies.Clear();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If they aren't in the list, damage them and add them to the list
        if (!hitEnemies.Contains(other))
        {
            hitEnemies.Add(other);
            Debug.Log("Hitbox sliced through " + other.name);
            
            // Apply damage here:
            EnemyStats enemyStats = other.GetComponentInChildren<EnemyStats>();
            if (enemyStats != null)
            {
                enemyStats.TakeDamage(damage);
            }

            // Note: If checking for a downward strike for a pogo jump, 
            // this is exactly where you'd fire off the bounce logic.
        }
    }
}