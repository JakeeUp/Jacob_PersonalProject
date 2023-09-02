using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class EquipManager : MonoBehaviour
{

    //ADD A WEAPON MOVING BOB 
    public static EquipManager instance;
    public Equip currentEquip;
    public Transform equipParent;
    private PlayerController player;


    private void Awake()
    {
        instance = this;
        player = GetComponent<PlayerController>();
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && currentEquip != null && player.canLook == true)
        {
            currentEquip.OnAttackInput();
        }
    }
    public void OnAltAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && currentEquip != null && player.canLook == true)
        {
            currentEquip.OnAltAttackInput();
        }
    }

    public void EquipNew(ItemData item)
    {
        UnEquip();
        currentEquip = Instantiate(item.equipPrefab, equipParent).GetComponent<Equip>();
    }

    public void UnEquip()
    {
        if(currentEquip != null)
        {
            Destroy(currentEquip.gameObject);
            currentEquip = null;
        }
    }

    public void ForLoopEx()
    {
        

    }
}
