using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using TMPro;

public class CharacterInventory : SerializedMonoBehaviour
{

    public static CharacterInventory instance;

    [SerializeField]
    Weapon startWeapon;
    [SerializeField, ReadOnly]
    int gold;
    [SerializeField, ReadOnly]
    int skillShards;
    [SerializeField, ReadOnly]
    Weapon[] weaponsInInventory;
    [SerializeField]
    int inventorySize = 10;
    [SerializeField]
    InventorySlot[] inventorySlots;
    [SerializeField, ReadOnly]
    WeaponDrop hoveredWeapon;
    float keyHoldTime;
    [SerializeField]
    float timeToTrigger;
    bool holding;
    bool pickedUp;

    [SerializeField]
    InventorySlot slotPrefab;
    [SerializeField]
    Transform inventorySlotParent;
    [SerializeField]
    TextMeshProUGUI weaponText;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    private void Start()
    {
        weaponsInInventory = new Weapon[inventorySize];
        Weapon weapon = ScriptableObject.Instantiate(startWeapon);
        weapon.rarity = Rarity.Common;
        

        weapon.InitializeWeapon();
        PickupWeaponNoInventory(weapon, true);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F) && hoveredWeapon != null)
        {
            pickedUp = false;
            holding = true;
        }
        if (Input.GetKey(KeyCode.F) && hoveredWeapon != null)
        {
            if (holding) keyHoldTime += Time.deltaTime;
        }
        if (Input.GetKeyUp(KeyCode.F) && !pickedUp)
        {
            keyHoldTime = 0;
            holding = false;
            keyHoldTime = 0;
            pickedUp = true;
            if (keyHoldTime < timeToTrigger)
            {
                hoveredWeapon.PickUpDropNoInventory(false);
            }
        }
        if (keyHoldTime >= timeToTrigger && !pickedUp)
        {
            Debug.Log("Picking up and equipping");
            holding = false;
            pickedUp = true;
            keyHoldTime = 0;
            hoveredWeapon.PickUpDropNoInventory(true);
        }
    }
    public void SetHoveredWeapon(WeaponDrop drop)
    {
        weaponText.gameObject.SetActive(true);
        weaponText.text = "[F] Pick Up " + drop.Weapon.specificType;
        hoveredWeapon = drop;
    }
    public void ClearHoveredWeapon(WeaponDrop drop)
    {
        if (hoveredWeapon == drop)
        {
            hoveredWeapon = null;
            weaponText.gameObject.SetActive(false);
        }
    }
    public Weapon GetWeaponInSlot(int slot)
    {
        return weaponsInInventory[slot];
    }
    [Button]
    public void CreateInventoryUi()
    {
        inventorySlots = new InventorySlot[inventorySize];
        for (int i = 0; i < inventorySize; i++)
        {
            InventorySlot slot = Instantiate(slotPrefab, inventorySlotParent);
            slot.SetSlot(i);
            inventorySlots[i] = slot;


        }
    }
    public void ClearHoveredWeapon(WeaponDrop drop, bool clearOnlyIfMatch)
    {
        if (clearOnlyIfMatch)
        {
            if (hoveredWeapon == drop) hoveredWeapon = null;
        }
        else
        {
            hoveredWeapon = null;
        }
    }
    [Button]
    public bool PickupWeapon(Weapon weapon, bool equipOnPickup)
    {
        for (int i = 0; i < weaponsInInventory.Length; i++)
        {
            if (weaponsInInventory[i] == null)
            {

                weaponsInInventory[i] = weapon;
                inventorySlots[i].UpdateIcon();
                weapon.SetOwner(CharacterCombat.instance.TeamManager);
                if (equipOnPickup || CharacterCombat.instance.EquipedWeapon == null)
                {
                    bool freeSlot = false;
                    for (int x = 0; x < 3; x++)
                    {
                        if (CharacterCombat.instance.IsSlotEmptyAndUnlocked(x))
                        {
                            if (!freeSlot) CharacterCombat.instance.SetWeaponInSlotAndEquip(weapon, x);
                            freeSlot = true;
                        }
                    }
                    if (!freeSlot) CharacterCombat.instance.SetWeaponInCurrentSlotAndEquip(weapon);
                }
                else
                {
                    for (int x = 0; x < 3; x++)
                    {
                        if (CharacterCombat.instance.IsSlotEmptyAndUnlocked(x))
                        {
                            CharacterCombat.instance.SetWeaponInSlot(x, weapon);
                        }
                    }
                }
                return true;
            }
        }
        Debug.Log("<size=20>Inventory Full!</size>");
        return false;
    }

    public bool PickupWeaponNoInventory(Weapon weapon, bool equipOnPickup)
    {
        if (!equipOnPickup)
        {
            for (int x = 0; x < 3; x++)
            {
                if (CharacterCombat.instance.IsSlotEmptyAndUnlocked(x))
                {
                    if (CharacterCombat.instance.EquipedWeapon == null)
                    {
                        CharacterCombat.instance.SetWeaponInSlotAndEquip(weapon, x);
                    }
                    else
                    {
                        CharacterCombat.instance.SetWeaponInSlot(x, weapon);
                    }
                    return true;
                }
            }
        }
        else
        {
            for (int x = 0; x < 3; x++)
            {
                if (CharacterCombat.instance.IsSlotEmptyAndUnlocked(x))
                {

                    CharacterCombat.instance.SetWeaponInSlotAndEquip(weapon, x);
                    return true;



                }
            }


            CharacterCombat.instance.SetWeaponInCurrentSlotAndEquip(weapon);

            return true;
        }
        return false;
    }

}
