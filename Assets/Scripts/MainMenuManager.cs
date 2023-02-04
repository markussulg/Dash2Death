using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
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

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit(); 
    }
}
