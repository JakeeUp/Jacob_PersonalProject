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
    private PlayerAttributes attr;
    

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
        attr = GetComponent<PlayerAttributes>();
    }
    

    private void Start()
    {
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uiSlots.Length];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = new ItemSlot();
            uiSlots[i].index = i;
            uiSlots[i].ClearSlot();
        }

    }

  
    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }

    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
            onCloseInventory.Invoke();
            controller.ToggleCursor(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
            onOpenInventory.Invoke();
            ClearSelectedItemWindow();
            controller.ToggleCursor(true);
        }
    }

    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    public void AddItem(ItemData item)
    {
        if (item.canStack)
        {
            ItemSlot slotToStackTo = GetItemStack(item);

            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }

        ThrowItem(item);
    }

    void ThrowItem(ItemData item)
    {
        GameObject droppedItem = Instantiate(item.dropPrefab, dropPosition.position, dropPosition.rotation);
        Rigidbody itemRigidbody = droppedItem.GetComponent<Rigidbody>();
        if (itemRigidbody != null)
        {
            Vector3 pushDirection = dropPosition.forward + Vector3.up * 0.2f; 
            itemRigidbody.AddForce(pushDirection.normalized * 200f); 
        }
    }

    void UpdateUI()
    {
        for (int x = 0; x < slots.Length; x++)
        {
            if (slots[x].item != null)
            {
                uiSlots[x].Set(slots[x]);
            }
            else 
            {
                uiSlots[x].ClearSlot();
            }
        }
    }

    ItemSlot GetItemStack(ItemData item)
    {
        for (int x = 0; x < slots.Length; x++)
        {
            if (slots[x].item == item && slots[x].quantity < item.maxStackAmount)
            {
                return slots[x];
            }
        }
        return null;
    }

    ItemSlot GetEmptySlot()
    {
        for (int x = 0; x < slots.Length; x++)
        {
            if (slots[x].item == null)
            {
                return slots[x];
            }
        }
        return null;
    }


    //HELP ME GOD 
    public void SelectItem(int index)
    {
        if (slots[index].item == null)
            return;
        selectedItem = slots[index];
        selectedItemIndex = index;
        selectedItemName.text = selectedItem.item.ItemName;
        selectedItemdescription.text = selectedItem.item.itemDescription;

        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        for (int x = 0; x < selectedItem.item.consumable.Length; x++)
        {
            selectedItemStatName.text += selectedItem.item.consumable[x].type.ToString() + "\n";
            selectedItemStatValue.text += selectedItem.item.consumable[x].value.ToString() + "\n";
        }
        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        unequipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }

    void ClearSelectedItemWindow()
    {
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemdescription.text = string.Empty;
        selectedItemStatName.text = string.Empty;
        selectedItemStatValue.text = string.Empty;

        dropButton.SetActive(false);
        useButton.SetActive(false);
        unequipButton.SetActive(false);
        equipButton.SetActive(false);
    }

    public void OnUseButton()
    {
        if (selectedItem.item.type == ItemType.Consumable)
        {
            for (int x = 0; x < selectedItem.item.consumable.Length; x++)
            {
                switch (selectedItem.item.consumable[x].type)
                {
                   case ConsumableType.Health: attr.Heal(selectedItem.item.consumable[x].value); break;
                   case ConsumableType.Hunger: attr.Eat(selectedItem.item.consumable[x].value); break;
                   case ConsumableType.Thirst: attr.Drink(selectedItem.item.consumable[x].value); break;
                    case ConsumableType.assaultAmmo: AmmoManager.instance.ReloadAssault(selectedItem.item.consumable[x].value); break;
                    case ConsumableType.pistolAmmo: AmmoManager.instance.ReloadPistol(selectedItem.item.consumable[x].value); break;
                }

                //if(tools != null && tools.assaultType == true)
                //{
                //    switch(selectedItem.item.consumable[x].type)
                //    {
                //        case ConsumableType.assaultAmmo: tools.AssaultReload(selectedItem.item.consumable[x].value); break;
                //        default:return;

                //    }
                //}else if(tools != null && tools.pistolType == true)
                //{
                //    switch (selectedItem.item.consumable[x].type)
                //    {
                //        case ConsumableType.pistolAmmo: tools.PistolReload(selectedItem.item.consumable[x].value); break;


                //        default:return;
                //    }
                //}
                //else
                //{
                //    return;
                //}


            }

        }
        RemoveSelectedItem();
    }

    public void OnEquipButton()
    {
        if (uiSlots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }

        uiSlots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        EquipManager.instance.EquipNew(selectedItem.item);
        UpdateUI();
        SelectItem(selectedItemIndex);
    }

    public void OnUnequipButton()
    {
        UnEquip(selectedItemIndex);
    }
    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }

    private void UnEquip(int index)
    {
        uiSlots[index].equipped = false;
        EquipManager.instance.UnEquip();
        UpdateUI();

        if(selectedItemIndex == index)
        {
            SelectItem(index);
        }
    }

    void RemoveSelectedItem()
    {
        selectedItem.quantity--;

        if (selectedItem.quantity == 0)
        {
            if (uiSlots[selectedItemIndex].equipped == true)
                UnEquip(selectedItemIndex);

            selectedItem.item = null;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }
    public void RemoveItem(ItemData item)
    {

    }
    public bool HasItem(ItemData item , int quantity)
    {
        return false;
    }


}
public class ItemSlot
{
    public ItemData item;
    public int quantity;
}
