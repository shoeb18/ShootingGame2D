using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class DashAbility : BaseAbility
    {
        public InputActionReference dashActionRef;
        [SerializeField] private float dashForce = 10f;
        [SerializeField] private float maxDashDuration;
        private float dashTimer;
        private string DashParameterName = "Dash";
        private int DashParamHash;

        public override void Initialization()
        {
            base.Initialization();
            DashParamHash = Animator.StringToHash(DashParameterName);
        }

        void OnEnable()
        {
            dashActionRef.action.performed += TryDash;
        }

        void OnDisable()
        {
            dashActionRef.action.performed -= TryDash;
        }

        public override void EnterAbility()
        {
            playerCharacter.playerStats.DisableDamage();// enables I-frames for dash
        }

        public override void ExitAbility()
        {
            linkedPhysicsControl.EnableGravity();
            linkedPhysicsControl.ResetVelocity();
            playerCharacter.playerStats.EnableDamage(); // disables I-frames for dash
        }

        private void TryDash(InputAction.CallbackContext value)
        {
            if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            {
                return;
            }

            if (linkedStateMachine.currentState == PlayerStates.State.Dash || linkedPhysicsControl.isTouchingWall || linkedStateMachine.currentState == PlayerStates.State.Crouch)
            {
                return;
            }

            linkedStateMachine.ChangeState(PlayerStates.State.Dash);
            linkedPhysicsControl.DisableGravity();
            linkedPhysicsControl.ResetVelocity();

            if (playerCharacter.facingRight)
            {
                linkedPhysicsControl.rb.linearVelocityX = dashForce;
            }
            else
            {
                linkedPhysicsControl.rb.linearVelocityX = -dashForce;
            }

            dashTimer = maxDashDuration;
        }

        public override void UpdateAbility()
        {
            dashTimer -= Time.deltaTime;

            if (linkedPhysicsControl.isTouchingWall)
            {
                dashTimer = -1;
            }

            if (dashTimer <= 0)
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

        public override void UpdateAnimator()
        {
            linkedAnimator.SetBool(DashParamHash, linkedStateMachine.currentState == PlayerStates.State.Dash);
        }
    }
}
