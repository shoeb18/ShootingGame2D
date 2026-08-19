using UnityEngine;

namespace Player
{
    /* PlayerCharacter: Root component that wires abilities, inputs, physics and animation.
 - GetStateMachine/GetPhysicsControl/GetAnimator/GetPlayerInputs(): accessor helpers.
 - Awake(): initialize state machine, gather abilities and cache components.
 - Update(): calls UpdateAbility/UpdateAnimator on abilities matching current state and flips sprite.
 - FixedUpdate(): calls FixedUpdateAbility on current ability.
 - FlipPlayer()/ForceFlipPlayer(): flip the transform to face movement direction.
*/
public class PlayerCharacter : MonoBehaviour
    {
        private PlayerInputs playerInputs;
        private StateMachine stateMachine;

        private BaseAbility[] playerAbilities;
        private Animator anim;
        private PhysicsControl physicsControl;
        public bool facingRight = true;
        public PlayerStats playerStats;

        public StateMachine GetStateMachine()
        {
            return stateMachine;
        }
        public PhysicsControl GetPhysicsControl()
        {
            return physicsControl;
        }
        public Animator GetAnimator()
        {
            return anim;
        }
        public PlayerInputs GetPlayerInputs()
        {
            return playerInputs;
        }

        private void Awake()
        {
            stateMachine = new StateMachine();
            playerAbilities = GetComponents<BaseAbility>();
            stateMachine.abilities = playerAbilities;
            anim = GetComponent<Animator>();
            physicsControl = GetComponent<PhysicsControl>();
            playerInputs = GetComponent<PlayerInputs>();
        }

        private void Update()
        {
            foreach (BaseAbility ability in playerAbilities)
            {
                if (ability.abilityState == stateMachine.currentState)
                {
                    ability.UpdateAbility();
                }

                ability.UpdateAnimator();
            }

            FlipPlayer();
        }

        private void FixedUpdate()
        {
            foreach (BaseAbility ability in playerAbilities)
            {
                if (ability.abilityState == stateMachine.currentState)
                {
                    ability.FixedUpdateAbility();
                }
            }
        }

        public void FlipPlayer()
        {
            if (facingRight && playerInputs.horizontalInput < 0)
            {
                transform.Rotate(0f, 180f, 0f);
                facingRight = !facingRight;
            }
            else if (!facingRight && playerInputs.horizontalInput > 0)
            {
                transform.Rotate(0f, 180f, 0f);
                facingRight = !facingRight;
            }
        }

        public void ForceFlipPlayer()
        {
            transform.Rotate(0f, 180f, 0f);
            facingRight = !facingRight;
        }
    }
}
