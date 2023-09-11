using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Keypad : MonoBehaviour, IInteractable
{
    public string nameObject;
    public bool unlocked = false;
    public TextMeshProUGUI textCode;
    public GameObject keypadPanel;
    public string password = "2022";
    public AudioSource audios;
    public AudioClip typeSound;
    public AudioClip validSound;
    public AudioClip invalidsound;

    void Start()
    {
        audios = GetComponent<AudioSource>();
    }

    void Update()
    {
        //check if keypad ui is active 
        if (keypadPanel.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
            PlayerController.instance.canLook = false;
        }
    }
    public void Excute()
    {
        if (textCode.text == password)
        {
            textCode.text = "Valid";
            textCode.color = Color.green;
            audios.PlayOneShot(validSound);
            unlocked = true;
            Invoke("CloseKeypad", 2);
        }
        else
        {
            audios.PlayOneShot(invalidsound);
            textCode.text = "Invalid";
            textCode.color = Color.red;
            StartCoroutine("ResetPW");
        }
    }
    IEnumerator ResetPW()
    {
        yield return new WaitForSeconds(1);
        textCode.text = string.Empty;
        textCode.color = Color.white;
    }

    public void Number(int number)
    {
        textCode.text += number.ToString();
        audios.PlayOneShot(typeSound);
    }

    public string GetInteractPrompt()
    {
        return string.Format("Examine {0}", nameObject);
    }

    public void OnInteract()
    {
        keypadPanel.SetActive(true);
    }
    public void CloseKeypad()
    {
        keypadPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        PlayerController.instance.canLook = true;

        if (unlocked == true)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }
    public void ClearInput()
    {
        textCode.text = string.Empty;
        audios.PlayOneShot(typeSound);
    }

}
