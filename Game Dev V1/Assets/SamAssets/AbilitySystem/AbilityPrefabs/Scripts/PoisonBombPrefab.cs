using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisonBombPrefab : AbilityPrefab
{
    [SerializeField]
    ArcLauncher bomb;
    
	public override void OnUseAbilityPlayer(AbilityInfo info)
    {
        base.OnUseAbilityPlayer(info);
        ArcLauncher GO = Instantiate(bomb, info.castBy.transform.position + (info.playerForward * 2), bomb.transform.rotation);
        GO.SetTarget(info.hitPoint);
        GO.GetComponent<OnCollisionSpawner>().OnObjectSpawned += SetPoisionTeam;
        
    }

    public void SetPoisionTeam(GameObject poison, GameObject parentObject)
    {
        AoeEffect aoe = poison.GetComponent<AoeEffect>();
        aoe.SetTeam(TeamTypes.Friendly);
        aoe.SetValuePerTick(abilityInfo.damage);

        parentObject.GetComponent<OnCollisionSpawner>().OnObjectSpawned -= SetPoisionTeam;
    }

}