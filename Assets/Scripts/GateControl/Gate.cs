using UnityEngine;
using Player;

public class Gate : MonoBehaviour
{
    [SerializeField] private string levelToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();
            playerCharacter.GetPlayerInputs().DisablePlayerInputs();
            playerCharacter.GetPhysicsControl().ResetVelocity();
            GetComponent<Collider2D>().enabled = false;
            
            LevelManager.instance.LoadNextLevel(levelToLoad);
        }
    }
}
