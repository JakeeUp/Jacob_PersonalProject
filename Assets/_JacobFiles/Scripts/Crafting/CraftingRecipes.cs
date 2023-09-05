using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crafting Recipe", menuName = "New Recipe")]
public class CraftingRecipes : ScriptableObject
{
    public ItemData itemToCrafting;
    public ResourceCost[] cost;
}

[System.Serializable]
public class ResourceCost
{
    public ItemData item;
    public int quantity;
}
