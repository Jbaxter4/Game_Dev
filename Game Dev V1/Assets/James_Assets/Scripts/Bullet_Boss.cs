using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_Boss : MonoBehaviour
{
    GameObject Player;
    PlayerStats PStats;
    GameObject Enemy;
    Enemy_Stats EStats;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        PStats = Player.GetComponent<PlayerStats>();
        Enemy = GameObject.FindGameObjectWithTag("Boss");
        EStats = Enemy.GetComponent<Enemy_Stats>();
    }

    void OnCollisionEnter(Collision Other)
    {
        if (Other.collider.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            PStats.Health -= EStats.RangeDamage;
            Debug.Log("Boss Dealt " + EStats.RangeDamage + " Damage");
            Debug.Log("Player Health is " + PStats.Health);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
