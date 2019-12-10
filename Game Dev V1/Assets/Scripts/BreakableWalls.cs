using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableWalls : MonoBehaviour 
{
    [SerializeField]
    float minVelocity, maxVelocity;
    [SerializeField]
    int breakableBits;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    bool isDoor;

    [SerializeField] GameObject brick;
    void OnCollisionEnter(Collision collision)
    {
        if (isDoor)
        {
            if (collision.gameObject.tag == "IExplodable" || collision.gameObject.tag == "Player")
            {
                Destroy(gameObject);
                ContactPoint contact = collision.contacts[0];
                Vector3 pos = contact.point;
                float x = pos.x, y = pos.y, z = pos.z;
                for (int i = 0; i < breakableBits; i++)
                {
                    GameObject obj = (GameObject)Instantiate(brick, pos, Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));
                    obj.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(minVelocity, maxVelocity), Random.Range(minVelocity, maxVelocity), Random.Range(minVelocity, maxVelocity));
                }
            }
        }
        else
        {
            if (collision.gameObject.tag == "IExplodable")
            {
                Destroy(gameObject);
                ContactPoint contact = collision.contacts[0];
                Vector3 pos = contact.point;
                float x = pos.x, y = pos.y, z = pos.z;
                for (int i = 0; i < breakableBits; i++)
                {
                    GameObject obj = (GameObject)Instantiate(brick, pos, Quaternion.Euler(new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360))));
                    obj.GetComponent<Rigidbody>().velocity = new Vector3(Random.Range(minVelocity, maxVelocity), Random.Range(minVelocity, maxVelocity), Random.Range(minVelocity, maxVelocity));

                }
            }
        }
    }
}
