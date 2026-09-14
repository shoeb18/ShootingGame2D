using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyingEnemyAI : MonoBehaviour
{
    // Define the states our enemy can be in
    private enum State { Patrol, Chase, Attack }
    private State currentState;

    [Header("Movement Stats")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 4f;

    [Header("Patrol Settings")]
    [SerializeField] private Transform[] waypoints;
    private int currentWaypointIndex = 0;

    [Header("Targeting & Ranges")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float attackRadius = 1.5f;
    
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int attackDamage = 10;
    private float nextAttackTime = 0f;

    private Transform player;
    private Animator anim;

    private void Start()
    {
        currentState = State.Patrol;
        
        // Find the player automatically using the "Player" tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
        
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (player == null) return;

        // The Brain of the State Machine
        switch (currentState)
        {
            case State.Patrol:
                PatrolBehavior();
                break;
            case State.Chase:
                ChaseBehavior();
                break;
            case State.Attack:
                AttackBehavior();
                break;
        }
    }

    private void PatrolBehavior()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[currentWaypointIndex];
        
        // Move towards the current waypoint
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, patrolSpeed * Time.deltaTime);
        FlipSprite(targetWaypoint.position);

        // If we reach the waypoint, pick the next one
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }

        // Transition to CHASE if player gets too close
        if (Vector2.Distance(transform.position, player.position) <= detectionRadius)
        {
            currentState = State.Chase;
        }
    }

    private void ChaseBehavior()
    {
        // Move towards the player
        transform.position = Vector2.MoveTowards(transform.position, player.position, chaseSpeed * Time.deltaTime);
        FlipSprite(player.position);

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Transition to ATTACK if close enough
        if (distanceToPlayer <= attackRadius)
        {
            currentState = State.Attack;
        }
        // Transition back to PATROL if player escapes the detection radius
        else if (distanceToPlayer > detectionRadius)
        {
            currentState = State.Patrol;
        }
    }

    private void AttackBehavior()
    {
        // Check if we are ready to attack again based on the cooldown timer
        if (Time.time >= nextAttackTime)
        {
            Debug.Log("Enemy Attacks Player for " + attackDamage + " damage!");
            anim.SetTrigger("Attack");
            
            // TODO: Here is where you would grab the Player's Health script and apply damage.
            // IDamageable playerHealth = player.GetComponent<IDamageable>();
            // playerHealth?.TakeDamage(attackDamage);

            nextAttackTime = Time.time + attackCooldown;
        }

        // Transition back to CHASE immediately after attacking (or if player backs away)
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer > attackRadius)
        {
            currentState = State.Chase;
        }
    }

    // Flips the enemy to face the target it is moving towards
    private void FlipSprite(Vector3 targetPosition)
    {
        if (targetPosition.x > transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Face Right
        }
        else if (targetPosition.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Face Left
        }
    }

    // Draws helpful visual rings in the Unity Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }
}