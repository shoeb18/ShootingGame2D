using Player;
using UnityEngine;

public class DeathAbility : BaseAbility
{
    public override void EnterAbility()
    {
        linkedPlayerInputs.DisablePlayerInputs();
        linkedPhysicsControl.ResetVelocity();

        if (linkedPhysicsControl.isGrounded)
        {
            linkedAnimator.SetBool("Death", true);
        }
        else
        {
            // air death animation
            // currently we don't have it. using same animation for now
            linkedAnimator.SetBool("Death", true);
        }
    }

    public void ResetGame()
    {
        Debug.Log("Reset Game");
    }
}
