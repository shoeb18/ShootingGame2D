using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class LadderAbility : BaseAbility
    {
        public InputActionReference ladderActionRef;
        [SerializeField] private float climbSpeed = 10f;
        [SerializeField] private float setMinLadderTime;
        private float minimumLadderTime;
        private bool climb;
        public bool canClimbLadder;
        private string LadderAnimParameterName = "Ladder";
        private int LadderAnimHash;

        void OnEnable()
        {
            ladderActionRef.action.performed += TryToClimb;
            ladderActionRef.action.canceled += StopClimb;
        }

        void OnDisable()
        {
            ladderActionRef.action.performed -= TryToClimb;
            ladderActionRef.action.canceled -= StopClimb;
        }

        public override void Initialization()
        {
            base.Initialization();
            LadderAnimHash = Animator.StringToHash(LadderAnimParameterName);
            minimumLadderTime = setMinLadderTime;
        }

        private void TryToClimb(InputAction.CallbackContext context)
        {
            if (!isPermitted || linkedStateMachine.currentState == PlayerStates.State.KnockBack)
            {
                return;
            }

            linkedAnimator.enabled = true;

            if (!canClimbLadder || linkedStateMachine.currentState == PlayerStates.State.LadderClimb || linkedStateMachine.currentState == PlayerStates.State.Dash)
            {
                return;
            }

            linkedStateMachine.ChangeState(PlayerStates.State.LadderClimb);
            linkedPhysicsControl.DisableGravity();
            linkedPhysicsControl.ResetVelocity();

            climb = true;

            minimumLadderTime = setMinLadderTime;
        }

        private void StopClimb(InputAction.CallbackContext context)
        {
            if (!isPermitted)
            {
                return;
            }
            if (linkedStateMachine.currentState != PlayerStates.State.LadderClimb)
            {
                return;
            }
            linkedPhysicsControl.ResetVelocity();
            linkedAnimator.enabled = false;
        }

        public override void FixedUpdateAbility()
        {
            if (climb)
            {
                linkedPhysicsControl.rb.linearVelocity = new Vector2(0, linkedPlayerInputs.verticalInput * climbSpeed);
            }
        }

        public override void ExitAbility()
        {
            linkedPhysicsControl.EnableGravity();
            climb = false;
            linkedAnimator.enabled = true;
        }
        public override void UpdateAbility()
        {
            if (climb)
            {
                minimumLadderTime -= Time.deltaTime;
            }

            if (linkedPlayerInputs.horizontalInput != 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                return;
            }

            if (canClimbLadder == false)
            {
                if (linkedPhysicsControl.isGrounded == false)
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                }
            }
            if (linkedPhysicsControl.isGrounded && minimumLadderTime <= 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
            }
        }

        public override void UpdateAnimator()
        {
            linkedAnimator.SetBool(LadderAnimHash,linkedStateMachine.currentState == PlayerStates.State.LadderClimb);
        }
    }
}
