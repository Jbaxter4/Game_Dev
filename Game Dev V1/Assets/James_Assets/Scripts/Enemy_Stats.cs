using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy_Stats : MonoBehaviour, IDamagable, IExplodable
{
    public float CurrentHealth;
    public float MaxHealth;
    public float MeleeDamage;
    public float RangeDamage;
    public float MeleeAttackSpeed;
    public float MeleeAttackCooldown;
    public float RangeAttackSpeed;
    public float RangeAttackCooldown;
    public float SightRadius;
    public float SightSize;
    public float Range;
    public float HealthRegenCooldown;
    public float HealthRegenRate;
    public float ChargeDamage;
    public float ChargeAttackSpeed;
    public float ChargeSpeed;
    public float ChargeCooldown;
    public float MovementSpeed;
    public float GroundPoundDamage;
    public float GroundPoundSpeed;
    public float GroundPoundCooldown;
    public bool DamageResistant;
    public float EarthShatterDamage;
    public float EarthShatterSpeed;
    public float EarthShatterCooldown;

    UnityEvent onDeath;

    public void OnDeath()
    {
        onDeath?.Invoke();
        LevelDesign.instance.numberOfEnemies--;
        Destroy(gameObject);
    }
    public void OnDamage(int damage)
    {
        CurrentHealth -= damage;
        if(CurrentHealth <= 0)
        {
            OnDeath();
        }
    }

    public void OnHeal(int healAmount)
    {
        CurrentHealth += healAmount;
    }

    public bool isDead()
    {
        return CurrentHealth <= 0;
    }

    public void OnExpload(Vector3 explosionPoint, float radius, float damage)
    {
        CurrentHealth -= damage;
    }
}
