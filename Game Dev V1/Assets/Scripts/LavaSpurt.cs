using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaSpurt : MonoBehaviour
{
    [SerializeField]
    GameObject lavaEffect;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0,100) == 50)
        {
            lavaSpurt();
        }
    }
    void lavaSpurt()
    {
        Vector3 pos = gameObject.transform.position;
        pos.y = 3;
        pos.x -= .5f;
        pos.z -= .5f;
        
        Instantiate(lavaEffect, pos, Quaternion.identity);
    }
}
