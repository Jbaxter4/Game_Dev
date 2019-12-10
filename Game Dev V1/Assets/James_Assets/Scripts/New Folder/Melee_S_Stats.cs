using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_S_Stats : Enemy_Stats
{
    public float CurrentHealth;
    public float MaxHealth = 50f;
    public float MeleeDamage = 10f;
    public float MeleeAttackSpeed = 1f;
    public float MeleeAttackCooldown = 0f;
    public float SightRadius = 60f;
    public float SightSize = 30f;
}