using System;
using UnityEngine;
using UnityEngine.Events;

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
    public LevelParameters levelParams;

    public Level(int _index, LevelState _state, LevelParameters _params)
    {
        levelIndex = _index;
        state = _state;
        levelParams = _params;
    }
}

[Serializable]
public class LevelParameters
{
    public int asteroidsCount;
    public float generationRate;
}

public class LevelController : MonoBehaviour
{
    [SerializeField] private AsteroidFieldController afc;

    public Level currentLevel;

    public void StartLevel(Level level)
    {
        currentLevel = level;
        afc.StartGeneration(currentLevel.levelParams);
    }
}
