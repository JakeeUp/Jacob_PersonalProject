using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Lights : MonoBehaviour, IInteractable
{
    public bool LightsTurnOn;
    public GameObject[] LightObjects;
    public Generator generator;
    public TextMeshProUGUI infoText;
    public string[] TalkText;

    // Start is called before the first frame update
    void Start()
    {
        generator = FindObjectOfType<Generator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (generator.enginTurnOn == false)
        {
            foreach (GameObject lightObject in LightObjects)
            {
                lightObject.SetActive(false);
            }
            LightsTurnOn = false;
        }
    }

  
    public void OnInteract()
    {

        if (generator.enginTurnOn == true)
        {
            LightsTurnOn = !LightsTurnOn;
            if (LightsTurnOn)
            {
                foreach (GameObject lightObject in LightObjects)
                {
                    lightObject.SetActive(true);
                }
                infoText.text = TalkText[Random.Range(0, TalkText.Length)];
                Invoke("DeleteText", 5);
            }
            else
            {
                foreach (GameObject lightObject in LightObjects)
                {
                    lightObject.SetActive(false);
                }
                
            }
        }

    }

    public void DeleteText()
    {

        infoText.text = string.Empty;
    }

    public string GetInteractPrompt()
    {
        if (generator.enginTurnOn == true)
            return LightsTurnOn ? "Turn Off Lights" : "Turn On Lights";
        else
            return "I think these Lights doesnt have electricity";
    }
}
