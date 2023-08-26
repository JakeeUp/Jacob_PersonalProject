using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData item;

    public string GetInteractPromp()
    {
        //show the name of item when our haircross is on the object
        return string.Format("pickup {0}", item.ItemName);
    }

    //public void OnInteract()
    //{
    //    //add item to inventory
    //    Inventory.instance.AddItem(item);
    //    //destroy game object after interacting with it
    //    Destroy(gameObject);
    //}
}
