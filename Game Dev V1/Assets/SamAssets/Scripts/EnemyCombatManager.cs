using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(TeamManager))]
public class EnemyCombatManager : MonoBehaviour, IDamagable, IExplodable
{
    [SerializeField]
    GameObject rootObject;
    [SerializeField]
    int currentHealth;

    public bool isDead()
    {
        return currentHealth <= 0;
    }

    public void OnDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0) Destroy(rootObject);
    }

    public void OnExpload(Vector3 explosionPoint, float radius, float damage)
    {
        OnDamage((int)damage);
    }

    public void OnHeal(int healAmount)
    {
        currentHealth += healAmount;
    }
}
