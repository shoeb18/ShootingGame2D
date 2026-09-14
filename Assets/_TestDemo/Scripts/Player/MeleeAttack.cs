using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    [SerializeField] private GameObject weaponHitBox;
    
    // for testing purposes, we can activate the hitbox with a key press using new Input System
    [Header("Input")]
    public InputAction attackInput;
    
    private void OnEnable()
    {
        attackInput.Enable();
        attackInput.performed += OnAttack;
    }

    private void OnDisable()
    {
        attackInput.Disable();
        attackInput.performed -= OnAttack; 
    }
    
    public void ActivateHitBox()
    {
        weaponHitBox.SetActive(true);
    }
    
    public void DeactivateHitBox()
    {
        weaponHitBox.SetActive(false);
    }
    
    // testing the attack input, you can remove this later when you implement the actual attack logic
    private void OnAttack(InputAction.CallbackContext context)
    {
        ActivateHitBox();
        // Deactivate after a short delay to simulate the attack duration
        Invoke(nameof(DeactivateHitBox), 0.2f); // Adjust the duration as needed
    }
}
