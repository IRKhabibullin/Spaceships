using UnityEngine;

public class GamePresenter : MonoBehaviour
{
    [SerializeField] private GameModel gameModel = new GameModel();
    [SerializeField] private GameView gameView;
    [SerializeField] private LevelController levelController;

    void Start()
    {
        gameModel.LoadGame();
        gameView.InitMap(gameModel.levels);
    }

    public void OnLevelSelected(int levelIndex)
    {
        levelController.StartLevel(gameModel.levels[levelIndex]);
    }

    public void OnLevelPassed()
    {
        gameModel.PassLevel(levelController.currentLevel.levelIndex);
        gameView.RedrawMap(gameModel.levels);
        gameView.ToggleMap(true);

        if (levelController.currentLevel.levelIndex >= gameModel.maxLevel)
        {
            Debug.Log("You won!");
            return;
        }
    }
}
