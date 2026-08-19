namespace Player
{
    /* PlayerStates: Enumeration of possible player states used by the StateMachine.
*/
public class PlayerStates
    {
        public enum State
        {
            Idle,
            Run,
            Jump,
            DoubleJump,
            WallJump,
            WallSlide,
            Dash,
            Crouch,
            LadderClimb,
            Ignore,
            KnockBack,
            Death
        }
    }
}
