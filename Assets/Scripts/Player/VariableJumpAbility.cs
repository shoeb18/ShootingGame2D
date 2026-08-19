using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /* VariableJumpAbility: Variable-height jump implementation (hold to extend), with timers and gravity tweaks.
 - Initialization(): cache animator hashes and initial minimumAirTime.
 - OnEnable()/OnDisable(): subscribe/unsubscribe jump input.
 - TryToJump(): handle ladder and coyote-based jumps and start timed jump state.
 - StopJump(): cancel ongoing timed jump.
 - UpdateAbility(): manage timers and transitions to Run/Idle/WallSlide.
 - FixedUpdateAbility(): apply variable vertical control, clamp velocity and reduce gravity while falling.
 - UpdateAnimator(): update jump and vertical speed parameters.
 - ExitAbility(): restore gravity when leaving jump state.
*/
public class VariableJumpAbility : BaseAbility
    {
        public InputActionReference jumpAction;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float airSpeed = 2f;
        [SerializeField] private float minimumAirTime = 0.2f;

        [SerializeField] private float setMaxJumpTime;
        private float jumpTimer;
        private bool jumping;

        [SerializeField] private float gravityDivider;

        private float startMinimumAirTime;

        private string jumpAnimParameterName = "Jump";
        private string ySpeedParameterName = "ySpeed";
        private int jumpParamHash;
        private int ySpeedParamHash;

        public override void Initialization()
        {
            base.Initialization();
            startMinimumAirTime = minimumAirTime; // Store the initial value of minimumAirTime
            jumpParamHash = Animator.StringToHash(jumpAnimParameterName);
            ySpeedParamHash = Animator.StringToHash(ySpeedParameterName);
        }

        private void OnEnable()
        {
            jumpAction.action.performed += TryToJump;
            jumpAction.action.canceled += StopJump;
        }

        private void OnDisable()
        {
            jumpAction.action.performed -= TryToJump;
            jumpAction.action.canceled -= StopJump;
        }

        override public void UpdateAbility()
        {
            if (minimumAirTime > 0)
            {
                minimumAirTime -= Time.deltaTime;
            }

            if (jumping)
            {
                jumpTimer -= Time.deltaTime;
                if (jumpTimer <= 0)
                {
                    jumping = false;
                }
            }

            if (linkedPhysicsControl.isGrounded && minimumAirTime <= 0)
            {
                if (linkedPlayerInputs.horizontalInput != 0)
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Run);
                }
                else
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Idle);
                }
            }

            if (!linkedPhysicsControl.isGrounded && linkedPhysicsControl.isTouchingWall)
            {
                if (linkedPhysicsControl.rb.linearVelocityY < 0)
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.WallSlide);
                }
            }

        }

        override public void UpdateAnimator()
        {
            linkedAnimator.SetBool(jumpParamHash, linkedStateMachine.currentState == PlayerStates.State.Jump || linkedStateMachine.currentState == PlayerStates.State.WallJump);
            linkedAnimator.SetFloat(ySpeedParamHash, linkedPhysicsControl.rb.linearVelocity.y);
        }

        override public void FixedUpdateAbility()
        {
            if (!linkedPhysicsControl.isGrounded)
            {
                if (jumping)
                {
                    linkedPhysicsControl.rb.linearVelocity = new Vector2(airSpeed * linkedPlayerInputs.horizontalInput, jumpForce);
                }
                else
                {
                    linkedPhysicsControl.rb.linearVelocity = new Vector2(airSpeed * linkedPlayerInputs.horizontalInput, Mathf.Clamp(linkedPhysicsControl.rb.linearVelocityY, -10, jumpForce));
                }
            }
            if (linkedPhysicsControl.rb.linearVelocityY < 0)
            {
                linkedPhysicsControl.rb.gravityScale = linkedPhysicsControl.GetGravity() / gravityDivider;
            }
        }

        private void TryToJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
                    return;

                if (linkedStateMachine.currentState == PlayerStates.State.LadderClimb)
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                    linkedPhysicsControl.rb.linearVelocity = new Vector2(airSpeed * linkedPlayerInputs.horizontalInput, 0);
                    minimumAirTime = startMinimumAirTime; // Reset minimumAirTime when jumping

                    jumping = true;
                    jumpTimer = setMaxJumpTime;
                    return;
                }
                if (linkedPhysicsControl.coyoteTimer > 0)
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                    linkedPhysicsControl.rb.linearVelocity = new Vector2(airSpeed * linkedPlayerInputs.horizontalInput, jumpForce);
                    minimumAirTime = startMinimumAirTime; // Reset minimumAirTime when jumping
                    linkedPhysicsControl.coyoteTimer = -1;

                    jumping = true;
                    jumpTimer = setMaxJumpTime;
                }

                /*
            if (linkedPhysicsControl.isGrounded)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                linkedPhysicsControl.rb.linearVelocity = new Vector2(airSpeed * linkedPlayerInputs.horizontalInput, jumpForce);
                minimumAirTime = startMinimumAirTime; // Reset minimumAirTime when jumping
            }
            */
            }
        }

        public override void ExitAbility()
        {
            linkedPhysicsControl.EnableGravity();
        }
        private void StopJump(InputAction.CallbackContext context)
        {
            jumping = false;
        }

    }
}
