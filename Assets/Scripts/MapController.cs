using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapController : MonoBehaviour
{
    [SerializeField] private List<Level> levels;
    private Dictionary<LevelState, Color> levelStateColors;

    void Start()
    {
        levelStateColors = new Dictionary<LevelState, Color>()
        {
            { LevelState.Closed, Color.grey },
            { LevelState.Open, Color.white },
            { LevelState.Passed, Color.green }
        };

        foreach (Level level in levels)
        {
            level.levelButton.GetComponent<Image>().color = levelStateColors[level.state];
            level.levelPath.color = levelStateColors[level.state];
            if (level.state == LevelState.Closed)
            {
                level.levelButton.interactable = false;
            }
            level.levelButton.onClick.AddListener(() => {
                Debug.Log($"Enter {level.levelIndex} level");
            });
        }
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
    public Button levelButton;
    public Image levelPath;
    public LevelState state = LevelState.Closed;
}
