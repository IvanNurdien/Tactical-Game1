using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject PanelMenu;
    public void Update()
    {
<<<<<<< Updated upstream

=======
        
>>>>>>> Stashed changes
    }
    public void Start()
    {
        PanelMenu.SetActive(true);
    }
    public void Play()
    {
        SceneManager.LoadScene("Lobby");
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
