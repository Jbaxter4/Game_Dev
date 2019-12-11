using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    [SerializeField]
    ParticleSystem attachedSystem;
    [SerializeField]
    float leeway;
    float deathTimer;

    private void Start()
    {
        if (attachedSystem == null) attachedSystem = GetComponent<ParticleSystem>();
        deathTimer = attachedSystem.main.duration + leeway;
    }
    private void Update()
    {
        deathTimer -= Time.deltaTime;
        if (deathTimer <= 0) Destroy(gameObject);
    }

}
