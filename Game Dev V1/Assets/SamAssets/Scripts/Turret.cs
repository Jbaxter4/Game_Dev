using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    
    public TeamManager deployedBy;

    [SerializeField]
    GameObject turretHead;
    bool lockedOn = false;
    [SerializeField]
    float turretRadius =10f;
    float startUpTime = 1.5f;
    TeamManager currentTarget;

    [Header("Rotation When Idle")]
    [SerializeField]
    float rotationSpeed;
    [SerializeField]
    float rotationBreakTime;
    [SerializeField]
    float currentRotationTimer;
    [SerializeField]
    List<GameObject> testList = new List<GameObject>();
    List<IDamagable> enemiesInRange = new List<IDamagable>();
    [Header("Combat Settings")]
    [SerializeField]
    float lifeTime, attackSpeed, velocity;
    [SerializeField]
    Transform barrelEnd;
    [SerializeField]
    Bullet bulletToSpawn;
    float currentAttackTimer;
    public float damagePerBullet;
    // Start is called before the first frame update
    void Start()
    {
        Collider[] cols = Physics.OverlapSphere(transform.position, turretRadius);

        foreach(Collider col in cols)
        {
            LookForEnemies(col);
        }
        
    }
   
    private void OnTriggerEnter(Collider other)
    {
        LookForEnemies(other);
    }
    public void LookForEnemies(Collider col)
    {
        TeamManager team = col.GetComponent<TeamManager>();
        if (team != null)
        {
            if (team.CanDamage(deployedBy))
            {
                enemiesInRange.Add(team.Damagable);
                testList.Add(col.gameObject);
                if (currentTarget == null) currentTarget = team;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (startUpTime > 0) startUpTime -= Time.deltaTime;
        else
        {
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0)
            {
                Destroy(gameObject);
            }
            if (currentTarget == null)
            {

                currentRotationTimer -= Time.deltaTime;
                if (currentRotationTimer <= 0)
                {
                    turretHead.transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
                    if (currentRotationTimer <= -1f)
                    {
                        currentRotationTimer = rotationBreakTime;
                    }
                }
            }
            else
            {
                currentAttackTimer -= Time.deltaTime;
                if (currentAttackTimer <= 0)
                {
                    currentAttackTimer = attackSpeed;
                    Bullet bul = Instantiate(bulletToSpawn, barrelEnd.position, Quaternion.identity);
                    bul.Init(barrelEnd.forward * velocity, (int)damagePerBullet, deployedBy, (int)turretRadius);
                }
                turretHead.transform.LookAt(new Vector3(currentTarget.transform.position.x, turretHead.transform.position.y, currentTarget.transform.position.z));
            }
        }
    }
}
