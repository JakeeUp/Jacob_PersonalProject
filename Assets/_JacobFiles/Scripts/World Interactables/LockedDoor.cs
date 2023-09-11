using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoor : MonoBehaviour, IInteractable
{
    public bool isNormalDoor;
    public Keypad keypad;
    public bool isOpen;
    public string nameObject;
    public Animator anim;
    private AudioSource audios;
    public AudioClip openDoor, closeDoor, CodedDoor;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audios = GetComponent<AudioSource>();
        keypad = FindObjectOfType<Keypad>();
    }





    public void OnInteract()
    {
        if (keypad.unlocked == true || isNormalDoor == true)
        {
            isOpen = !isOpen;
            if (isOpen)
            {
                anim.SetBool("Open", true);
                audios.PlayOneShot(openDoor);
            }
            else
            {
                anim.SetBool("Open", false);
                audios.PlayOneShot(closeDoor);
            }
        }
        else
        {
            audios.PlayOneShot(CodedDoor);
        }
    }

    public string GetInteractPrompt()
    {
        return isOpen ? "Close Door" : "Open Door";

    }
}
