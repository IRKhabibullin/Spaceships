using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

[Serializable]
public class GameModel
{
    public const string SAVE_FILE_PATH = "data";
    public List<Level> levels;
    public int maxLevel;

    private void GenerateLevels()
    {
        levels = new List<Level>();
        for (int i = 0; i < maxLevel; i++)
        {
            levels.Add(new Level(
                i,
                i == 0 ? LevelState.Open : LevelState.Closed,
                GetLevelParameters(i)
            ));
        }
        SaveGame();
    }

    private LevelParameters GetLevelParameters(int level)
    {
        LevelParameters levelParams = new LevelParameters();
        levelParams.asteroidsCount = (int)UnityEngine.Random.Range(3 + level * 4, 5 + level * 5);
        levelParams.generationRate = UnityEngine.Random.Range(0.8f - level * 0.1f, 1 - level * 0.1f) * 3;
        return levelParams;
    }

    public void PassLevel(int levelIndex)
    {
        levels[levelIndex].state = LevelState.Passed;
        if (levelIndex < levels.Count - 1 && levels[levelIndex + 1].state == LevelState.Closed)
            levels[levelIndex + 1].state = LevelState.Open;
        SaveGame();
    }

    #region game save actions
    public void SaveGame()
    {
        Stream stream = File.Open(SAVE_FILE_PATH, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, levels);
        stream.Close();
    }

    public void LoadGame()
    {
        try
        {
            if (HasSavedGame())
            {
                Stream stream = File.Open(SAVE_FILE_PATH, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                levels = (List<Level>)formatter.Deserialize(stream);
                stream.Close();
            }
            else
            {
                GenerateLevels();
            }
        }
        catch (Exception) {}
    }

    public static bool HasSavedGame()
    {
        return File.Exists(SAVE_FILE_PATH);
    }

    public static void DeleteSavedGame()
    {
        File.Delete(SAVE_FILE_PATH);
    }
    #endregion
}
