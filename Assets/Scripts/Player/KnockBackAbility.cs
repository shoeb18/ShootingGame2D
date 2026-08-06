using System.Collections;
using UnityEngine;

namespace Player
{
    public class KnockBackAbility : BaseAbility
    {
        private Coroutine currentCoroutine;

        public override void ExitAbility()
        {
            currentCoroutine = null;
        }

        public void StartKnockBack(float duration, Vector2 force, Transform enemyObject)
        {
            if (player.playerStats.GetCanTakeDamage() == false)
                return;
            
            if (currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(KnockBack(duration, force, enemyObject));
            }
            else
            {
                // do nothing OR
                // StopCoroutine(currentCoroutine);
                // currentCoroutine = StartCoroutine(KnockBack(duration, force, enemyObject));
            }
        }

        public void StartSwingKnockBack(float duration, Vector2 force, int direction)
        {
            if (player.playerStats.GetCanTakeDamage() == false)
                return;
            
            if (currentCoroutine == null)
            {
                currentCoroutine = StartCoroutine(SwingKnockBack(duration, force, direction));
            }
            else
            {
                // do nothing OR
                // StopCoroutine(currentCoroutine);
                // currentCoroutine = StartCoroutine(KnockBack(duration, force, enemyObject));
            }
        }
        public IEnumerator KnockBack(float duration, Vector2 force, Transform enemyObject)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.KnockBack);
            linkedPhysicsControl.ResetVelocity();

            if (transform.position.x >= enemyObject.transform.position.x)
            {
                linkedPhysicsControl.rb.linearVelocity = force;
            }
            else
            {
                linkedPhysicsControl.rb.linearVelocity = new Vector2(-force.x, force.y);
            }

            yield return new WaitForSeconds(duration);

            if (player.playerStats.GetCurrentHealth() > 0)
            {
                if (linkedPhysicsControl.isGrounded)
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
                else
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                }
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Death);
            }
        }

        public IEnumerator SwingKnockBack(float duration, Vector2 force, int direction)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.KnockBack);
            linkedPhysicsControl.ResetVelocity();

            force.x *= direction;
            linkedPhysicsControl.rb.linearVelocity = force;

            yield return new WaitForSeconds(duration);

            if (player.playerStats.GetCurrentHealth() > 0)
            {
                if (linkedPhysicsControl.isGrounded)
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
                else
                {
                    linkedStateMachine.ChangeState(PlayerStates.State.Jump);
                }
            }
            else
            {
                linkedStateMachine.ChangeState(PlayerStates.State.Death);
            }
        }

        public override void UpdateAnimator()
        {
            linkedAnimator.SetBool("KnockBack", linkedStateMachine.currentState == PlayerStates.State.KnockBack);
        }
    }
}
