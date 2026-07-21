using UnityEngine;

public class Player : MonoBehaviour
{
    private PlayerInputs playerInputs;
    private StateMachine stateMachine;

    private BaseAbility[] playerAbilities;
    private Animator anim;
    private PhysicsControl physicsControl;
    private SpriteRenderer spriteRenderer;
    public bool facingRight = true;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        FlipPlayer(playerInputs.horizontalInput);
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

    private void FlipPlayer(float horizontalInput)
    {
        if (horizontalInput > 0.01f && !facingRight)
        {
            facingRight = true;
            spriteRenderer.flipX = false;
        }
        else if (horizontalInput < -0.01f && facingRight)
        {
            facingRight = false;
            spriteRenderer.flipX = true;
        }
    }
}
