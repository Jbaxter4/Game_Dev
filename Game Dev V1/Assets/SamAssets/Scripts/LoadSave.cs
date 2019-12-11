using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LoadSave
{




    public static string CharacterName;


    static GlobalSave CreateSaveObject()
    {
        GlobalSave globalSave = new GlobalSave();

        List<WeaponSave> weaponSaves = new List<WeaponSave>();
        foreach(Weapon weapon in CharacterCombat.instance.EquipedWeaponSlots)
        {
            if (weapon)
            {
                WeaponSave save = new WeaponSave();
                save.Init(weapon);
                weaponSaves.Add(save);
            }
        }
        globalSave.weaponSaves = weaponSaves.ToArray();
        globalSave.slottedAbilities = new int[CharacterAbilityManager.instance.SlottedAbilities.Length];
        for (int i = 0; i < globalSave.slottedAbilities.Length; i++)
        {
            globalSave.slottedAbilities[i] = CharacterAbilityManager.instance.SlottedAbilities[i].abilityInfo.ID;
        }
        return globalSave;
    }
    public static void Save()
    {

        GlobalSave objectToSave = CreateSaveObject();
        string path = Application.persistentDataPath + "/saves/";
        Debug.Log("Created");
        Directory.CreateDirectory(path);
        BinaryFormatter bf = new BinaryFormatter();
        using (FileStream fileStream = new FileStream(path +  "save.txt", FileMode.Create))
        {
            bf.Serialize(fileStream, objectToSave);
        }


    }
    public static GlobalSave Load()
    {
        string path = Application.persistentDataPath + "/saves/";

        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Open(path + "save.txt", FileMode.Open);
        
        GlobalSave save = (GlobalSave) bf.Deserialize(file);
        Debug.Log(save);
        return save;

        
       
    }
    public static bool SaveExists()
    {
        string path = Application.persistentDataPath + "/saves/";
        return File.Exists(path + "save.txt");
    }


}
[System.Serializable]
public class GlobalSave
{
   public WeaponSave[] weaponSaves;
    public int[] slottedAbilities;
}
[System.Serializable]
public class SerializableColour
{
    public float r, g, b;

    public void SetFromColour(Color colour)
    {
        r = colour.r;
        g = colour.g;
        b = colour.b;
    }
}

[System.Serializable]
public class WeaponSave
{
    public int id;
    

    public Rarity rarity;
   
    public  WeaponAttributeSave[] attributes;
    public SpecialAttributeTypes specialAttributeType;
    public SerializableColour[] colours;
   

    public void Init(Weapon weapon)
    {
        id = weapon.GetID();
        rarity = weapon.rarity;
        attributes = new WeaponAttributeSave[weapon.weaponAttributes.Length];
        colours = new SerializableColour[weapon.colours.Length];
        specialAttributeType = weapon.specialWeaponAttribute != null ? weapon.specialWeaponAttribute.SpecialAttributeType : SpecialAttributeTypes.None;
        for (int i = 0; i < colours.Length; i++)
        {
            colours[i] = new SerializableColour();
            colours[i].SetFromColour(weapon.colours[i]);
        }
        for (int i = 0; i < attributes.Length; i++)
        {
            attributes[i] = new WeaponAttributeSave();
            attributes[i].Init(weapon.weaponAttributes[i]);
        }
       
       
    }
}
[System.Serializable]
public class WeaponAttributeSave
{
    public AttributeTypes type;
    
    public float value;
    

    public void Init(WeaponAttribute attribute)
    {
        type = attribute.AttributeType;
        value = attribute.ChosenValue;
    }

}
