using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPortal : MonoBehaviour
{
    bool isBossAlive = true;
    [SerializeField] GameObject portal;

    // Update is called once per frame
    void Update()
    {
        if (!isBossAlive)
        {
            //spawnPortal
            Instantiate(portal, this.transform.position, Quaternion.identity);
        }
    }
}
