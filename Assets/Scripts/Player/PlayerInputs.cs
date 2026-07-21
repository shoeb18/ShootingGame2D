using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputActionMap playerMap;
    private InputActionMap uiMap;

    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    [HideInInspector]
    public float horizontalInput;

    private void OnEnable()
    {
        playerMap = playerInput.actions.FindActionMap("Player");
        uiMap = playerInput.actions.FindActionMap("UI");

        playerMap.Enable();
        jumpAction.action.performed += TryToJump;
        jumpAction.action.canceled += StopJump;
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= TryToJump;
        jumpAction.action.canceled -= StopJump;
        playerMap.Disable();
    }

    private void TryToJump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("Jump action performed");
        }
    }
    
    private void StopJump(InputAction.CallbackContext context)
    {
        if (context.canceled)
        {
            Debug.Log("Jump action canceled");
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        horizontalInput = moveAction.action.ReadValue<float>();
    }
}
