using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class GameplayMenu : MonoBehaviourPunCallbacks
{
    public static bool gameIsPaused;

    public AudioMixer audioMixer;

    public GameObject pausePanelUI;
    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;

    public GameObject playerLeaveRoomUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (gameIsPaused)
            {
                Resume();
            } else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        navigateToPause();
        pausePanelUI.SetActive(false);

        gameIsPaused = false;
    }

    void Pause()
    {
        pausePanelUI.SetActive(true);

        gameIsPaused = true;
    }

    public void navigateToOptions()
    {
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(true);
    }

    public void navigateToPause()
    {
        pauseMenuUI.SetActive(true);
        optionsMenuUI.SetActive(false);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", volume);
    }

    public void DisconnectPlayer()
    {
        StartCoroutine(DisconnectingAndLoad());
    }

    IEnumerator DisconnectingAndLoad()
    {
        //PhotonNetwork.LeaveRoom();

        PhotonNetwork.Disconnect();
        //while (PhotonNetwork.InRoom)
        while (PhotonNetwork.IsConnected)
            yield return null;

        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        SceneManager.LoadScene("Loading Screen");
    }

    IEnumerator DisconnectingAfterLeave()
    {
        //PhotonNetwork.LeaveRoom();

        PhotonNetwork.Disconnect();
        //while (PhotonNetwork.InRoom)
        while (PhotonNetwork.IsConnected)
            yield return null;

        yield return new WaitForSeconds(3f);
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        SceneManager.LoadScene("Loading Screen");
    }

    public override void OnPlayerLeftRoom(Player newPlayer)
    {
        playerLeaveRoomUI.SetActive(true);
        StartCoroutine(DisconnectingAfterLeave());
    }

    
}
