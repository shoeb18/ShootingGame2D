using UnityEngine;

public class MoveAbility : BaseAbility
{
    [SerializeField] private float moveSpeed = 5f;
    private string moveAnimationName = "Run";
    private int moveAnimHash;
    public override void Initialization()
    {
        base.Initialization();
        moveAnimHash = Animator.StringToHash(moveAnimationName);
    }

    override public void UpdateAbility()
    {
        if (linkedPlayerInputs.horizontalInput == 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
    }

    override public void FixedUpdateAbility()
    {
        linkedPhysicsControl.rb.linearVelocity = new Vector2(moveSpeed * linkedPlayerInputs.horizontalInput,
         linkedPhysicsControl.rb.linearVelocity.y);
    }

    override public void UpdateAnimator()
    {
        linkedAnimator.SetBool(moveAnimHash, linkedStateMachine.currentState == PlayerStates.State.Run);
    }
}
