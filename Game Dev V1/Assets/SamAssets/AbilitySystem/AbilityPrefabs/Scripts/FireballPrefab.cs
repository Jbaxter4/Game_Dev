using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballPrefab : AbilityPrefab
{
    [SerializeField]
    float vel =20;
    [SerializeField]
    Bullet fireballPrefab;
	public override void OnUseAbilityPlayer(AbilityInfo info)
    {
        base.OnUseAbilityPlayer(info);
        Bullet bul = Instantiate(fireballPrefab, info.castBy.transform.position + (info.playerForward * 2), Quaternion.identity);
        bul.Init(info.playerForward * vel, (int)abilityInfo.damage, CharacterCombat.instance.TeamManager, (int)abilityInfo.range);
    }



}