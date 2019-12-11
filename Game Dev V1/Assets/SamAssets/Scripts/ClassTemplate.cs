using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Class", menuName ="Create/New Class")]
public class ClassTemplate : ScriptableObject
{

    public int startingHealth, healthPerLevel;
    public float moveSpeed;
    public Weapon[] equipableWeapons;
        
}
