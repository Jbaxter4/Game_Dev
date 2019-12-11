using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void OnDamage(int damage);
    void OnHeal(int healAmount);
    bool isDead();
}
