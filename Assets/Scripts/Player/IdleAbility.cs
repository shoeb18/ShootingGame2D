using UnityEngine;

namespace Player
{
    /* IdleAbility: Handles idle state logic and animation.
 - Initialization(): cache animator hash.
 - EnterAbility(): zero horizontal velocity.
 - UpdateAbility(): switch to Run when horizontal input present.
 - UpdateAnimator(): set idle animation parameter.
*/
public class IdleAbility : BaseAbility
    {
        private string idleAnimParameterName = "Idle";
        private int idleAnimHash;
        override public void Initialization()
        {
            base.Initialization();
            idleAnimHash = Animator.StringToHash(idleAnimParameterName);
            // add more things..
        }

        override public void EnterAbility()
        {
            linkedPhysicsControl.rb.linearVelocityX = 0f;
        }

        public override void UpdateAbility()
        {
            if (linkedPlayerInputs.horizontalInput != 0)
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Run);
            }
        }

        override public void UpdateAnimator()
        {
            linkedAnimator.SetBool(idleAnimHash, linkedStateMachine.currentState == PlayerStates.State.Idle);
        }

    }
}
