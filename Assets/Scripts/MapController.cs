using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private LevelController levelController;

    [SerializeField] private List<Level> levels;
    [SerializeField] private List<Button> levelButtons;
    [SerializeField] private List<Image> levelPaths;
    private Dictionary<LevelState, Color> levelStateColors;

    void Start()
    {
        levelStateColors = new Dictionary<LevelState, Color>()
        {
            { LevelState.Closed, Color.grey },
            { LevelState.Open, Color.white },
            { LevelState.Passed, Color.green }
        };

        try
        {
            if (File.Exists("data"))
            {
                Stream stream = File.Open("data", FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();
                levels = (List<Level>)formatter.Deserialize(stream);
                stream.Close();
            }
        }
        catch (Exception) {}

        RedrawMap();
    }

    private void RedrawMap()
    {
        foreach (Level level in levels)
        {
            levelButtons[level.levelIndex - 1].GetComponent<Image>().color = levelStateColors[level.state];
            levelPaths[level.levelIndex - 1].color = levelStateColors[level.state];
            levelButtons[level.levelIndex - 1].interactable = level.state != LevelState.Closed;
            levelButtons[level.levelIndex - 1].onClick.AddListener(() => {
                map.SetActive(false);
                levelController.StartLevel(level.levelIndex);
            });
        }
    }

    public void OnLevelPassed(int levelIndex)
    {
        levels[levelIndex - 1].state = LevelState.Passed;
        if (levels[levelIndex].state == LevelState.Closed)
            levels[levelIndex].state = LevelState.Open;
        RedrawMap();
        SaveGameProgress();
        map.SetActive(true);

    }

    private void SaveGameProgress()
    {
        Stream stream = File.Open("data", FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(stream, levels);
        stream.Close();
    }
}

public enum LevelState
{
    Closed,
    Open,
    Passed
}

[Serializable]
public class Level
{
    public int levelIndex;
    public LevelState state = LevelState.Closed;
}
