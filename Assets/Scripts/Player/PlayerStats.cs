using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerStats : MonoBehaviour
    {
        [Header("Health")]
        [SerializeField] private float maxHealth;
        [SerializeField] private Player player;
        [SerializeField] private HealthBarControl healthBarControl;
        private float currentHealth;

        [Header("Flash")] 
        [SerializeField] private float flashDuration;
        [SerializeField, Range(0,1)] private float flashIntensity;
        [SerializeField] private Color flashColor;
        [SerializeField] private Material flashMaterial;
        private Material defaultMaterial;
        private SpriteRenderer spriteRenderer;
        private bool canTakeDamage;

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

        public void DamagePlayer(float damageValue)
        {
            currentHealth -= damageValue;
            healthBarControl.SetSliderValue(currentHealth, maxHealth);
            StartCoroutine(Flash());

            if (currentHealth <= 0)
            {
                print("Player dead!");
            }
        }

        private IEnumerator Flash()
        {
            spriteRenderer.material = flashMaterial;
            flashMaterial.color = flashColor;
            flashMaterial.SetColor("_FlashColor", flashColor);
            flashMaterial.SetFloat("_FlashAmount", flashDuration);
            
            yield return new WaitForSeconds(flashDuration);
            spriteRenderer.material = defaultMaterial;
        }
    }
}
