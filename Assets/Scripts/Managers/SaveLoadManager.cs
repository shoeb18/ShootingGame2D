using System;
using System.IO;
using UnityEngine;

/* SaveLoadManager: Generic save/load utilities and file management.
 - Awake(): singleton setup and DontDestroyOnLoad.
 - SaveData<T>(T,string,string): writes JSON to persistent path, ensures directories and file attributes.
 - LoadData<T>(T,string,string): loads JSON and overwrites provided object if file exists.
 - DeleteSaveFile(string,string): deletes a specific save file.
 - DeleteFolder(string): deletes a folder under persistent data path.
*/
public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager instance;
    [Header("Spawn Settings")]
    public string folderName = "SaveFiles";
    public string fileName = "SpawnPoint.json";
    
    [Header("Checkpoint Settings")]
    public string fileCheckpoint = "Checkpoint.json";
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // template save and load functions
    public void SaveData<T>(T data, string folderName, string fileName)
    {
        string savePath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        string dirPath = Path.GetDirectoryName(savePath);

        // If a directory exists where the file should be, remove it (it blocks file creation).
        if (Directory.Exists(savePath))
        {
            Debug.LogWarning($"A directory exists at file path '{savePath}'. Deleting directory to create file.");
            Directory.Delete(savePath, true);
        }

        // Ensure parent directory exists
        if (!string.IsNullOrEmpty(dirPath) && !Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        // If an existing file is read-only, clear the attribute so it can be overwritten.
        if (File.Exists(savePath))
        {
            var attrs = File.GetAttributes(savePath);
            if ((attrs & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                File.SetAttributes(savePath, attrs & ~FileAttributes.ReadOnly);
        }

        File.WriteAllText(savePath, JsonUtility.ToJson(data, true));
    }

    public void LoadData<T>(T data, string folderName, string fileName)
    {
        string loadPath = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(loadPath))
        {
            string loadDataString = File.ReadAllText(loadPath);
            JsonUtility.FromJsonOverwrite(loadDataString, data);
        }
    }

    public void DeleteSaveFile(string folderName, string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, folderName, fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

    public void DeleteFolder(string folderName)
    {
        string path = Path.Combine(Application.persistentDataPath, folderName);
        if (Directory.Exists(path))
        {
            Directory.Delete(path, true);
        }
    }
    
}
