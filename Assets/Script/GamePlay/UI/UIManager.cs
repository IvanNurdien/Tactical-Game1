using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject PanelMenu;
    
    public void Start()
    {
        PanelMenu.SetActive(true);
    }
    public void Play()
    {
        SceneManager.LoadScene("Loading Screen");
    }
    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void PlayMatch()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void ExitLobby ()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
}
