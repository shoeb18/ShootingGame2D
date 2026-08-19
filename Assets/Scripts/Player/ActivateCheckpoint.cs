using System;
using UnityEngine;
using UnityEngine.InputSystem;

/* ActivateCheckpoint: Listens for checkpoint input and triggers checkpoint activation.
 - OnEnable()/OnDisable(): subscribe/unsubscribe to input action.
 - TryToActivate(InputAction.CallbackContext): if standing at a checkpoint, call Activate() on it.
*/
public class ActivateCheckpoint : MonoBehaviour
{
    [SerializeField] private InputActionReference checkpointInputAction;
    [HideInInspector] public Checkpoint checkpoint;

    private void OnEnable()
    {
        checkpointInputAction.action.performed += TryToActivate;
    }

    private void OnDisable()
    {
        checkpointInputAction.action.performed -= TryToActivate;
    }

    private void TryToActivate(InputAction.CallbackContext context)
    {
        if (checkpoint == null)
        {
            return;
        }
        checkpoint.Activate();
    }

}
