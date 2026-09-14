using System;
using UnityEngine;

public class AttackDetection : MonoBehaviour
{
    [SerializeField] private PatrolPhysics _patrolPhysics;

    private void OnTriggerStay2D(Collider2D other)
    {
        _patrolPhysics.inAttackRange = true;
    }
    
    private void OnTriggerExit2D(Collider2D other)
    {
        _patrolPhysics.inAttackRange = false;
    }
}
