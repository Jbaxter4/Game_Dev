using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class GameLoadSaveManager : MonoBehaviour
{
    public string characterName;


    [Button]
    public void Save()
    {
        LoadSave.CharacterName = characterName;
        LoadSave.Save();
    }
    [Button]
    public void Load()
    {
        LoadSave.CharacterName = characterName;
        GlobalSave save = LoadSave.Load();

        foreach(WeaponSave weaponSave in save.weaponSaves)
        {
            Weapon weapon = GameManager.instance.WeaponDatabase.GetWeaponByID(weaponSave.id);
            weapon.LoadFromSave(weaponSave);
            CharacterInventory.instance.PickupWeaponNoInventory(weapon, false);
        }
    }


}

