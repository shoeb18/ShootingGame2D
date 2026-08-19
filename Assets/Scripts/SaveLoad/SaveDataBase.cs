

[System.Serializable]
/* SaveDataBase: Serializable data containers used by SaveLoadManager.
 - ExampleData: simple example with number and name.
 - SpawnData: which spawn key to place the player and facing direction; default Start,true.
 - CheckpointData: saved checkpoint info including scene, checkpoint key and facing; has defaults.
*/
public class ExampleData
{
    public int number;
    public string name;
}

[System.Serializable]
public class SpawnData
{
    public string spawnPointKey;
    public bool facingRight;

    public SpawnData()
    {
        spawnPointKey = "Start";
        facingRight = true;
    }
}

[System.Serializable]
public class CheckpointData
{
    public string sceneToLoad;
    public string checkpointKey;
    public bool facingRight;
    
    public CheckpointData()
    {
        sceneToLoad = "Level 1";
        checkpointKey = "Check1";
        facingRight = true;
    }
}