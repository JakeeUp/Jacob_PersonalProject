using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InteractionManager : MonoBehaviour
{
    [Header("Interaction Settings")]
    public float checkRate = 0.04f;
    public float maxCheckDistance;
    public LayerMask layerMask;

    private GameObject currentInteractGameObject;
    private IInteractable currentInteractable;

    [Header("UI")]
    public TextMeshProUGUI promptText;

    private Camera cam;

    private float lastCheckTime;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        HandleInteractionCheck();
    }

    private void HandleInteractionCheck()
    {
        if (Time.time - lastCheckTime > checkRate)
        {
            lastCheckTime = Time.time;
            Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, maxCheckDistance, layerMask))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject != currentInteractGameObject)
                {
                    currentInteractGameObject = hitObject;
                    currentInteractable = hitObject.GetComponent<IInteractable>();
                    SetPromptText();
                }
            }
            else
            {
                ClearInteraction();
            }
        }
    }

    private void SetPromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = string.Format("<b>[E]</b> {0}", currentInteractable.GetInteractPrompt());
    }

    private void ClearInteraction()
    {
        currentInteractGameObject = null;
        currentInteractable = null;
        promptText.gameObject.SetActive(false);
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started && currentInteractable != null)
        {
            currentInteractable.OnInteract();
            ClearInteraction();
        }
    }
}

public interface IInteractable
{
    string GetInteractPrompt();
    void OnInteract();
}
