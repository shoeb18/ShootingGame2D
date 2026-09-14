using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float damageAmount;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<Player.PlayerStats>().DamagePlayer(damageAmount);
    }
}
