using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    public PlayerInput playerInput;
    private InputActionMap playerMap;
    private InputActionMap uiMap;

    public InputActionReference moveAction;
    public InputActionReference verticalAction;

    [HideInInspector] public float horizontalInput;
    [HideInInspector] public float verticalInput;

    private void OnEnable()
    {
        playerMap = playerInput.actions.FindActionMap("Player");
        uiMap = playerInput.actions.FindActionMap("UI");

        playerMap.Enable();
    }

    private void OnDisable()
    {
        playerMap.Disable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        horizontalInput = moveAction.action.ReadValue<float>();
        verticalInput = verticalAction.action.ReadValue<float>();
    }
}
