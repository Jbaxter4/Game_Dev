using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployTurretPrefab : AbilityPrefab
{
    [SerializeField]
    Turret turretPrefab;
	public override void OnUseAbilityPlayer(AbilityInfo info)
    {
        base.OnUseAbilityPlayer(info);
        Turret turret = Instantiate(turretPrefab, info.hitPoint, Quaternion.identity);
        turret.damagePerBullet = abilityInfo.damage;
        turret.deployedBy = CharacterCombat.instance.TeamManager;
        
    }



}