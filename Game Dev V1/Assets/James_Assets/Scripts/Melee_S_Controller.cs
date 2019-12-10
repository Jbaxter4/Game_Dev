using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Melee_S_Controller : BaseEnemyMovement
{ 
    GameObject Player;
    PlayerStats PStats;
    Enemy_Stats EStats;
    private bool inSight;
    private Vector3 playerLastSighting;
    private bool recentSighting;
    private float distanceToTargetLastPos;

    void Start()
    {
        EStats = GetComponent<Enemy_Stats>();
        EStats.MaxHealth = 50f;
        EStats.CurrentHealth = EStats.MaxHealth;
        EStats.MeleeDamage = 10f;
        EStats.MeleeAttackSpeed = 1f;
        EStats.MeleeAttackCooldown = 0f;
        EStats.SightRadius = 60f;
        EStats.SightSize = 30f;
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
                    SnapToTarget();
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

        //If enemy is aware of player it will move towards the player
        if ((inSight == true) && (PStats.Health > 0))
        {
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(target.position, path))
            {
                agent.SetDestination(target.position);
            }
            FaceTarget();
        }

        //Attack cooldown
        if (EStats.MeleeAttackCooldown > 0)
        {
            EStats.MeleeAttackCooldown -= Time.deltaTime;
        }

        //If within range, attack player
        if ((distanceToTarget <= agent.stoppingDistance))
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
        Debug.Log("Small Enemy Dealt " + EStats.MeleeDamage + " Damage");
        Debug.Log("Player Health is " + PStats.Health);
    }
}