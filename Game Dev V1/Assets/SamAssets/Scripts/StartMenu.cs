using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    GameObject continueButton;
    // Start is called before the first frame update
    void Start()
    {
        continueButton.SetActive(LoadSave.SaveExists());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
