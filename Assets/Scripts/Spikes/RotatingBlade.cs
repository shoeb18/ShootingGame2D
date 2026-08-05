using System;
using Player;
using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    [SerializeField] private float rotateSpeed;
    [SerializeField] private float damageAmount;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;
    
    
    private void Update()
    {
        transform.Rotate(0, 0, rotateSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        KnockBackAbility knockBackAbility = collision.GetComponentInParent<KnockBackAbility>();
        knockBackAbility.StartKnockBack(knockBackDuration, knockBackForce, transform);

        PlayerStats playerStats = collision.GetComponent<PlayerStats>();

        if (playerStats != null)
        {
            playerStats.DamagePlayer(damageAmount);
        }
    }
}
