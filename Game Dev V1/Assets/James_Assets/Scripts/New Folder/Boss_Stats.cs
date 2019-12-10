using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Stats : Enemy_Stats
{
    public float MaxHealth = 1000f;
    public float MeleeDamage = 40f;
    public float RangeDamage = 25f;
    public float MeleeAttackSpeed = 1f;
    public float MeleeAttackCooldown = 0f;
    public float RangeAttackSpeed = 0.5f;
    public float RangeAttackCooldown = 0f;
    public float GroundPoundDamage = 15f;
    public float GroundPoundSpeed = 10f;
    public float GroundPoundCooldown = 0f;
    public float EarthShatterDamage = 30f;
    public float EarthShatterSpeed = 7f;
    public float EarthShatterCooldown = 0f;
    public float SightRadius = 60f;
    public float SightSize = 40f;
    public float Range = 30f;
}
