using System;
using UnityEngine;

public class EnemySimpleStateMachine : MonoBehaviour
{
    [SerializeField] protected Animator _animator;
    
    protected EnemyState currentState;
    protected EnemyState previousState;
    public bool facingRight = true;
    public enum EnemyState
    {
        Idle,
        Move,
        Chase,
        Attack,
        Death
    }
    
    // Change enemy state
    public void ChangeState(EnemyState newState)
    {
        if (currentState == newState)
            return;
        ExitState(currentState);
        previousState = currentState;
        currentState = newState;
        EnterState(currentState);
    }

    // update and fixed update methods
    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                UpdateIdleState();
                break;
            case EnemyState.Move:
                UpdateMoveState();
                break;
            case EnemyState.Chase:
                UpdateChaseState();
                break;
            case EnemyState.Attack:
                UpdateAttackState();
                break;
            case EnemyState.Death:
                UpdateDeathState();
                break;
        }
    }

    private void FixedUpdate()
    {
        switch (currentState)
        {
            case EnemyState.Idle:
                FixedUpdateIdleState();
                break;
            case EnemyState.Move:
                FixedUpdateMoveState();
                break;
            case EnemyState.Chase:
                FixedUpdateChaseState();
                break;
            case EnemyState.Attack:
                FixedUpdateAttackState();
                break;
            case EnemyState.Death:
                FixedUpdateDeathState();
                break;
        }
    }

    // Update state methods
    public virtual void UpdateIdleState()
    {
        
    }
    
    public virtual void UpdateMoveState()
    {
        
    }
    
    public virtual void UpdateChaseState()
    {
        
    }
    
    public virtual void UpdateAttackState()
    {
        
    }
    
    public virtual void UpdateDeathState()
    {
        
    }
    
    // FixedUpdate state methods
    public virtual void FixedUpdateIdleState()
    {
        
    }

    public virtual void FixedUpdateMoveState()
    {
        
    }

    public virtual void FixedUpdateChaseState()
    {
        
    }

    public virtual void FixedUpdateAttackState()
    {
        
    }

    public virtual void FixedUpdateDeathState()
    {
        
    }

    // Enter and exit state methods
    protected void EnterState(EnemyState state)
    {
        switch(state)
        {
            case EnemyState.Idle:
                EnterIdleState();
                break;
            case EnemyState.Move:
                EnterMoveState();
                break;
            case EnemyState.Chase:
                EnterChaseState();
                break;
            case EnemyState.Attack:
                EnterAttackState();
                break;
            case EnemyState.Death:
                EnterDeathState();
                break;
        }
    }
    
    protected void ExitState(EnemyState state)
    {
        switch(state)
        {
            case EnemyState.Idle:
                ExitIdleState();
                break;
            case EnemyState.Move:
                ExitMoveState();
                break;
            case EnemyState.Chase:
                ExitChaseState();
                break;
            case EnemyState.Attack:
                ExitAttackState();
                break;
            case EnemyState.Death:
                ExitDeathState();
                break;
        }
    }
    
    public virtual void EnterIdleState()
    {
        
    }
    
    public virtual void EnterMoveState()
    {
        
    }
    
    public virtual void EnterChaseState()
    {
        
    }
    
    public virtual void EnterAttackState()
    {
        
    }
    
    public virtual void EnterDeathState()
    {
        
    }
    
    public virtual void ExitIdleState()
    {
        
    }
    
    public virtual void ExitMoveState()
    {
        
    }
    
    public virtual void ExitChaseState()
    {
        
    }
    
    public virtual void ExitAttackState()
    {
        
    }
    
    public virtual void ExitDeathState()
    {
        
    }
    
    public void ForceFlip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
    }
}
