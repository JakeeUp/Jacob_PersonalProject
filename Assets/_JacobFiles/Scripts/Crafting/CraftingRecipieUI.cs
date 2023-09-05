using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingRecipieUI : MonoBehaviour
{
    public CraftingRecipes recipe;
    public Image backgroundImage;
    public Image icon;
    public TextMeshProUGUI itemName;
    public Image[] resourceCosts;

    public Color canCraftColor, cannotCraftColor;
    private bool canCraft;

    private void Start()
    {
        icon.sprite = recipe.itemToCrafting.icon;
        itemName.text = recipe.itemToCrafting.ItemName;

        for (int i = 0; i < resourceCosts.Length; i++)
        {
            if (i < recipe.cost.Length)
            {
                resourceCosts[i].gameObject.SetActive(true);
                resourceCosts[i].sprite = recipe.cost[i].item.icon;
                resourceCosts[i].transform.GetComponentInChildren<TextMeshProUGUI>().text = recipe.cost[i].quantity.ToString();
            }
            else
            {
                resourceCosts[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnEnable()
    {
        UpdateCanCraft();
       
    }

    public void UpdateCanCraft()
    {
        canCraft = true;
        for (int i = 0; i < recipe.cost.Length; i++)
        {
            if (!Inventory.instance.HasItem(recipe.cost[i].item, recipe.cost[i].quantity))
            {
                canCraft = false;
                break;
            }
        }

        backgroundImage.color = canCraft ? canCraftColor : cannotCraftColor;

    }

    public void OnClickButton()
    {
        if(canCraft == true)
        {
            CraftingWindow.instance.Craft(recipe);
            Debug.Log("Pressing Button");
        }
    }
}
