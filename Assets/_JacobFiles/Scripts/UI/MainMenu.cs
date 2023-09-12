using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioClip startSFX;
    public AudioClip clickSFX;
    public AudioClip musicSFX;
    private AudioSource audioS;
    // Start is called before the first frame update
    void Start()
    {
        audioS = GetComponent<AudioSource>();
        audioS.PlayOneShot(musicSFX);
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnNewGameButton()
    {
        PlayerPrefs.DeleteAll();
        audioS.PlayOneShot(startSFX);
        audioS.PlayOneShot(clickSFX);
        Invoke("StartSound",5f);
    }
    public void OnStartGameButtonFromObjectiveMenu()
    {
        PlayerPrefs.DeleteAll();
        audioS.PlayOneShot(startSFX);
        audioS.PlayOneShot(clickSFX);
        Invoke("StartGame", 5f);
    }
    public void OnControlsButton()
    {
        PlayerPrefs.DeleteAll();
        audioS.PlayOneShot(clickSFX);
        SceneManager.LoadScene("Controls");
    }

    public void BackButton()
    {
        audioS.PlayOneShot(startSFX);
        audioS.PlayOneShot(clickSFX);
        Invoke("ControlsMenuBackButton", 5f);
    }
    public void ControlsMenuBackButton()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void StartSound()
    {
        SceneManager.LoadScene("ObjectiveScene");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void OnQuitButton()
    {
        Application.Quit();
        audioS.PlayOneShot(clickSFX);
    }
}
