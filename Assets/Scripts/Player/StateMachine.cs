namespace Player
{
    /* StateMachine: Manages current/previous player state and transitions via abilities.
 - ChangeState(State): safely exit current ability then enter new state's ability (respecting isPermitted flags).
 - ForceChangeState(State): forcefully set current state and track previous state.
*/
public class StateMachine
    {
        public PlayerStates.State currentState;
        public PlayerStates.State previousState;
        public BaseAbility[] abilities;

        public void ChangeState(PlayerStates.State newState)
        {
            foreach (BaseAbility ability in abilities)
            {
                if (ability.abilityState == currentState)
                {
                    if (!ability.isPermitted)
                    {
                        return;
                    }
                }
            }

            // leave the current state
            foreach (BaseAbility ability in abilities)
            {
                if (ability.abilityState == currentState)
                {
                    ability.ExitAbility();
                    previousState = currentState;
                }
            }

            // enter the new state
            foreach (BaseAbility ability in abilities)
            {
                if (ability.abilityState == newState)
                {
                    if (ability.isPermitted)
                    {
                        currentState = newState;
                        ability.EnterAbility();
                    }
                    break;
                }
            }
        }

        public void ForceChangeState(PlayerStates.State newState)
        {
            previousState = currentState;
            currentState = newState;
        }
    }
}
