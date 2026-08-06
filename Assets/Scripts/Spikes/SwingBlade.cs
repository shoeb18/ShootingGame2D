using Player;
using UnityEngine;

public class SwingBlade : MonoBehaviour
{
    [Header("Swing Blade Settings")] 
    [SerializeField] private float maxAngle;
    [SerializeField] private float speed;
    private float timer;
    
    [Header("Knockback Settings")]
    [SerializeField] private float damageAmount;
    [SerializeField] private float knockBackDuration;
    [SerializeField] private Vector2 knockBackForce;

    private int pushDirection = 1;
    private float previousAngle = 0f;

    private void Update()
    {
        timer += Time.deltaTime * speed;
        float angle = maxAngle * Mathf.Sin(timer);
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (angle > previousAngle)
        {
            pushDirection = 1;
        }
        else if (angle < previousAngle)
        {
            pushDirection = -1;
        }
        
        previousAngle = angle;
    }
    
    void OnTriggerEnter2D(Collider2D collision)
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
