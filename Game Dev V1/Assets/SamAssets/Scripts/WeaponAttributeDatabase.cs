using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName ="New Attribute Database", menuName ="Create/Weapon/Attribute Database")]
public class WeaponAttributeDatabase : SerializedScriptableObject
{
    [SerializeField]
    WeaponAttributeShell[] weaponAttributes;
    [SerializeField]
    SpecialWeaponAttribute[] specialWeaponAttributes;

    public int GetAttributeAmount(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return 1;
            case Rarity.Uncommon:
                return 2;
            case Rarity.Rare:
                return 1;
            case Rarity.Epic:
                return 2;
            case Rarity.Legendary:
                return 3;
        }
        throw new System.Exception("Rarity of weapon is not in switch statment");
    }
    public WeaponAttribute[] GetWeaponAttributes(Weapon weapon)
    {
        WeaponAttribute[] temp = new WeaponAttribute[GetAttributeAmount(weapon.rarity)];
        for (int i = 0; i < temp.Length; i++)
        {
            Debug.Log("Finding");
            bool found = false;
            while (!found)
            {
                WeaponAttributeShell tempAttribute = weaponAttributes[Random.Range(0, weaponAttributes.Length)];
                if (tempAttribute.CanAttachAttribute(weapon, temp))
                {
                    WeaponAttribute attributeToAttach = new WeaponAttribute();
                    tempAttribute.ChooseValue(weapon, i);
                    attributeToAttach.Create(tempAttribute.AttributeType, tempAttribute.ChosenValue, weapon);
                    temp[i] = attributeToAttach;

                    found = true;
                }
            }
        }
        return temp;

    }
    public SpecialWeaponAttribute GetSpecialWeaponAttribute(Weapon weapon)
    {
        SpecialWeaponAttribute temp = null; 

        bool found = false;
        int breakAmount = 10;

        int currentBreakAmount = 0;
        while (!found)
        {
            temp = specialWeaponAttributes[Random.Range(0, specialWeaponAttributes.Length)];
            found = temp.CanAttachToWeapon(weapon);
            if (!found)
            {
                currentBreakAmount++;
            }
            if(currentBreakAmount >= breakAmount)
            {
                throw new System.Exception("Potential infinite loop stopped while selecting a special attribute");
            }
        }
        return temp;
    }
    public SpecialWeaponAttribute LoadSpecialWeaponAttribute(SpecialAttributeTypes type)
    {
        foreach( SpecialWeaponAttribute att in specialWeaponAttributes)
        {
            if (att.SpecialAttributeType == type) return att;
        }
        return null;
    }
}
