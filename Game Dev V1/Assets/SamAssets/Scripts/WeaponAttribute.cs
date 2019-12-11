using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class WeaponAttributeShell
{

    [SerializeField, FoldoutGroup("$title")]
    string title;
    [SerializeField, FoldoutGroup("$title")]

    AttributeTypes attributeType;
    public AttributeTypes AttributeType { get { return attributeType; } }
    [SerializeField, FoldoutGroup("$title")]
    List<WeaponSpecificTypes> specifyWeaponType;
    [SerializeField, FoldoutGroup("$title")]
    WeaponAttributeDataSet[] dataSets;
    [SerializeField, ReadOnly, FoldoutGroup("$title")]
    float chosenValue;
    public float ChosenValue { get { return chosenValue; } }


    public bool CanAttachAttribute(Weapon weaponToAttachTo, WeaponAttribute[] currentAttributes)
    {

        bool hasAttributeType = false;
        foreach (WeaponAttribute attribute in currentAttributes)
        {
            if (attribute != null)
            {
                if (attribute.AttributeType == attributeType) hasAttributeType = true;
            }
        }
        bool specificWeapon = specifyWeaponType == null || specifyWeaponType.Count == 0 || specifyWeaponType.Contains(weaponToAttachTo.specificType);
        return specificWeapon && !hasAttributeType;
    }

    public float GetWeaponValue(Weapon weapon)
    {
        Gun gun = weapon as Gun;
        switch (attributeType)
        {
            case AttributeTypes.Damage:
                
                if(gun != null)
                {
                    return gun.projectileDamage;
                }
                else
                {
                    throw new System.Exception("Weapon Type Not Implamented");
                }
            case AttributeTypes.AmmoPerClip:
                
                if (gun != null)
                {
                    return gun.maxAmmo;
                }
                else
                {
                    throw new System.Exception("Weapon Type Not Implamented");
                }
            case AttributeTypes.FireRate:
                
                if (gun != null)
                {
                    return gun.attackSpeed;
                }
                else
                {
                    throw new System.Exception("Weapon Type Not Implamented");
                }
            case AttributeTypes.ReloadSpeed:
                
                if (gun != null)
                {
                    return gun.reloadSpeed;
                }
                else
                {
                    throw new System.Exception("Weapon Type Not Implamented");
                }
            case AttributeTypes.Spread:

                if (gun != null)
                {
                    return gun.spread;
                }
                else
                {
                    throw new System.Exception("Weapon Type Not Implamented");
                }
            case AttributeTypes.Range:

                if (gun != null)
                {
                    return gun.attackRange;
                }
                else
                {
                    throw new System.Exception("Weapon Type Not Implamented");
                }
        }
        throw new System.Exception("Attribute Type not handled");
    }
    public void ChooseValue(Weapon weapon, int i)
    {
        float value = GetWeaponValue(weapon);
        foreach(WeaponAttributeDataSet dataSet in dataSets)
        {
            if (dataSet.CorrectValueSet(value))
            {
                dataSet.ChooseValue(weapon.rarity, i);
                chosenValue = dataSet.ChosenValue;

                return;
            }

        }
        throw new System.Exception("weapon attribute of type " + attributeType + " is too high");
    }
   



}
[System.Serializable]
public class WeaponAttributeDataSet
{
    public float valueThreshHold =  float.MaxValue;
    [FoldoutGroup("DataSet"), Title("Common"), SerializeField]
    Vector2 commonNegativeRange;
    [FoldoutGroup("DataSet"), Title("Uncommon"), SerializeField]
    Vector2 uncommonNegativeRange;
    [FoldoutGroup("DataSet"), SerializeField]
    Vector2 uncommonPositiveRange;
    [FoldoutGroup("DataSet"), Title("Rare"), SerializeField]
    Vector2 rarePositiveRange;
    [FoldoutGroup("DataSet"), Title("Epic"), SerializeField]
    Vector2 epicPositiveRange;
    [FoldoutGroup("DataSet"), Title("Legendary"), SerializeField]
    Vector2 legendaryPositiveRange;
    float chosenValue;
    public float ChosenValue { get { return chosenValue; } }

    public void ChooseValue(Rarity rarity, int attributeIndex)
    {
        
        switch (rarity)
        {
            case Rarity.Common:

                chosenValue = 1 + (Random.Range(commonNegativeRange.x, commonNegativeRange.y) * 0.01f);
                break;
            case Rarity.Uncommon:
                if (attributeIndex == 0) chosenValue = 1 + (Random.Range(uncommonNegativeRange.x, uncommonNegativeRange.y) * 0.01f);
                else chosenValue = 1 + (Random.Range(uncommonPositiveRange.x, uncommonPositiveRange.y) * 0.01f);
                break;
            case Rarity.Rare:
                chosenValue = 1 + (Random.Range(rarePositiveRange.x, rarePositiveRange.y) * 0.01f);
                break;
            case Rarity.Epic:
                chosenValue = 1 + (Random.Range(epicPositiveRange.x, epicPositiveRange.y) * 0.01f);
                break;
            case Rarity.Legendary:
                chosenValue = 1 + (Random.Range(legendaryPositiveRange.x, legendaryPositiveRange.y) * 0.01f);
                break;

        }
    }

