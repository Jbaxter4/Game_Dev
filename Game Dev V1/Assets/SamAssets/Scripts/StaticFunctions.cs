using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticFunctions 
{
   public static void CreateExplosion(Vector3 pos, float explosionRadius, float damage,TeamTypes explosionBy)
    {
        Collider[] cols = Physics.OverlapSphere(pos, explosionRadius);

        foreach(Collider col in cols)
        {
            IExplodable explodable = col.GetComponent<IExplodable>();
            if (explodable != null) explodable.OnExpload(pos, explosionRadius, damage);
        }
    }
}
