using Player;
using UnityEngine;

/* Spikes: Static spike hazard that damages player and triggers knockback on contact.
 - OnTriggerEnter2D(Collider2D): apply knockback and damage to colliding player.
*/
public class Spikes : MonoBehaviour
{
    [SerializeField] private float spikeDamage;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAbility knockBackAbility = collision.GetComponentInParent<KnockBackAbility>();
        knockBackAbility.StartKnockBack(knockBackDuration, knockBackForce, transform);

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.DamagePlayer(spikeDamage);
        }
    }
}
