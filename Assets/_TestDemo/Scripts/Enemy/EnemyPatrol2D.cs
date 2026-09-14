using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyPatrol2D : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 2f;
    private bool movingRight = true;

    [Header("Detection Settings")]
    [SerializeField] private Transform groundCheck; // Added Ground Check
    [SerializeField] private Transform edgeCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float checkDistance = 0.5f;
    [SerializeField] private float groundCheckRadius = 0.2f; // Size of the ground check circle
    [SerializeField] private LayerMask groundLayer;

    private Rigidbody2D rb;
    private bool isGrounded;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // 1. Check if the enemy is touching the ground
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 2. Only check for edges and walls IF we are on the ground
        if (isGrounded)
        {
            RaycastHit2D edgeInfo = Physics2D.Raycast(edgeCheck.position, Vector2.down, checkDistance, groundLayer);
            RaycastHit2D wallInfo = Physics2D.Raycast(wallCheck.position, transform.right, checkDistance, groundLayer);

            if (edgeInfo.collider == false || wallInfo.collider == true)
            {
                Flip();
            }
        }
    }

    private void FixedUpdate()
    {
        // 3. Only apply forced horizontal movement if grounded. 
        // If falling, let Unity's gravity and physics take over entirely.
        if (isGrounded)
        {
            float moveSpeed = movingRight ? speed : -speed;
            rb.linearVelocity = new Vector2(moveSpeed, rb.linearVelocity.y);
        }
    }

    private void Flip()
    {
        movingRight = !movingRight;
        transform.Rotate(0f, 180f, 0f);
    }

    private void OnDrawGizmos()
    {
        // Draw the ground check circle so you can see it in the editor
        if (groundCheck != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }

        if (edgeCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(edgeCheck.position, edgeCheck.position + Vector3.down * checkDistance);
        }
        
        if (wallCheck != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(wallCheck.position, wallCheck.position + transform.right * checkDistance);
        }
    }
}