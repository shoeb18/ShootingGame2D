using System;
using UnityEngine;
using UnityEngine.InputSystem;

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
