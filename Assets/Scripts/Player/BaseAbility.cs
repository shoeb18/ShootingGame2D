using UnityEngine;

namespace Player
{
    public class BaseAbility : MonoBehaviour
    {
        protected PlayerCharacter playerCharacter;
        protected PlayerInputs linkedPlayerInputs;
        protected StateMachine linkedStateMachine;
        protected Animator linkedAnimator;
        protected PhysicsControl linkedPhysicsControl;
        public PlayerStates.State abilityState;
        public bool isPermitted = true;

        protected virtual void Start()
        {
            Initialization();
        }

        public virtual void EnterAbility()
        {
        
        }

        public virtual void ExitAbility()
        {
        
        }

        public virtual void UpdateAbility()
        {

        }

        public virtual void FixedUpdateAbility()
        {

        }
        public virtual void UpdateAnimator()
        {

        }
    
        public virtual void Initialization()
        {
            playerCharacter = GetComponent<PlayerCharacter>();

            if (playerCharacter != null)
            {
                linkedPlayerInputs = playerCharacter.GetPlayerInputs();
                linkedStateMachine = playerCharacter.GetStateMachine();
                linkedAnimator = playerCharacter.GetAnimator();
                linkedPhysicsControl = playerCharacter.GetPhysicsControl();
            }
        }
    }
}
