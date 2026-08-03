using UnityEngine;

namespace Player
{
    public class WallSlideAbility : BaseAbility
    {
        [SerializeField] private float maxSlideSpeed;
        private string wallSlideParameterName = "WallSlide";
        private int wallSlideParamHash;

        public override void Initialization()
        {
            base.Initialization();
            wallSlideParamHash = Animator.StringToHash(wallSlideParameterName);
        }

        public override void EnterAbility()
        {
            linkedPhysicsControl.rb.linearVelocity = Vector2.zero;
        }
        public override void UpdateAbility()
        {
            if (linkedPhysicsControl.isGrounded)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Idle);
                return;
            }
            if (player.facingRight && linkedPlayerInputs.horizontalInput < 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                linkedPhysicsControl.isTouchingWall = false;
                linkedAnimator.SetBool("WallSlide", false);
                return;
            }
            if (!player.facingRight && linkedPlayerInputs.horizontalInput > 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                linkedPhysicsControl.isTouchingWall = false;
                linkedAnimator.SetBool("WallSlide", false);
                return;
            }
            if (!linkedPhysicsControl.isTouchingWall)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                return;
            }
        }

        public override void FixedUpdateAbility()
        {
            linkedPhysicsControl.rb.linearVelocityY = Mathf.Clamp(linkedPhysicsControl.rb.linearVelocityY, -maxSlideSpeed, 1);
        }

        public override void UpdateAnimator()
        {
            linkedAnimator.SetBool(wallSlideParamHash, linkedStateMachine.currentState == PlayerStates.State.WallSlide);
        }
    }
}
