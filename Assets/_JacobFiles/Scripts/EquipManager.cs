using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class EquipManager : MonoBehaviour
{

    //ADD A WEAPON MOVING BOB 
    public static EquipManager instance;
    public Equip currentEquip;
    public Transform equipParent;
    private PlayerController player;
    public Image crosshair;

    public bool autoFire;

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
            autoFire = true;
        }
        if (context.phase == InputActionPhase.Canceled && currentEquip != null && player.canLook == true)
        {
            autoFire = false;
        }


    }
    public void OnAltAttackInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed && currentEquip != null && player.canLook == true)
        {
            currentEquip.OnAltAttackInput();
            //crosshair.enabled = false;
        }
        //else
        //    crosshair.enabled = true;
    }
    public void DisableCrosshairImage()
    {
        if (crosshair != null)
        {
            crosshair.enabled = false;
        }
    }
    public void EnableCrosshairImage()
    {
        if (crosshair != null)
        {
            crosshair.enabled = true;
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
