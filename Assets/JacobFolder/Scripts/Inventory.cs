using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Events;
public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlots;       
    public ItemSlot[] slots;           
    public GameObject inventoryWindow; 
    public Transform dropPosition;

    [Header("Selected Items")]
    private ItemSlot selectedItem;      
    private int selectedItemIndex;    
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemdescription;
    public TextMeshProUGUI selectedItemStatName;
    public TextMeshProUGUI selectedItemStatValue;
    public GameObject useButton;
    public GameObject dropButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    private PlayerController controller;

    private int curEquipIndex;

    [Header("Events")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    //singelton
    public static Inventory instance;

    private void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
    }
}
public class ItemSlot
{
    public ItemData item;
    public int quantity;
}
