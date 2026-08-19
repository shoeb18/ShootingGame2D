using System;
using Player;
using UnityEngine;

/* Ladders: Handles player ladder interactions.
 - OnTriggerEnter2D(Collider2D): stores LadderAbility reference when a player enters the ladder trigger.
 - OnTriggerStay2D(Collider2D): while player stays, enables canClimbLadder if permitted.
 - OnTriggerExit2D(Collider2D): disables canClimbLadder when player exits.
*/
public class Ladders : MonoBehaviour
{
    private LadderAbility ladderAbility;

    void OnTriggerEnter2D(Collider2D collision)
    {
        ladderAbility = collision.GetComponent<LadderAbility>();
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        if (ladderAbility != null)
        {
            if (ladderAbility.isPermitted)
            {
                ladderAbility.canClimbLadder = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (ladderAbility != null)
        {
            if (ladderAbility.isPermitted)
            {
                ladderAbility.canClimbLadder = false;
            }
        }
    }
}
