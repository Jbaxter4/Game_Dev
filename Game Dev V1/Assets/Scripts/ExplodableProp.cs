using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodableProp : MonoBehaviour, IExplodable
{
    [SerializeField]
    float minVelocity, maxVelocity;
    [SerializeField]
    int breakableBits;
    [SerializeField]
    GameObject brick,explosion;
    [SerializeField]
    Rigidbody rb;
    public void OnExpload(Vector3 explosionPoint, float radius, float damage)
    {
        Destroy(gameObject);
        int layerMask = 1 << 8;
        Collider[] hitColliders = Physics.OverlapBox(explosionPoint, new Vector3(radius / 2, 2, radius / 2), Quaternion.identity, layerMask);
        Instantiate(explosion, explosionPoint, Quaternion.identity);
        foreach (Collider collision in hitColliders)
        {
            if(collision.tag == "Player")
            {
                //player takes 50 damage plz sam
            }
            if(collision.tag == "Enemy")
            {
                //Enemy_Stats stats = collision.gameObject.GetComponent<Enemy_Stats>();
                //stats.CurrentHealth -= 50;
                //damage enemy
            }
            Destroy(collision.gameObject);
            for (int i = 0; i < breakableBits; i++)
            {
                Vector3 pos = collision.gameObject.transform.position;
                pos.x +=Random.Range(-2, 2);
                pos.y+= Random.Range(0, 2);
                pos.z += Random.Range(-2, 2);
                GameObject obj = (GameObject)Instantiate(brick,pos , Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));
                //obj.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(minVelocity, maxVelocity), Random.Range(minVelocity, maxVelocity), Random.Range(minVelocity, maxVelocity));
                float lerpT = Vector3.Distance(pos, explosionPoint) / radius;
                float vel = Mathf.Lerp(maxVelocity, minVelocity, lerpT);

                Vector3 dir = pos - explosionPoint;
                obj.GetComponent<Rigidbody>().velocity = dir.normalized * vel;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            ContactPoint contact = collision.contacts[0];
            OnExpload(transform.position, 10, 50);
        }
    }
}
