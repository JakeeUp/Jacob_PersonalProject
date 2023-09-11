using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour, IInteractable
{

    public bool enginTurnOn;
    public GameObject turnOn, turnOff;
    private AudioSource audios;
    public AudioClip engineStartSFX;



    // Start is called before the first frame update
    void Start()
    {
        audios = GetComponent<AudioSource>();
    }


   

    public void OnInteract()
    {
        enginTurnOn = !enginTurnOn;
        if (enginTurnOn)
        {
            turnOn.SetActive(true);
            turnOff.SetActive(false);
            audios.PlayOneShot(engineStartSFX);
            Invoke("PlayEngine", 2);
        }
        else
        {
            turnOn.SetActive(false);
            turnOff.SetActive(true);
            audios.Stop();
            CancelInvoke("PlayEngine");
        }
    }

    public void PlayEngine()
    {
        audios.Play();
    }

    public string GetInteractPrompt()
    {
        return enginTurnOn ? "Turn Off Generator" : "Turn On Generator";

    }
}
