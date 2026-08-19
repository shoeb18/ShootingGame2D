using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /* JumpAbility: Single jump behavior and animator updates.
 - Initialization(): cache animator hashes and minimum air time.
 - OnEnable()/OnDisable(): subscribe/unsubscribe jump input.
 - TryToJump(): handle jump input; supports coyote time and ladder-jump case, sets vertical velocity and minimum air time.
 - StopJump(): hook for jump cancel (currently no variable jump logic here).
 - UpdateAbility(): manage minimumAirTime and transitions to Run/Idle/WallSlide.
 - FixedUpdateAbility(): apply horizontal control while airborne.
 - UpdateAnimator(): update jump flag and vertical speed parameter.
*/
public class JumpAbility : BaseAbility
    {
        public InputActionReference jumpAction;
        [SerializeField] private float jumpForce = 5f;
        [SerializeField] private float airSpeed = 2f;
        [SerializeField] private float minimumAirTime = 0.2f;

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
                linkedPhysicsControl.rb.linearVelocity = new Vector2(airSpeed * linkedPlayerInputs.horizontalInput, linkedPhysicsControl.rb.linearVelocity.y);
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
                    return;
                }
                if (linkedPhysicsControl.coyoteTimer > 0)
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                    linkedPhysicsControl.rb.linearVelocity = new Vector2(airSpeed * linkedPlayerInputs.horizontalInput, jumpForce);
                    minimumAirTime = startMinimumAirTime; // Reset minimumAirTime when jumping
                    linkedPhysicsControl.coyoteTimer = -1;
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

        private void StopJump(InputAction.CallbackContext context)
        {
            if (context.canceled)
            {
                // Debug.Log("Jump action canceled");
            }
        }

    }
}
