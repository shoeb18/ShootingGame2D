using System;
using UnityEngine;
using Player;


public class KillZone : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerStats playerStats = other.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            playerStats.DamagePlayer(damageAmount);
        }
    }
}
