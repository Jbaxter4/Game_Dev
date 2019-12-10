using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IExplodable
{
    void OnExpload(Vector3 explosionPoint, float radius, float damage);
}
