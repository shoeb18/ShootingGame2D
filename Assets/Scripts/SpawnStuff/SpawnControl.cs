using System;
using System.IO;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnControl : MonoBehaviour
{
    private Transform playerTransform;
    [SerializeField] private SpawnIdentifier[] spawnPoints;
    [SerializeField] private SpawnIdentifier[] spawnCheckpoints;
    private SpawnData spawnData = new SpawnData();
    private CheckpointData checkpointData = new CheckpointData();
    private bool canLoadFromCheckpoint = false;

    private void Start()
    {
        playerTransform = GameObject.FindAnyObjectByType<Player.PlayerCharacter>().transform;
        
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);

        if (File.Exists(loadPath))
        {
            SaveLoadManager.instance.LoadData(checkpointData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);
            
            if (checkpointData.sceneToLoad == SceneManager.GetActiveScene().name)
            {
                canLoadFromCheckpoint = true;
            }
            
        }

        if (SpawnMode.spawnFromCheckpoint == true && canLoadFromCheckpoint == true)
        {
            foreach (SpawnIdentifier spawnCheck in spawnCheckpoints)
            {
                if (spawnCheck != null)
                {
                    if (spawnCheck.spawnKey == checkpointData.checkpointKey)
                    {
                        playerTransform.position = spawnCheck.transform.position;
                        break;
                    }
                }
            }
            if (spawnData.facingRight == false)
            {
                playerTransform.GetComponent<PlayerCharacter>().ForceFlipPlayer();
            }

            SpawnMode.spawnFromCheckpoint = false;
        }
        else
        {
            SaveLoadManager.instance.LoadData(spawnData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileName);
            foreach (SpawnIdentifier spawnPoint in spawnPoints)
            {
                if (spawnPoint != null)
                {
                    if (spawnPoint.spawnKey == spawnData.spawnPointKey)
                    {
                        playerTransform.position = spawnPoint.transform.position;
                        break;
                    }
                }
            }
            if (spawnData.facingRight == false)
            {
                playerTransform.GetComponent<PlayerCharacter>().ForceFlipPlayer();
            }
        }




    }
}
