using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    /* PlayerInputs: Reads input actions and exposes horizontal/vertical values.
 - OnEnable()/OnDisable(): enable/disable the Player input action map.
 - DisablePlayerInputs(): disables the player action map (used during transitions like level load).
 - Update(): read move and vertical action values into public fields.
*/
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

        public void DisablePlayerInputs()
        {
            playerMap.Disable();
        }

        void Update()
        {
            horizontalInput = moveAction.action.ReadValue<float>();
            verticalInput = verticalAction.action.ReadValue<float>();
        }
    }
}
