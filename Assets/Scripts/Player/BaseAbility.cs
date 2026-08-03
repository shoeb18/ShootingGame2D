using UnityEngine;

namespace Player
{
    public class BaseAbility : MonoBehaviour
    {
        protected Player player;
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
            player = GetComponent<Player>();

            if (player != null)
            {
                linkedPlayerInputs = player.GetPlayerInputs();
                linkedStateMachine = player.GetStateMachine();
                linkedAnimator = player.GetAnimator();
                linkedPhysicsControl = player.GetPhysicsControl();
            }
        }
    }
}
