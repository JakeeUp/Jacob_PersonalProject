using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
    public static CraftingWindow instance;
    public CraftingRecipieUI[] recipeUIs;
    public GameObject inventoryPanel;
    private void Awake()
    {
        instance = this;
    }

    private void OnEnable()
    {
        Inventory.instance.onOpenInventory.AddListener(OnOpenInventory);
    }

    private void OnDisable()
    {
        Inventory.instance.onOpenInventory.RemoveListener(OnOpenInventory);
    }

    public void OnOpenInventory()
    {
        gameObject.SetActive(false);
        inventoryPanel.SetActive(true);
    }

    public void OnOpenCraft()
    {
        gameObject.SetActive(true);
        inventoryPanel.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        PlayerController.instance.canLook = false;
    }

    public void OnCloseCraft()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        PlayerController.instance.canLook = true;
    }

    public void Craft(CraftingRecipes recipe)
    {
        for (int i = 0; i < recipe.cost.Length; i++)
        {
            for (int x = 0; x < recipe.cost[i].quantity; x++)
            {
                Inventory.instance.RemoveItem(recipe.cost[i].item);
            }
        }

        Inventory.instance.AddItem(recipe.itemToCrafting);

        for (int i = 0; i < recipeUIs.Length; i++)
        {
            recipeUIs[i].UpdateCanCraft();
        }
         
    }
}
