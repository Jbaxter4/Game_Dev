using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterClassManager : MonoBehaviour
{
    public static CharacterClassManager instance;

    [SerializeField]
    ClassTemplate characterClass;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;

        }
        else
        {
            Destroy(this);
        }
    }

    public ClassTemplate GetCharacterClass()
    {
        return characterClass;
    }
}
