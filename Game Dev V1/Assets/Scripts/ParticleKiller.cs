using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleKiller : MonoBehaviour
{
    ParticleSystem system;
    float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        system = GetComponent<ParticleSystem>();
        lifeTime = system.main.duration;
    }

    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0) Destroy(gameObject);
    }
}
