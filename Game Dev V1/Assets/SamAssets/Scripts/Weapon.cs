using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class Weapon : SerializedScriptableObject
{
    [SerializeField, ReadOnly]
    int id;
    public WeaponTypes weaponType;
    public WeaponSpecificTypes specificType;
    public Rarity rarity;
    public GameObject model;
    public bool canReload;
    public bool automatic;
    public float reloadSpeed;
    public float attackSpeed;
    public int attackRange = 10;
    public int maxAmmo;
    public Sprite weaponSprite;
    protected TeamManager owner;
    [ReadOnly]
    public WeaponAttribute[] weaponAttributes;
    [ReadOnly]
    public SpecialWeaponAttribute specialWeaponAttribute;
    [ReadOnly]
    public Color[] colours = new Color[3];
    bool loaded;



    public AudioClip reload, fire;
    public void SetID(int id)
    {
        this.id = id;
    }
    public int GetID()
    {
        return id;
    }
    public void SetOwner(TeamManager newOwner)
    {
        owner = newOwner;
    }
    public virtual void OnFire()
    {

    }
    public virtual void OnLoad()
    {

    }
    public virtual void InitializeWeapon()
    {
        if (!loaded)
        {
            InitializeAttributes();
            for (int i = 0; i < colours.Length; i++)
            {
                colours[i] = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
        }
    }
    public virtual void OnEquipWeapon(TeamManager owner)
    {

    }
    [Button]
    public void InitializeAttributes()
    {
        

        weaponAttributes =  GameManager.instance.WeaponAttributeDatabase.GetWeaponAttributes(this);
        foreach(WeaponAttribute attribute in weaponAttributes)
        {
            attribute.InitializeAttribute();
        }
        if(rarity == Rarity.Legendary)
        {

            specialWeaponAttribute = GameManager.instance.WeaponAttributeDatabase.GetSpecialWeaponAttribute(this);
            specialWeaponAttribute.InitializeAttribute(this);
        }
    }
    public void LoadFromSave(WeaponSave save)
    {
        loaded = true;
        this.rarity = save.rarity;
        for (int i = 0; i < colours.Length; i++)
        {
            colours[i] = new Color(save.colours[i].r, save.colours[i].g, save.colours[i].b);
        }
        WeaponAttribute[] temp = new WeaponAttribute[save.attributes.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            
            temp[i] = new WeaponAttribute();
            temp[i].Create(save.attributes[i].type, save.attributes[i].value, this);
            temp[i].InitializeAttribute();
        }
        specialWeaponAttribute = GameManager.instance.WeaponAttributeDatabase.LoadSpecialWeaponAttribute(save.specialAttributeType);
        specialWeaponAttribute.InitializeAttribute(this);
        weaponAttributes = temp;
        OnLoad();
        
    }
    
    
}

public enum WeaponTypes {
    Gun
}
public enum WeaponSpecificTypes
{
    Pistol,
    Shotgun, 
    AssaultRifle,
    RocketLancher
}
public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}
