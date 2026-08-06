using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class CrouchAbility : BaseAbility
    {
        public InputActionReference crouchActionRef;
        [SerializeField] private float crouchSpeed = 2f;
        private string crouchAnimParameterName = "Crouch";
        private int crouchAnimHash;

        private string xSpeedParameterName = "xSpeed";
        private int xSpeedHash;
        private bool wantToStop;

        public override void Initialization()
        {
            base.Initialization();
            crouchAnimHash = Animator.StringToHash(crouchAnimParameterName);
            xSpeedHash = Animator.StringToHash(xSpeedParameterName);
        }

        void OnEnable()
        {
            crouchActionRef.action.performed += TryToCrouch;
            crouchActionRef.action.canceled += StopCrouch;
        }
        void OnDisable()
        {
            crouchActionRef.action.performed -= TryToCrouch;
            crouchActionRef.action.canceled -= StopCrouch;
        }
        public override void EnterAbility()
        {
            linkedPhysicsControl.CrouchCollider();
            player.playerStats.EnableCrouchingCollider();
        }
        private void TryToCrouch(InputAction.CallbackContext context)
        {
            if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            {
                return;
            }
            if (linkedPhysicsControl.isGrounded == false || linkedStateMachine.currentState == PlayerStates.State.Dash
                                                         || linkedStateMachine.currentState == PlayerStates.State.LadderClimb)
            {
                return;
            }
            wantToStop = false;
            linkedStateMachine.ChangeState(PlayerStates.State.Crouch);
        }

        public override void ExitAbility()
        {
            linkedPhysicsControl.StandCollider();
            wantToStop = false;
            player.playerStats.EnableStandingCollider();
        }

        public override void FixedUpdateAbility()
        {
            if (linkedPhysicsControl.isGrounded)
            {
                linkedPhysicsControl.rb.linearVelocity = new Vector2(linkedPlayerInputs.horizontalInput * crouchSpeed, linkedPhysicsControl.rb.linearVelocityY);
            }
        }

        private void StopCrouch(InputAction.CallbackContext context)
        {
            if (!isPermitted)
            {
                return;
            }

            if (linkedStateMachine.currentState != PlayerStates.State.Crouch)
            {
                return;
            }

            if (linkedPhysicsControl.ceilingDetected)
            {
                wantToStop = true;
                return;
            }

            if (linkedPlayerInputs.horizontalInput == 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
            else if (linkedPlayerInputs.horizontalInput != 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            }
        }

        public override void UpdateAbility()
        {
            player.FlipPlayer();

            if (wantToStop && linkedPhysicsControl.ceilingDetected == false)
            {
                if (linkedPlayerInputs.horizontalInput == 0)
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Idle);
                }
                else if (linkedPlayerInputs.horizontalInput != 0)
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Run);
                }
            }

            if (linkedPhysicsControl.isGrounded == false)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
            }
        }

        public override void UpdateAnimator()
        {
            linkedAnimator.SetBool(crouchAnimHash, linkedStateMachine.currentState == PlayerStates.State.Crouch);
            linkedAnimator.SetFloat(xSpeedHash, Mathf.Abs(linkedPhysicsControl.rb.linearVelocityX));
        }
    }
}
