

[System.Serializable]
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