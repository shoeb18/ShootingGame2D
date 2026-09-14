using System;
using UnityEngine;

public class PatrolPhysics : MonoBehaviour
{
    public Rigidbody2D rb;
    
    [Header("Ground and Wall Check")] 
    [SerializeField] private float _checkRadius;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private Transform _wallCheck;
    [SerializeField] private LayerMask _groundLayer;
    
    [Header("Colliders")]
    [SerializeField] private Collider2D _attackDetectionCollider;
    [SerializeField] private Collider2D _attackCollider;
    [SerializeField] private Collider2D _statCollider;

    public bool inAttackRange;
    public bool _isGrounded;
    public bool _isTouchingWall;

    private void FixedUpdate()
    {
        _isGrounded = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, _groundLayer);
        _isTouchingWall = Physics2D.OverlapCircle(_wallCheck.position, _checkRadius, _groundLayer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheck.position, _checkRadius);
        Gizmos.DrawWireSphere(_wallCheck.position, _checkRadius);
    }

    public void NegateForces()
    {
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }
    
    public void ActivateAttackCollider()
    {
        _attackCollider.enabled = true;
    }
    public void DeactivateAttackCollider()
    {
        _attackCollider.enabled = false;
    }
    
    public void DeathColliderDeactivation()
    {
        DeactivateAttackCollider();
        _attackDetectionCollider.enabled = false;
        _statCollider.enabled = false;
    }
}
