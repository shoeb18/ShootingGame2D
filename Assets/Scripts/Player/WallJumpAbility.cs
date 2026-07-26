using UnityEngine;
using UnityEngine.InputSystem;

public class WallJumpAbility : BaseAbility
{
    public InputActionReference jumpInputAction;
    [SerializeField] private Vector2 wallJumpForce = new Vector2(10f, 15f);
    [SerializeField] private float wallJumpMaxTime = 0.2f;
    private float wallJumpMinimumTime = 0.1f;
    private float wallJumpTimer;

    void OnEnable()
    {
        jumpInputAction.action.performed += TryWallJump;
    }

    void OnDisable()
    {
        jumpInputAction.action.performed -= TryWallJump;
    }

    public override void Initialization()
    {
        base.Initialization();
        wallJumpTimer = wallJumpMaxTime;
    }

    private void TryWallJump(InputAction.CallbackContext context)
    {
        if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
        {
            return;
        }

        if (EvaluateWallJumpConditions())
        {
            linkedStateMachine.ChangeState(PlayerStates.State.WallJump);
            wallJumpTimer = wallJumpMaxTime;
            wallJumpMinimumTime = 0.15f;

            // flip the player and apply the wall jump force
            player.ForceFlipPlayer();

            // temp code for lower gravity for wall jump
            linkedPhysicsControl.rb.gravityScale = linkedPhysicsControl.GetGravity() / 2.5f;

            if (player.facingRight)
            {
                linkedPhysicsControl.rb.linearVelocity = new Vector2(wallJumpForce.x, wallJumpForce.y);
            }
            else
            {
                linkedPhysicsControl.rb.linearVelocity = new Vector2(-wallJumpForce.x, wallJumpForce.y);
            }
        }
    }

    public override void ExitAbility()
    {
        linkedPhysicsControl.rb.gravityScale = linkedPhysicsControl.GetGravity();
    }

    private bool EvaluateWallJumpConditions()
    {
        if (linkedPhysicsControl.isGrounded || !linkedPhysicsControl.isTouchingWall)
        {
            return false;
        }
        else
        {
            return true;
        }
    }   

    override public void UpdateAbility()
    {
        wallJumpTimer -= Time.deltaTime;
        wallJumpMinimumTime -= Time.deltaTime;

        if (wallJumpMinimumTime < 0 && linkedPhysicsControl.isGrounded)
        {
            if (linkedPlayerInputs.horizontalInput != 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
            return;
        }

        if (wallJumpTimer <= 0f)
        {
            if (linkedPhysicsControl.isGrounded)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            }
            return;
        }

        if (wallJumpMinimumTime <= 0f && linkedPhysicsControl.isTouchingWall)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.WallSlide);
            wallJumpTimer = -1;
            return;
        }
        
    }
}
