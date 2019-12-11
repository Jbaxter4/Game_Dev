using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [SerializeField]
    AbilitySystem.AbilityDatabase abilityDatabase;
    [SerializeField]
    WeaponAttributeDatabase weaponAttributeDatabase;
    public WeaponAttributeDatabase WeaponAttributeDatabase { get { return weaponAttributeDatabase; } }
    [SerializeField]
    WeaponDatabase weaponDatabase;
    public WeaponDatabase WeaponDatabase { get { return weaponDatabase; } }
    [ ColorUsage(false, true)]
    public Color[] rarityColors = new Color[5];
    private void Awake()
    {
        if (instance == null) instance = this;
    }
}
