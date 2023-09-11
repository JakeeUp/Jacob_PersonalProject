using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable,
    Resource
}

public enum ConsumableType
{
    Hunger,
    Thirst,
    Health,
    Battery,
    pistolAmmo,
    assaultAmmo
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Information")]
    public string ItemName;
    public string itemDescription;
    public ItemType type;
    public Sprite icon;
    public GameObject dropPrefab;

    [Header("Stacking Items")]
    public bool canStack;
    public int maxStackAmount;

    [Header("Consumable")]
    public ItemDataConsumable[] consumable;

    [Header("Equip Items")]
    public GameObject equipPrefab;
}


[System.Serializable]
public class ItemDataConsumable
{
    public ConsumableType type;
    public float value;

}