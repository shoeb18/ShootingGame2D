using UnityEngine;

public class MoveAbility : BaseAbility
{
    [SerializeField] private float moveSpeed = 5f;
    private string moveAnimParameterName = "Run";
    private int moveAnimHash;
    public override void Initialization()
    {
        base.Initialization();
        moveAnimHash = Animator.StringToHash(moveAnimParameterName);
    }

    public override void EnterAbility()
    {
        player.FlipPlayer();
    }
    override public void UpdateAbility()
    {
        if (linkedPhysicsControl.isGrounded && linkedPlayerInputs.horizontalInput == 0)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Idle);
        }
        if (!linkedPhysicsControl.isGrounded)
        {
            linkedStateMachine.ChangeState(PlayerStates.State.Jump);
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
