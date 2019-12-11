using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.UI;
public class InventorySlot : MonoBehaviour
{
    [SerializeField, ReadOnly]
    int slot;
    [SerializeField]
    Image icon;

    public void SetSlot(int slot)
    {
        this.slot = slot;
        
    }
    public void UpdateIcon()
    {
       icon.sprite =  CharacterInventory.instance.GetWeaponInSlot(slot).weaponSprite;
    }
}
