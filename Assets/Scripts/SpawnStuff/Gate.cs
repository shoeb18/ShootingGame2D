using UnityEngine;
using Player;

public class Gate : MonoBehaviour
{
    [SerializeField] private string levelToLoad;
    public SpawnData spawnDataForOtherLevel;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SaveLoadManager.instance.SaveData(spawnDataForOtherLevel, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileName);
            
            PlayerCharacter playerCharacter = other.GetComponent<PlayerCharacter>();
            playerCharacter.GetPlayerInputs().DisablePlayerInputs();
            playerCharacter.GetPhysicsControl().ResetVelocity();
            GetComponent<Collider2D>().enabled = false;
            
            LevelManager.instance.LoadLevelString(levelToLoad);
        }
    }
}
