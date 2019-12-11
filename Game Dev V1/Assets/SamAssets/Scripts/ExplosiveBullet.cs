using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBullet : Bullet
{
    [SerializeField]
    float explosiveRadius, explosiveDamage;
   
    public override void OnBulletTrigger(Collider collision)
    {
        StaticFunctions.CreateExplosion(transform.position, explosiveRadius, explosiveDamage, owner.GetTeam());
        base.OnBulletTrigger(collision);
    }
    public override void OnBulletCollide()
    {
        StaticFunctions.CreateExplosion(transform.position, explosiveRadius,explosiveDamage, owner.GetTeam());
        base.OnBulletCollide();
    }
}
