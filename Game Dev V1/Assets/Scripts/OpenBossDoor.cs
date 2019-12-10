using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenBossDoor : MonoBehaviour
{
    [SerializeField] GameObject brick;
    public int numberOfEnemies;
    // Update is called once per frame
    void Update()
    {
        if(numberOfEnemies < 5)
        {
            Destroy(gameObject);
        }
    }
}
