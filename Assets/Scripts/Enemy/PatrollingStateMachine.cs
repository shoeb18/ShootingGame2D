using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class PatrollingStateMachine : EnemySimpleStateMachine
{
    [SerializeField] private PatrolPhysics _patrolPhysics;

    [Header("Idle State")] 
    [SerializeField] private bool _canIdle = false;
    [SerializeField] private string _idleAnimationName;
    [SerializeField] private float _minIdleTime;
    [SerializeField] private float _maxIdleTime;
    private float _idleStateTimer;
    
    [Header("Move State")]
    [SerializeField] private string _moveAnimationName;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _minMoveTime;
    [SerializeField] private float _maxMoveTime;
    [SerializeField] private float _minimumTurnDelay;
    private float _moveStateTimer;
    private float _turnCooldown;
    
    [Header("Attack State")]
    [SerializeField] private string _attackAnimationName;
    
    [Header("Death State")]
    [SerializeField] private string _deathAnimationName;

    #region Idle
    public override void EnterIdleState()
    {
        if (!_canIdle) return;
        _animator.Play(_idleAnimationName);
        _idleStateTimer = Random.Range(_minIdleTime, _maxIdleTime);
        _patrolPhysics.NegateForces();
    }

    public override void UpdateIdleState()
    {
        _idleStateTimer -= Time.deltaTime;
        if (_idleStateTimer <= 0)
        {
            ChangeState(EnemyState.Move);
        }
        
        if (_patrolPhysics.inAttackRange)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    public override void ExitIdleState()
    {
        // do something when exiting idle state
    }
    #endregion

    #region Move

    public override void EnterMoveState()
    {
        _animator.Play(_moveAnimationName);
        _moveStateTimer = Random.Range(_minMoveTime, _maxMoveTime);
    }

    public override void UpdateMoveState()
    {
        _moveStateTimer -= Time.deltaTime;
        if (_moveStateTimer <= 0)
        {
            ChangeState(EnemyState.Idle);
        }
        
        if (_turnCooldown > 0)
        {
            _turnCooldown -= Time.deltaTime;
        }
        
        if (_patrolPhysics._isTouchingWall || !_patrolPhysics._isGrounded)
        {
            if (_turnCooldown > 0)
            {
                return;
            }
            ForceFlip();
            _moveSpeed *= -1;
            _turnCooldown = _minimumTurnDelay;
        }
        
        if (_patrolPhysics.inAttackRange)
        {
            ChangeState(EnemyState.Attack);
        }
    }

    public override void FixedUpdateMoveState()
    {
        _patrolPhysics.rb.linearVelocity = new Vector2(_moveSpeed, _patrolPhysics.rb.linearVelocityY);
    }

    #endregion

    #region Attack

    public override void EnterAttackState()
    {
        _animator.Play(_attackAnimationName);
        _patrolPhysics.NegateForces();
    }

    public void EndOfAttack()
    {
        if (_patrolPhysics.inAttackRange)
        {
            _animator.Play(_attackAnimationName, 0, 0);
        }
        else
        {
            ChangeState(previousState);
        }
    }

    #endregion

    #region Death
    
    public override void EnterDeathState()
    {
        _animator.Play(_deathAnimationName);
        _patrolPhysics.DeathColliderDeactivation();
        _patrolPhysics.NegateForces();
    }

    #endregion
}
