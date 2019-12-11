using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookPrefab : AbilityPrefab
{

    [SerializeField]
    GameObject hookObject;
    Vector3 startPos, endPos;
    [SerializeField]
    float hookSpeed;
    GameObject CurrentHookObject;
    bool returning;
    TeamManager castBy;
    LineRenderer currentLineRenderer;
    bool launching;
    float launchTimer, returnTimer;

    public override void OnUseAbilityPlayer(AbilityInfo info)
    {
        
        base.OnUseAbilityPlayer(info);
        castBy = info.team;
        CurrentHookObject = Instantiate(hookObject, info.castBy.transform.position + info.playerForward, Quaternion.identity);
        CurrentHookObject.transform.LookAt(info.castBy.transform.position + (info.playerForward * 5));
        CurrentHookObject.GetComponent<Rigidbody>().velocity = CurrentHookObject.transform.forward * hookSpeed;
        startPos = CurrentHookObject.transform.position;
        currentLineRenderer = CurrentHookObject.GetComponent<LineRenderer>();
        endPos = CurrentHookObject.transform.position + CurrentHookObject.transform.forward * abilityInfo.range;
        OnCollisionHandler collisionHandler = CurrentHookObject.GetComponent<OnCollisionHandler>();
        collisionHandler.OnCollide+=( OnHookCollide);
        collisionHandler.OnTrigger+=( OnHookTrigger);
        returning = false;
        launching = false;
    }
    public void OnHookCollide(GameObject hookObj, Collision col)
    {
        returning = true;
        returnTimer = Vector3.Distance(CurrentHookObject.transform.position, startPos) / ((startPos - endPos).normalized * hookSpeed).magnitude;
        hookObj.GetComponent<Rigidbody>().velocity = (startPos - hookObj.transform.position).normalized * hookSpeed;
    }
    public void OnHookTrigger(GameObject hookObj, Collider col)
    {
        returning = true;
        TeamManager damagable = col.GetComponent<TeamManager>();
        if(damagable && damagable.CanDamage(castBy))
        {
            hookObj.GetComponent<Rigidbody>().isKinematic = true;
            hookObj.transform.parent = col.transform;
            ArcLauncher launcher = col.GetComponentInParent<ArcLauncher>();
            launcher.SetTarget(startPos);
            LaunchInfo info = launcher.Launch();
            launchTimer = info.time;
            launching = true;
            returning = false;
        }
        
    }
    private void Update()
    {
        if(CurrentHookObject != null)
        {
            currentLineRenderer.SetPosition(0, CurrentHookObject.transform.position);
            currentLineRenderer.SetPosition(1, castBy.transform.position);
            if(Vector3.Distance(CurrentHookObject.transform.position, endPos) <= 0.1f && !returning)
            {
                CurrentHookObject.GetComponent<Rigidbody>().velocity = (startPos - endPos).normalized * hookSpeed;
                returnTimer = Vector3.Distance(CurrentHookObject.transform.position, startPos) / ((startPos - endPos).normalized * hookSpeed).magnitude;
                returning = true;
            }
            if(returning)
            {
                returnTimer -= Time.deltaTime;
                if (returnTimer <= 0)
                {
                    returning = false;
                    Destroy(CurrentHookObject);
                }
            }
            if (launching)
            {
                launchTimer -= Time.deltaTime;
                if (launchTimer <= 0) Destroy(CurrentHookObject);
            }
        }
    }

}