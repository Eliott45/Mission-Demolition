using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Set in Inspector")]
    public GameObject music;
    public Text UIMusic;
    public GameObject UIMenu;
    public GameObject UICredits;

    AudioSource audioSource;
    Text UIMusicText;

    public void Awake() {
        audioSource = music.GetComponent<AudioSource>();
        UIMusicText = UIMusic.GetComponent<Text>();
    }

    public void Update() {
        if (Input.GetKeyDown("escape"))  
        {
            Application.Quit();       
        }
    }

    
    public void StartGame() {
        DontDestroyOnLoad(music);
        SceneManager.LoadScene("Game");
    }

    public void ControlMusic() {
        audioSource.mute = !audioSource.mute;
        if(audioSource.mute) {
            // print("Off");
            UIMusicText.text = "Enable music";
        } else {
            // print("On");
            UIMusicText.text = "Disable music";
        }
    }

    public void Exit() {
        Application.Quit();   
    }

    public void ShowCredits() {
        UIMenu.SetActive(false);
        UICredits.SetActive(true);
    }

    public void HideCredits() {
        UIMenu.SetActive(true);
        UICredits.SetActive(false);
    }
}
