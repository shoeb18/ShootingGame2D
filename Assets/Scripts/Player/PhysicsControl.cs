using UnityEngine;

public class PhysicsControl : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Ground Check")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundCheckRadius = 0.2f;
    [SerializeField]
    private LayerMask groundLayer;
    public bool isGrounded;
    private RaycastHit2D groundHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private bool IsGrounded()
    {
        groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckRadius, groundLayer);
        // debug
        Debug.DrawRay(groundCheck.position, Vector2.down * groundCheckRadius, Color.red);
        return groundHit.collider != null;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
    }
}
