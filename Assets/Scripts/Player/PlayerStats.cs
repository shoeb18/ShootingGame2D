using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    /* PlayerStats: Tracks health, flashing damage effect and collider swapping for crouch/stand.
 - EnableStandingCollider/EnableCrouchingCollider(): switch current collider based on state.
 - Awake(): cache sprite renderer.
 - Start(): initialize health and UI, store default material.
 - GetCurrentHealth(): accessor for current health.
 - EnableDamage()/DisableDamage(): toggle invulnerability flag.
 - DamagePlayer(float): apply damage, update UI and trigger flash; handle death state change.
 - Flash(): coroutine that swaps material to flash effect then restores material and invulnerability.
 - GetCanTakeDamage(): returns whether player can currently be damaged.
*/
public class PlayerStats : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float maxHealth;
        [FormerlySerializedAs("player")] [SerializeField] private PlayerCharacter playerCharacter;
        [SerializeField] private HealthBarControl healthBarControl;
        private float currentHealth;

        [Header("Flash")] 
        [SerializeField] private float flashDuration;
        [SerializeField, Range(0,1)] private float flashIntensity;
        [SerializeField] private Color flashColor;
        [SerializeField] private Material flashMaterial;
        private Material defaultMaterial;
        private SpriteRenderer spriteRenderer;
        private bool canTakeDamage = true;
        
        [Header("StatesColliders")]
        [SerializeField] private Collider2D standingCollider;
        [SerializeField] private Collider2D crouchingCollider;
        private Collider2D currentCollider;

        public void EnableStandingCollider()
        {
            if (currentHealth <= 0) return;
            currentCollider = standingCollider;
            standingCollider.enabled = true;
            crouchingCollider.enabled = false;
        }

        public void EnableCrouchingCollider()
        {
            if (currentHealth <= 0) return;
            currentCollider = crouchingCollider;
            crouchingCollider.enabled = true;
            standingCollider.enabled = false;
        }
        

        private void Awake()
        {
            spriteRenderer = GetComponentInParent<SpriteRenderer>();
        }


        void Start()
        {
            currentHealth = maxHealth;
            healthBarControl.SetSliderValue(currentHealth, maxHealth);
            defaultMaterial = spriteRenderer.material;
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }

        public void EnableDamage()
        {
            canTakeDamage = true;
        }

        public void DisableDamage()
        {
            canTakeDamage = false;
        }

        public void DamagePlayer(float damageValue)
        {
            if (canTakeDamage == false) return;
            currentHealth -= damageValue;
            healthBarControl.SetSliderValue(currentHealth, maxHealth);
            StartCoroutine(Flash());

            if (currentHealth <= 0)
            {
                if (playerCharacter.GetStateMachine().currentState != PlayerStates.State.KnockBack)
                    playerCharacter.GetStateMachine().ChangeState(PlayerStates.State.Death);
            }
        }

        private IEnumerator Flash()
        {
            spriteRenderer.material = flashMaterial;
            flashMaterial.color = flashColor;
            flashMaterial.SetColor("_FlashColor", flashColor);
            flashMaterial.SetFloat("_FlashAmount", flashDuration);
            canTakeDamage = false;
            
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.material = defaultMaterial;
            if (currentHealth > 0)
                canTakeDamage = true;
        }

        public bool GetCanTakeDamage()
        {
            return canTakeDamage;
        }
    }
}
