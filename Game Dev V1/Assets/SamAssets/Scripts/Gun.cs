using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "New Gun", menuName = "Create/Weapon/Gun")]
public class Gun : Weapon
{
    public int level = 1;
    public float projectileSpeed;
    public int projectileDamagePerLevel;
    [ReadOnly]
    public int projectileDamage;
    public int bulletsPerSameShot =1;
    public int bulletsPerBurst = 1;
    public float spread;
    public float burstSpeed;
    public Bullet bulletPrefab;
    Transform bulletSpawnLocation;
    public override void OnFire()
    {
        
            for (int i = 0; i < bulletsPerSameShot; i++)
            {
                Bullet bullet = Instantiate(bulletPrefab, bulletSpawnLocation.transform.position, bulletPrefab.transform.rotation);
                Vector3 dir = bulletSpawnLocation.forward + Vector3.right * Random.Range(-spread, spread);
                dir.Normalize();
                bullet.Init(dir * projectileSpeed, projectileDamage, owner, attackRange);
            }
        
        
    }

    public override void InitializeWeapon()
    {
        
        projectileDamage = projectileDamagePerLevel * level;
        
        base.InitializeWeapon();

    }
    public override void OnLoad()
    {
        projectileDamage = projectileDamagePerLevel * level;
        
    }
    public override void OnEquipWeapon(TeamManager equipBy)
    {
        owner = equipBy;
        GameObject weaponModel = Instantiate(model, CharacterCombat.instance.WeaponSlot);
        weaponModel.transform.localPosition = Vector3.zero;
        bulletSpawnLocation = weaponModel.GetComponent<GunModel>().BulletSpawnLocation;
        weaponModel.GetComponent<ColourRandomizer>().SetColours(colours);
    }
}
