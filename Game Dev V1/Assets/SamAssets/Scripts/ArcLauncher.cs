using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.Events;

public class ArcLauncher : MonoBehaviour
{
    [SerializeField]
    Vector3 target;
    [SerializeField]
    float addedHeight = 2;
    float height;
    float g;
    [SerializeField]
    Rigidbody rb;
    [SerializeField]
    bool kinematicAtEnd;
    bool launching;
    [SerializeField]
    bool launchOnStart;
    float timer;
    
   
    // Start is called before the first frame update
    void Start()
    {
        g = Physics.gravity.y;
        if(launchOnStart)Launch();
    }
    private void Update()
    {
        if (launching)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                launching = false;
                if (kinematicAtEnd)
                {
                    rb.isKinematic = true;
                        
                }
            }
        }
    }
    public void SetTarget(Vector3 newTarget)
    {
        target = newTarget;
       
    }
    [Button]
    public LaunchInfo Launch()
    {
        LaunchInfo info = GetLaunchInfo();
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.velocity = info.initialVelocity ;
        launching = true;
        return info;

    }
    public LaunchInfo GetLaunchInfo()
    {
        LaunchInfo info = new LaunchInfo();
        height = target.y + addedHeight;
        float displacementY = target.y - transform.position.y;
        Vector3 displacementXZ = new Vector3(target.x - transform.position.x, 0, target.z - transform.position.z);
        float time = Mathf.Sqrt(-2 * height / g) + Mathf.Sqrt(2 * (displacementY - height) / g);
        timer = time;
        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * g * height);
        Vector3 velocityXY = displacementXZ / time;
        info.time = time;
        info.initialVelocity = velocityXY + velocityY;
        return info;
    }

   
}
public class LaunchInfo
{
    public float time;
    public Vector3 initialVelocity;
}

