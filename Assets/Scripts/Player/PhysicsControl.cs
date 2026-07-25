using UnityEngine;

public class PhysicsControl : MonoBehaviour
{
    public Rigidbody2D rb;

    [Header("Ground Check")]
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private float groundRaycastDistance = 0.2f;
    [SerializeField]
    private LayerMask groundLayer;
    public bool isGrounded;
    private RaycastHit2D groundHit;

    private float gravityValue;

    [Header("Wall Check")]
    [SerializeField] float wallRaycastDistance = 0.5f;
    [SerializeField] Transform wallCheckUpper;
    [SerializeField] Transform wallCheckLower;
    public bool isTouchingWall;
    private RaycastHit2D wallHitUpper;
    private RaycastHit2D wallHitLower;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        gravityValue = rb.gravityScale;
    }

    public void DisableGravity()
    {
        rb.gravityScale = 0f;
    }

    public void EnableGravity()
    {
        rb.gravityScale = gravityValue;
    }

    public void ResetVelocity()
    {
        rb.linearVelocity = Vector2.zero;
    }

    private bool IsGrounded()
    {
        groundHit = Physics2D.Raycast(groundCheck.position, Vector2.down, groundRaycastDistance, groundLayer);
        // debug
        Debug.DrawRay(groundCheck.position, Vector2.down * groundRaycastDistance, Color.red);
        return groundHit.collider != null;
    }

    private bool IsTouchingWall()
    {
        wallHitUpper = Physics2D.Raycast(wallCheckUpper.position, transform.right, wallRaycastDistance, groundLayer);
        wallHitLower = Physics2D.Raycast(wallCheckLower.position, transform.right, wallRaycastDistance, groundLayer);
        // debug
        Debug.DrawRay(wallCheckUpper.position, transform.right * wallRaycastDistance, Color.blue);
        Debug.DrawRay(wallCheckLower.position, transform.right * wallRaycastDistance, Color.blue);
        return wallHitUpper.collider != null || wallHitLower.collider != null;
    }

    private void FixedUpdate()
    {
        isGrounded = IsGrounded();
        isTouchingWall = IsTouchingWall();
    }
}
