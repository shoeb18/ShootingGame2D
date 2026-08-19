using Player;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class DeathAbility : BaseAbility
{
    public override void EnterAbility()
    {
        linkedPlayerInputs.DisablePlayerInputs();
        linkedPhysicsControl.ResetVelocity();

        SpawnMode.spawnFromCheckpoint = true;

        if (linkedPhysicsControl.isGrounded)
        {
            linkedAnimator.SetBool("Death", true);
        }
        else
        {
            // air death animation
            // currently we don't have it. using same animation for now
            linkedAnimator.SetBool("Death", true);
        }
    }

    public void ResetGame()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);
        if (File.Exists(loadPath))
        {
            CheckpointData checkpointData = new CheckpointData();
            SaveLoadManager.instance.LoadData(checkpointData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);
            LevelManager.instance.LoadLevelString(checkpointData.sceneToLoad);
        }
        else
        {
            LevelManager.instance.RestartLevel();
        }
    }
}
