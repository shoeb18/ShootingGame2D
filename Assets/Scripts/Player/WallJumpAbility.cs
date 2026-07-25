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
        if (!isPermitted)
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

        if (wallJumpMinimumTime <= 0f && linkedPhysicsControl.isTouchingWall)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            wallJumpTimer = -1;
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
        }
    }
}
