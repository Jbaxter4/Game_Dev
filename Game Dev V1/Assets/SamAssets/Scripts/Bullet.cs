using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    protected Rigidbody rb;
    [SerializeField]
    protected float destroyRadius = 20f;
    [SerializeField]
    protected int damage;
    [SerializeField]
    protected ParticleSystem onCollideParticle;
    protected TeamManager owner;
    protected float bulletVelocity;

    protected Vector3 startPos;
    private void Start()
    {
        startPos = transform.position;
    }
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(rb.velocity);
        if(Vector3.Distance(startPos, transform.position) > destroyRadius)
        {
            Destroy(gameObject);
        }
    }
    public void SetInitialVelocity(Vector3 velocity)
    {
        rb.velocity = velocity;

    }
    public void Init(Vector3 velocity, int damage, TeamManager owner, int range = 10)
    {
        bulletVelocity = velocity.magnitude;
        rb.velocity = velocity;
        this.damage = damage;
        this.owner = owner;
        destroyRadius = range;
    }
   public virtual void OnBulletTrigger(Collider collision)
    {

        TeamManager teamManager = collision.gameObject.GetComponent<TeamManager>();
        if (teamManager)
        {
            if (teamManager.CanDamage(owner))
            {
                teamManager.Damagable.OnDamage(damage);
            }

        }
        Instantiate(onCollideParticle, transform.position, onCollideParticle.transform.rotation);
        Destroy(gameObject);
    }
    public virtual void OnBulletCollide()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject);
        TeamManager teamManager = collision.gameObject.GetComponent<TeamManager>();
        if (teamManager)
        {
            if (teamManager.CanDamage(owner))
            {
                teamManager.Damagable.OnDamage(damage);
            }

        }
        Instantiate(onCollideParticle, transform.position, onCollideParticle.transform.rotation);
        OnBulletCollide();
        Destroy(gameObject);
        
    }
    private void OnTriggerEnter(Collider other)
    {
        OnBulletTrigger(other);
    }
}
