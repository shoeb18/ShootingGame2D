using System;
using System.IO;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite spriteEnabled;
    [SerializeField] private Sprite spriteDisabled;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private CheckpointData checkPointData;

    private void Start()
    {
        string loadPath = Path.Combine(Application.persistentDataPath, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);

        if (File.Exists(loadPath))
        {
            CheckpointData helpCheck = new CheckpointData();
            SaveLoadManager.instance.LoadData(helpCheck, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);
            if (helpCheck.checkpointKey == checkPointData.checkpointKey)
            {
                spriteRenderer.sprite = spriteEnabled;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<ActivateCheckpoint>().checkpoint = this;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<ActivateCheckpoint>().checkpoint = null;
        }
    }

    public void Activate()
    {
        spriteRenderer.sprite = spriteEnabled;
        // save data
        SaveLoadManager.instance.SaveData(checkPointData, SaveLoadManager.instance.folderName, SaveLoadManager.instance.fileCheckpoint);
    }
}
