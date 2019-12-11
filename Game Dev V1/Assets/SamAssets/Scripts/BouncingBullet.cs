using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncingBullet : Bullet
{
    [SerializeField]
    int bounceLimit;
    [SerializeField]
    float radius;

    List<Collider> damagedColliders = new List<Collider>();

    public override void OnBulletTrigger(Collider collision)
    {
        TeamManager teamManager = collision.gameObject.GetComponent<TeamManager>();
        if (teamManager)
        {
            if (teamManager.CanDamage(owner) && !damagedColliders.Contains(collision))
            {
                teamManager.Damagable.OnDamage(damage);
                damagedColliders.Add(collision);
            }
            if(bounceLimit == 0)
            {
                Instantiate(onCollideParticle, transform.position, Quaternion.identity);
                Destroy(gameObject);
            }
            else
            {
                bounceLimit--;
                Collider[] collidersInRadius = Physics.OverlapSphere(transform.position, radius);
                TeamManager chosenEnemy = null;
                foreach(Collider col in collidersInRadius)
                {
                    TeamManager temp = col.GetComponent<TeamManager>();
                    if (temp && !damagedColliders.Contains(col))
                    {
                        if (temp.CanDamage(owner))
                        {
                            chosenEnemy = temp;
                        }
                    }
                }
                if (chosenEnemy != null)
                {
                    Vector3 enemyPoint = new Vector3(chosenEnemy.transform.position.x, transform.position.y, chosenEnemy.transform.position.z);
                    Vector3 dir = enemyPoint - transform.position;
                    dir.Normalize();
                    rb.velocity = dir * bulletVelocity;
                }
                else
                {
                    Instantiate(onCollideParticle, transform.position, Quaternion.identity);
                    Destroy(gameObject);
                }
            }

        }
        else
        {
            Instantiate(onCollideParticle, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
        
    }
}
