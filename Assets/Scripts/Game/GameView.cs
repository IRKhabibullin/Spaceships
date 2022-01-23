using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    [SerializeField] private GameObject map;
    [SerializeField] private List<Button> levelButtons;
    [SerializeField] private List<Image> levelPaths;

    Dictionary<LevelState, Color> levelStateColors = new Dictionary<LevelState, Color>()
    {
        { LevelState.Closed, Color.grey },
        { LevelState.Open, Color.white },
        { LevelState.Passed, Color.green }
    };

    public void InitMap(List<Level> levels)
    {
        GamePresenter _gp = gameObject.GetComponent<GamePresenter>();
        foreach (Level level in levels)
        {
            levelButtons[level.levelIndex].onClick.AddListener(() =>
            {
                ToggleMap(false);
                _gp.OnLevelSelected(level.levelIndex);
            });
        }
        RedrawMap(levels);
    }

    public void RedrawMap(List<Level> levels)
    {
        
        foreach (Level level in levels)
        {
            levelButtons[level.levelIndex].GetComponent<Image>().color = levelStateColors[level.state];
            levelPaths[level.levelIndex].color = levelStateColors[level.state];
            levelButtons[level.levelIndex].interactable = level.state != LevelState.Closed;
        }
    }

    public void ToggleMap(bool value)
    {
        map.SetActive(value);
    }
}
