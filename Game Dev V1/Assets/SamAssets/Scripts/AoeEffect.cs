using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeEffect : MonoBehaviour
{
    public enum EffectType
    {
        Damage,
        Healing
    }
    [SerializeField]
    EffectType effectType;
    [SerializeField]
    float tickTime, valuePerTick, radius;
    [SerializeField]
    TeamTypes spawnedByTeam;
    float timer;
    [SerializeField]
    float duration = 10f;
    float currentDuration;
    public void SetTeam(TeamTypes team)
    {
        spawnedByTeam = team;
    }
    public void SetValuePerTick(float value)
    {
        valuePerTick = value;
    }
    private void Update()
    {
        currentDuration += Time.deltaTime;
        if (currentDuration >= duration) Destroy(gameObject);

        timer -= Time.deltaTime;
        if(timer <= 0)
        {

            if (effectType == EffectType.Healing) OnHealTick();
            else if (effectType == EffectType.Damage) OnDamageTick();
        }
    }
    void OnHealTick()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);

        foreach(Collider col in cols)
        {
            TeamManager team = col.GetComponent<TeamManager>();
            if (team != null)
            {
                if(team.GetTeam() == spawnedByTeam)
                {
                    team.Damagable.OnHeal((int)valuePerTick);
                }
            }
        }
        timer = tickTime;
    }
    void OnDamageTick()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, radius);

        foreach (Collider col in cols)
        {
            TeamManager team = col.GetComponent<TeamManager>();
            if (team != null)
            {
                if (team.CanDamage(spawnedByTeam))
                {
                    team.Damagable.OnDamage((int)valuePerTick);
                }
            }
        }
        timer = tickTime;
    }
}
