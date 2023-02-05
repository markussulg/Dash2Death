using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    public void BackToMenu(GameObject from)
    {
        mainMenu.SetActive(true);
        from.SetActive(false);
    }

    public void GoToFromMenu(GameObject To)
    {
        mainMenu.SetActive(false);
        To.SetActive(true);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(0); //To be finished
    }

    public void StartLevelTutoral()
    {
        SceneManager.LoadScene(2);
    }

    public void StartLevelEasy()
    {
        SceneManager.LoadScene(4);
    }
    public void StartLevelMedium()
    {
        SceneManager.LoadScene(7);
    }
    public void StartLevelHard()
    {
        SceneManager.LoadScene(6);
    }
    public void StartLevelArena()
    {
        SceneManager.LoadScene(8);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit(); 
    }
}