    public bool CorrectValueSet(float value)
    {
        return value <= valueThreshHold;
    }
}
[System.Serializable]
public class WeaponAttribute
{
    [SerializeField, ReadOnly]
    AttributeTypes attributeType;
    public AttributeTypes AttributeType { get { return attributeType; } }
    [SerializeField, ReadOnly]
    float chosenValue;
    public float ChosenValue { get { return chosenValue; } }
    [SerializeField, ReadOnly]
    Weapon attachedWeapon;
    

   
    public void Create(AttributeTypes attributeType, float chosenValue, Weapon attachedWeapon)
    {
        this.attributeType = attributeType;
        this.chosenValue = chosenValue;
        this.attachedWeapon = attachedWeapon;
    }
    
    public void InitializeAttribute()
    {
        switch (attributeType)
        {
            case AttributeTypes.None:
                break;

            case AttributeTypes.Damage:
                if (attachedWeapon.weaponType == WeaponTypes.Gun)
                {
                    Gun gun = attachedWeapon as Gun;
                    float damageFloat = gun.projectileDamage;
                    damageFloat *= chosenValue;
                    gun.projectileDamage = (int)damageFloat;
                }
                break;

            case AttributeTypes.AmmoPerClip:
                if (attachedWeapon.weaponType == WeaponTypes.Gun)
                {
                    Gun gun = attachedWeapon as Gun;
                    float ammoPerClip = gun.maxAmmo;
                    ammoPerClip *= chosenValue;
                    gun.maxAmmo = (int)ammoPerClip;
                }
                break;
            case AttributeTypes.FireRate:
                if (attachedWeapon.weaponType == WeaponTypes.Gun)
                {
                    Gun gun = attachedWeapon as Gun;
                    float attackSpeed = 1 - chosenValue;
                    attackSpeed = 1 + attackSpeed;

                    gun.attackSpeed *= attackSpeed;
                }
                break;
            case AttributeTypes.ReloadSpeed:
                if (attachedWeapon.weaponType == WeaponTypes.Gun)
                {
                    Gun gun = attachedWeapon as Gun;
                    float reloadSpeed = 1 - chosenValue;
                    reloadSpeed = 1 + reloadSpeed;

                    gun.reloadSpeed *= reloadSpeed;
                }
                break;
            case AttributeTypes.Spread:
                if (attachedWeapon.weaponType == WeaponTypes.Gun)
                {
                    Gun gun = attachedWeapon as Gun;
                    float spread = 1 - chosenValue;
                    spread = 1 + spread;

                    gun.spread *= spread;
                }
                break;
            case AttributeTypes.Range:
                if (attachedWeapon.weaponType == WeaponTypes.Gun)
                {
                    Gun gun = attachedWeapon as Gun;
                    float range = gun.attackRange;
                    range *= chosenValue;

                    gun.attackRange = (int)range;
                }
                break;



        }
    }
}
[System.Serializable]
public class SpecialWeaponAttribute
{
    [SerializeField, ShowIf("PrefabShowIf")]
    GameObject prefabOverride;
    [SerializeField]
    SpecialAttributeTypes specialAttributeType;
    public SpecialAttributeTypes SpecialAttributeType { get { return specialAttributeType;} }
    [SerializeField]
    List<WeaponSpecificTypes> includedWeaponTypes;

    public bool PrefabShowIf()
    {
        return specialAttributeType != SpecialAttributeTypes.None;
    }

    public bool CanAttachToWeapon(Weapon weapon)
    {
        return includedWeaponTypes == null || includedWeaponTypes.Count == 0 || includedWeaponTypes.Contains(weapon.specificType);
    }

    public void InitializeAttribute(Weapon weapon)
    {
        switch (specialAttributeType)
        {
            case (SpecialAttributeTypes.BouncingBullet):
                Gun gun = (Gun)weapon;
                gun.bulletPrefab = prefabOverride.GetComponent<Bullet>();
                break;

            default:
                break;
        }
    }
}
public enum SpecialAttributeTypes
{
    None,
    BouncingBullet,

}
public enum AttributeTypes
{
    None,
    Damage,
    AmmoPerClip,
    FireRate,
    ReloadSpeed,
    Spread,
    Range,
    BouncingBullet
}
