using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour, IInteractable
{
    public GameObject fire;
    public GameObject water;
    public GameObject HurtBoxes;
    private Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void OnInteract()
    {
        anim.SetBool("Open", true);
        fire.SetActive(false);
        HurtBoxes.SetActive(false);
        water.SetActive(true);
    }

    public string GetInteractPrompt()
    {
        return "Open";
    }
}
