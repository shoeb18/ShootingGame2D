using UnityEngine;

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
