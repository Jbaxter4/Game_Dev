using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnCollisionHandler : MonoBehaviour
{
    public System.Action<GameObject, Collision> OnCollide;
    public System.Action<GameObject, Collider> OnTrigger;


    public void OnCollisionEnter(Collision collision)
    {
        OnCollide?.Invoke(gameObject, collision);
    }
    public void OnTriggerEnter(Collider other)
    {
        OnTrigger?.Invoke(gameObject, other);
    }
}
