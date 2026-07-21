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
                ability.ExitAbility();
                previousState = currentState;
            }
        }

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
