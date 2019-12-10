using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_M_Stats : Enemy_Stats
{
    public float MaxHealth = 125f;
    public float MeleeDamage = 15f;
    public float ChargeDamage = 25f;
    public float ChargeAttackSpeed = 10f;
    public float ChargeSpeed = 100f;
    public float ChargeCooldown = 0f;
    public float MeleeAttackSpeed = 1f;
    public float MeleeAttackCooldown = 0f;
    public float SightRadius = 60f;
    public float SightSize = 30f;
    public float MovementSpeed = 3.5f;
}
