using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;


[CreateAssetMenu(fileName ="Weapon Database", menuName ="Create/Weapon Database")]
public class WeaponDatabase : ScriptableObject
{
    [SerializeField, ReadOnly]
    List<Weapon> weaponList;

#if UNITY_EDITOR
    [Button]
    public void UpdateDatabase()
    {
        Weapon[] weapons = Resources.LoadAll<Weapon>("Weapons");
        foreach(Weapon weapon in weapons)
        {
            if (!weaponList.Contains(weapon))
            {
                weapon.SetID(weaponList.Count);
                EditorUtility.SetDirty(weapon);
                weaponList.Add(weapon);
            }
        }
    }
    [Button]
    public void ClearDatabase()
    {
        weaponList = new List<Weapon>();
    }
#endif

    public Weapon GetWeaponByID(int id)
    {
         if(id < 0 || id >= weaponList.Count)
        {
            throw new System.Exception("trying to get a Weapon using an ID outside the range");
        }
        return ScriptableObject.Instantiate( weaponList[id]);
    }
}
