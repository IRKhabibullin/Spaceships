using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    void Start()
    {
        if (GameModel.HasSavedGame())
        {
            continueButton.gameObject.SetActive(true);
        }
    }

    public void Continue()
    {
        SceneManager.LoadScene("Main");
    }

    public void NewGame()
    {
        if (GameModel.HasSavedGame())
            GameModel.DeleteSavedGame();
        SceneManager.LoadScene("Main");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
