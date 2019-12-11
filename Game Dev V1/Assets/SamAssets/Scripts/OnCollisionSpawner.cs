using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionSpawner : MonoBehaviour
{
    [SerializeField]
    GameObject objectToSpawn;
    public System.Action<GameObject, GameObject> OnObjectSpawned;
    [SerializeField]
    bool destroyOnContact;

    public void OnCollide(Vector3 point)
    {
        GameObject GO = Instantiate(objectToSpawn, point, objectToSpawn.transform.rotation);
        OnObjectSpawned?.Invoke(GO, gameObject);
        if (destroyOnContact) Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollide(transform.position);
    }

}
