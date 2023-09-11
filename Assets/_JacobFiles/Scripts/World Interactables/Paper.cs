using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Paper : MonoBehaviour, IInteractable
{
    public GameObject paper;
    public string nameObject;
    public Sprite papersprite;
    public Image paperUI;
    public TextMeshProUGUI UItext;
    [TextArea(7, 10)]
    public string textInfo;

    public TextMeshProUGUI UIPlayerText;
    [TextArea(7, 10)]
    public string PlayertextInfo;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (paper.activeInHierarchy)
        {
            Cursor.lockState = CursorLockMode.None;
            PlayerController.instance.canLook = false;
        }
    }


    public void OnInteract()
    {
        paper.SetActive(true);
        paperUI.sprite = papersprite;
        UItext.text = textInfo;

        UIPlayerText.text = PlayertextInfo;
    }

    public void CloseUI()
    {
        paper.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        PlayerController.instance.canLook = true;
    }

    public string GetInteractPrompt()
    {
        return string.Format("Examine {0}", nameObject);
    }
}
