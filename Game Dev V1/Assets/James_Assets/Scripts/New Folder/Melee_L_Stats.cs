using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee_L_Stats : Enemy_Stats
{
    public float MaxHealth = 200f;
    public float MeleeDamage = 20f;
    public float MeleeAttackSpeed = 1f;
    public float MeleeAttackCooldown = 0f;
    public float GroundPoundDamage = 15f;
    public float GroundPoundSpeed = 10f;
    public float GroundPoundCooldown = 0f;
    public float SightRadius = 60f;
    public float SightSize = 30f;
    public bool DamageResistant = false;
}
