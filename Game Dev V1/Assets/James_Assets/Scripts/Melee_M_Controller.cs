using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Melee_M_Controller : BaseEnemyMovement
{
    GameObject Player;
    PlayerStats PStats;
    Enemy_Stats EStats;
    private bool inSight;
    private Vector3 playerLastSighting;
    private bool recentSighting;
    private float distanceToTargetLastPos;
    private bool charging = false;
    private Vector3 chargeDestination;
    private float chargeDistance;
    private float chargeDistanceTravelled;
    public Vector3 chargeStartPoint;

    void Start()
    {
        EStats = GetComponent<Enemy_Stats>();
        EStats.MaxHealth = 125f;
        EStats.CurrentHealth = EStats.MaxHealth;
        EStats.MeleeDamage = 15f;
        EStats.MeleeAttackSpeed = 1f;
        EStats.MeleeAttackCooldown = 0f;
        EStats.SightRadius = 60f;
        EStats.SightSize = 30f;
        EStats.MovementSpeed = 3.5f;
        EStats.ChargeDamage = 25f;
        EStats.ChargeAttackSpeed = 10f;
        EStats.ChargeSpeed = 100f;
        EStats.ChargeCooldown = 0f;
        target = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 2;
        Player = GameObject.Find("Player");
        PStats = Player.GetComponent<PlayerStats>();
    }

    void Update()
    {
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
            agent.acceleration = 50f;
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

        //If enemy is aware of player it will either charge the player if the player is far away or move towards the player
        if ((inSight == true) && (PStats.Health > 0))
        {
            if ((distanceToTarget > 15) && (EStats.ChargeCooldown <= 0) && !charging)
            {
                SnapToTarget();
                Debug.Log("Medium Melee enemy is charging");
                charging = true;
                chargeDestination = target.position + (transform.forward * 2);
                chargeDistance = Vector3.Distance(transform.position, chargeDestination);
                chargeStartPoint = transform.position;
                Charge(chargeDestination);
                EStats.ChargeCooldown = 0;
                EStats.ChargeCooldown += EStats.ChargeAttackSpeed;
            }
            else if (!charging)
            {
                FaceTarget();
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(target.position, path))
                {
                    agent.SetDestination(target.position);
                }
            }
        }

        //If charge misses the player or an obstacle, stop charging
        if (charging)
        {
            chargeDistanceTravelled = Vector3.Distance(chargeStartPoint, transform.position);
            if (chargeDistanceTravelled > (chargeDistance - 2))
            {
                charging = false;
            }
        }
        else
        {
            agent.speed = 3.5f;
        }

        //Attack and charge cooldowns
        if (EStats.MeleeAttackCooldown > 0)
        {
            EStats.MeleeAttackCooldown -= Time.deltaTime;
        }
        if (EStats.ChargeCooldown > 0)
        {
            EStats.ChargeCooldown -= Time.deltaTime;
        }

        //If within range, attack player
        if ((distanceToTarget <= agent.stoppingDistance) && !charging)
        {
            if (EStats.MeleeAttackCooldown <= 0)
            {
                Attack();
                EStats.MeleeAttackCooldown = 0;
                EStats.MeleeAttackCooldown += EStats.MeleeAttackSpeed;
            }
        }

        //Destroy enemy if health falls to zero
        if (EStats.CurrentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    void Attack()
    {
        PStats.Health -= EStats.MeleeDamage;
        Debug.Log("Medium Melee Enemy Dealt " + EStats.MeleeDamage + " Damage");
        Debug.Log("Player Health is " + PStats.Health);
    }

    void Charge(Vector3 playerPosition)
    {
        agent.acceleration = 30f;
        agent.speed = EStats.ChargeSpeed;
        agent.SetDestination(playerPosition);
    }

    void OnCollisionEnter(Collision Other)
    {
        if (charging)
        {
            if (Other.collider.CompareTag("Player"))
            {
                PStats.Health -= EStats.ChargeDamage;
                Debug.Log("Medium Melee Enemy charged player and Dealt " + EStats.ChargeDamage + " Damage");
                Debug.Log("Player Health is " + PStats.Health);
                agent.velocity = Vector3.zero;
                target.GetComponent<Rigidbody>().AddForce(agent.velocity * 50f);
                charging = false;
            }            
        }
    }
}