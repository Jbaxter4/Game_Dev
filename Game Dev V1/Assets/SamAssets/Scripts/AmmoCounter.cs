using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AmmoCounter : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI ammoText;

    public static AmmoCounter instance;
    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void SetAmmoText(string text)
    {
        ammoText.text = text;
    }
}
