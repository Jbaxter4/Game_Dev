using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss_Controller : BaseEnemyMovement
{
    GameObject Player;
    PlayerStats PStats;
    Enemy_Stats EStats;
    private bool inSight;
    private Vector3 playerLastSighting;
    private bool recentSighting;
    private float distanceToTargetLastPos;
    public ParticleSystem Ground;
    public GameObject Bullet;

    void Start()
    {
        EStats = GetComponent<Enemy_Stats>();
        EStats.MaxHealth = 1000f;
        EStats.CurrentHealth = EStats.MaxHealth;
        EStats.MeleeDamage = 40f;
        EStats.RangeDamage = 25f;
        EStats.MeleeAttackSpeed = 1f;
        EStats.MeleeAttackCooldown = 0f;
        EStats.RangeAttackSpeed = 0.5f;
        EStats.RangeAttackCooldown = 0f;
        EStats.GroundPoundDamage = 15f;
        EStats.GroundPoundSpeed = 10f;
        EStats.GroundPoundCooldown = 0f;
        EStats.EarthShatterDamage = 30f;
        EStats.EarthShatterSpeed = 7f;
        EStats.EarthShatterCooldown = 0f;
        EStats.SightRadius = 60f;
        EStats.SightSize = 40f;
        EStats.Range = 30f;
        target = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 2;
        Player = GameObject.Find("Player");
        PStats = Player.GetComponent<PlayerStats>();
        
    }

    void Update()
    {
        Ground.Stop();
        int MeleeAttack = Random.Range(1, 10);
        int RangeAttack = Random.Range(1, 6);
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        angleToTarget = Vector3.Angle(transform.forward, target.transform.position - transform.position);
        distanceToTargetLastPos = Vector3.Distance(playerLastSighting, transform.position);

        //Check if the enemy can currently hear or see the player
        if ((distanceToTarget <= 5) || ((distanceToTarget <= sightSize) && (angleToTarget <= sightRadius)))
        {
            RaycastHit hit = new RaycastHit();
            bool detectionRay = Physics.Raycast(transform.position, target.position - transform.position, out hit);
            if (detectionRay)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    inSight = true;
                    recentSighting = true;
                    playerLastSighting = target.transform.position;
                }
                else
                {
                    inSight = false;
                }
            }
        }
        else
        {
            inSight = false;
        }

        //If enemy has not seen the player, wander around
        if (!inSight && !recentSighting)
        {
            if (isWandering == false)
            {
                StartCoroutine(Wander());
            }
            if (isRotatingRight == true)
            {
                transform.Rotate(transform.up * Time.deltaTime * RotationSpeed);
            }
            if (isRotatingLeft == true)
            {
                transform.Rotate(transform.up * Time.deltaTime * -RotationSpeed);
            }
            if (isWalking == true)
            {
                Vector3 moveTo = transform.position += transform.forward * MovementSpeed * Time.deltaTime;
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(moveTo, path))
                {
                    agent.SetDestination(moveTo);
                }
            }
        }
        //If enemy has seen player but lost sight of them, go to last know position of player
        else if (!inSight && recentSighting)
        {
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(playerLastSighting, path))
            {
                agent.SetDestination(playerLastSighting);
            }
            if (distanceToTargetLastPos < 2)
            {
                recentSighting = false;
            }
        }

        //Attacks cooldown
        if (EStats.MeleeAttackCooldown > 0)
        {
            EStats.MeleeAttackCooldown -= Time.deltaTime;
        }
        if (EStats.RangeAttackCooldown > 0)
        {
            EStats.RangeAttackCooldown -= Time.deltaTime;
        }
        if (EStats.EarthShatterCooldown > 0)
        {
            EStats.EarthShatterCooldown -= Time.deltaTime;
        }
        if (EStats.GroundPoundCooldown > 0)
        {
            EStats.GroundPoundCooldown -= Time.deltaTime;
        }

        //If enemy is aware of player it will face the player and it will move towards player
        if (inSight && (PStats.Health > 0))
        {
            FaceTarget();
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(target.position, path))
            {
                agent.SetDestination(target.position);
            }
            //If within melee range, attack player
            if ((distanceToTarget <= agent.stoppingDistance))
            {
                if (MeleeAttack == 9)
                {
                    if (EStats.GroundPoundCooldown <= 0)
                    {
                        GroundPound();
                        EStats.GroundPoundCooldown = 0;
                        EStats.GroundPoundCooldown += EStats.GroundPoundSpeed;
                    }
                }
                else
                {
                    if (EStats.MeleeAttackCooldown <= 0)
                    {
                        AttackMelee();
                        EStats.MeleeAttackCooldown = 0;
                        EStats.MeleeAttackCooldown += EStats.MeleeAttackSpeed;
                    }
                }
            }
            //If within range weapon range, attack player
            else if ((distanceToTarget > 5) && (distanceToTarget < EStats.Range))
            {
                if (EStats.RangeAttackCooldown <= 0)
                {
                    AttackRange();
                    EStats.RangeAttackCooldown = 0;
                    EStats.RangeAttackCooldown += EStats.RangeAttackSpeed;
                }
            }
        }

        if (EStats.CurrentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void AttackMelee()
    {
        PStats.Health -= EStats.MeleeDamage;
        Debug.Log("Boss Dealt " + EStats.MeleeDamage + " Damage");
        Debug.Log("Player Health is " + PStats.Health);
    }

    void GroundPound()
    {
        Ground.Play();
        PStats.Health -= EStats.GroundPoundDamage;
        target.GetComponent<Rigidbody>().AddForce(transform.forward * 500f);
        Debug.Log("Boss used Ground Pound and Dealt " + EStats.GroundPoundDamage + " Damage");
        Debug.Log("Player Health is " + PStats.Health);
    }

    void AttackRange()
    {
        GameObject bulletClone = Instantiate(Bullet, transform.GetChild(2).position, Quaternion.identity) as GameObject;
        Rigidbody bulletCloneRB = bulletClone.GetComponent<Rigidbody>();
        bulletCloneRB.AddForce(transform.forward * 1000);
        GameObject bulletCloneL = Instantiate(Bullet, transform.GetChild(3).position, Quaternion.identity) as GameObject;
        Rigidbody bulletCloneRBL = bulletCloneL.GetComponent<Rigidbody>();
        bulletCloneRBL.AddForce((transform.forward * 1000) - (transform.right * 100));
        GameObject bulletCloneR = Instantiate(Bullet, transform.GetChild(4).position, Quaternion.identity) as GameObject;
        Rigidbody bulletCloneRBR = bulletCloneR.GetComponent<Rigidbody>();
        bulletCloneRBR.AddForce((transform.forward * 1000) + (transform.right * 100));
    }
}