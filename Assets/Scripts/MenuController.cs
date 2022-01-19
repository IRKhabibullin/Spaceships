using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    [SerializeField] private Button continueButton;

    void Start()
    {
        if (File.Exists("data"))
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
        if (File.Exists("data"))
            File.Delete("data");
        SceneManager.LoadScene("Main");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
