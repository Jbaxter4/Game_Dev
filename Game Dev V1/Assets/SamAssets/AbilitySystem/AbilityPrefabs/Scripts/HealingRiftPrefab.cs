using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingRiftPrefab : AbilityPrefab
{
    [SerializeField]
    AoeEffect healingArea;
    [SerializeField]
    LayerMask hitMask;
    

	public override void OnUseAbilityPlayer(AbilityInfo info)
    {
        base.OnUseAbilityPlayer(info);

        Ray ray = new Ray(info.castBy.transform.position, Vector3.down);
        RaycastHit hit = new RaycastHit();
        if(Physics.Raycast(ray, out hit, 50f, hitMask))
        {
            Debug.Log("HIT");
            AoeEffect area = Instantiate(healingArea, hit.point, healingArea.transform.rotation);
            area.SetTeam(CharacterCombat.instance.TeamManager.GetTeam());
            area.SetValuePerTick(abilityInfo.damage);
        }
    }



}