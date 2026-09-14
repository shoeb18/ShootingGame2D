using System;
using System.Collections;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] protected EnemySimpleStateMachine enemyStateMachine;
    [SerializeField] protected float health;
    
    [Header("Flash")] 
    [SerializeField] private float flashDuration;
    [SerializeField, Range(0,1)] private float flashIntensity;
    [SerializeField] private Color flashColor;
    [SerializeField] private Material flashMaterial;
    private Material defaultMaterial;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    protected Coroutine damageCoroutine;

    private void Start()
    {
        defaultMaterial = spriteRenderer.material;
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        DamageProcess();
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
        }
        damageCoroutine = StartCoroutine(Flash());
        if (health <= 0)
        {
            DeathProcess();
        }
    }
    
    protected virtual void DeathProcess()
    {
        // Implement death logic in child classes
    }

    protected virtual void DamageProcess()
    {
        // Implement damage logic in child classes
    }
    
    private IEnumerator Flash()
    {
        spriteRenderer.material = flashMaterial;
        flashMaterial.color = flashColor;
        flashMaterial.SetColor("_FlashColor", flashColor);
        flashMaterial.SetFloat("_FlashAmount", flashDuration);
            
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = defaultMaterial;
        damageCoroutine = null;
    }

}
