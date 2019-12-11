using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_M : MonoBehaviour
{
    GameObject Player;
    PlayerStats PStats;
    GameObject Enemy;
    Enemy_Stats EStats;

    void Start()
    {
        Enemy = GameObject.FindGameObjectWithTag("Range_M");
        EStats = Enemy.GetComponent<Enemy_Stats>();
    }
    public void Init(Enemy_Stats stats)
    {
        EStats = stats;
    }
    void OnCollisionEnter(Collision Other)
    {
        if (Other.collider.CompareTag("Player"))
        {
            Destroy(this.gameObject);
            //PStats.Health -= EStats.RangeDamage;
            CharacterCombat.instance.OnDamage((int)EStats.RangeDamage);
            Debug.Log("Medium Enemy Dealt " + EStats.RangeDamage + " Damage");
            Debug.Log("Player Health is " + PStats.Health);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
