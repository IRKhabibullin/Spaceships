using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamePresenter : MonoBehaviour
{
    [SerializeField] private GameModel gameModel = new GameModel();
    [SerializeField] private GameView gameView;
    [SerializeField] private LevelController levelController;
    [SerializeField] private GameObject gameFinishPanel;

    void Start()
    {
        gameModel.LoadGame();
        gameView.InitMap(gameModel.levels);
        gameView.ToggleMap(true);
    }

    public void OnLevelSelected(int levelIndex)
    {
        levelController.StartLevel(gameModel.levels[levelIndex]);
    }

    public void OnLevelPassed()
    {
        gameModel.PassLevel(levelController.currentLevel.levelIndex);

        if (levelController.currentLevel.levelIndex >= gameModel.maxLevel - 1)
        {
            gameFinishPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Victory!";
            gameFinishPanel.SetActive(true);
            return;
        }
        gameView.RedrawMap(gameModel.levels);
        gameView.ToggleMap(true);
    }

    public void GameOver()
    {
        gameFinishPanel.GetComponentInChildren<TextMeshProUGUI>().text = "Game over";
        gameFinishPanel.SetActive(true);
    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
