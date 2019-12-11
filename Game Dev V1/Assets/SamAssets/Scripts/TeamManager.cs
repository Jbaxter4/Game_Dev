using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    [SerializeField]
    TeamTypes teamType;
   
    IDamagable damagable;
    public IDamagable Damagable { get { if (damagable == null) { damagable = GetComponent<IDamagable>(); } return damagable; } }


    public TeamTypes GetTeam()
    {
        return teamType;
    }
    public bool CanDamage(TeamTypes attacker)
    {
        return (attacker != teamType || teamType == TeamTypes.Neutral);
    }
    public bool CanDamage(TeamManager attacker)
    {
        return (attacker.GetTeam() != teamType || teamType == TeamTypes.Neutral);
    }
}

public enum TeamTypes
{
    Friendly,
    Enemy,
    Neutral
}