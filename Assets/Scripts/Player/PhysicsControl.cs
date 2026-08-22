using UnityEngine;

namespace Player
{
    /* PhysicsControl: Centralized physics checks and helper methods for the player.
 - Awake(): cache Rigidbody2D.
 - Start(): store default gravity and coyote timer setup.
 - GetGravity(): returns stored gravity value.
 - OnDrawGizmos(): draw debug rays for ceiling checks.
 - DisableGravity()/EnableGravity(): toggle gravityScale.
 - ResetVelocity(): zero Rigidbody velocity.
 - IsGrounded(): raycast down to detect ground.
 - IsTouchingWall(): raycasts to detect wall contacts.
 - IsCeilingDetected(): raycasts upwards to detect ceiling.
 - Update(): update coyote timer when airborne.
 - FixedUpdate(): update grounded/wall/ceiling booleans each physics step.
 - StandCollider()/CrouchCollider(): toggle colliders for standing/crouching.
*/
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

        [Header("Colliders")]
        [SerializeField] private Collider2D standCollider;
        [SerializeField] private Collider2D crouchCollider;

        [Header("Ceiling Check")]
        [SerializeField] float ceilingRaycastDistance = 0.5f;
        [SerializeField] Transform ceilingCheckRight;
        [SerializeField] Transform ceilingCheckLeft;
        public bool ceilingDetected;
        private RaycastHit2D ceilingHitRight;
        private RaycastHit2D ceilingHitLeft;

        [Header("Coyote Time")]
        [SerializeField] private float coyoteSetTime;
        public float coyoteTimer;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            gravityValue = rb.gravityScale;
            coyoteTimer = coyoteSetTime;
        }
        
        public void SetInterpolationMode(RigidbodyInterpolation2D mode)
        {
            rb.interpolation = mode;
        }

        public float GetGravity()
        {
            return gravityValue;
        }

        private void OnDrawGizmos()
        {
            Debug.DrawRay(ceilingCheckRight.position, new Vector3(0, ceilingRaycastDistance, 0));
            Debug.DrawRay(ceilingCheckLeft.position, new Vector3(0, ceilingRaycastDistance, 0));
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

        private bool IsCeilingDetected()
        {
            ceilingHitRight = Physics2D.Raycast(ceilingCheckRight.position, Vector2.up, ceilingRaycastDistance, groundLayer);
            ceilingHitLeft = Physics2D.Raycast(ceilingCheckLeft.position, Vector2.up, ceilingRaycastDistance, groundLayer);
            return ceilingHitRight.collider != null || ceilingHitLeft.collider != null;
        }

        void Update()
        {
            if (!isGrounded)
            {
                coyoteTimer -= Time.deltaTime;
            }
            else
            {
                coyoteTimer = coyoteSetTime;
            }
        }

        private void FixedUpdate()
        {
            isGrounded = IsGrounded();
            isTouchingWall = IsTouchingWall();
            ceilingDetected = IsCeilingDetected();
        }

        public void StandCollider()
        {
            standCollider.enabled = true;
            crouchCollider.enabled = false;
        }
        public void CrouchCollider()
        {
            standCollider.enabled = false;
            crouchCollider.enabled = true;
        }
    }
}
