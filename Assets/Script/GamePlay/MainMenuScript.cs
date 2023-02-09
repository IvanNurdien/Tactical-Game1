using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Play ()
    {
        SceneManager.LoadScene("Gameplay");
    }
    void Exit ()
    {
        Application.Quit();
    }
}
