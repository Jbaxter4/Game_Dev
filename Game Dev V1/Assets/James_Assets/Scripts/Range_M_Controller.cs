using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Range_M_Controller : BaseEnemyMovement
{
    GameObject Player;
    PlayerStats PStats;
    Enemy_Stats EStats;
    private bool inSight;
    private Vector3 playerLastSighting;
    private bool recentSighting;
    private float distanceToTargetLastPos;
    public GameObject Bullet;

    void Start()
    {
        EStats = GetComponent<Enemy_Stats>();
        EStats.MaxHealth = 70f;
        EStats.CurrentHealth = EStats.MaxHealth;
        EStats.RangeDamage = 15f;
        EStats.RangeAttackSpeed = 1f;
        EStats.RangeAttackCooldown = 0f;
        EStats.SightRadius = 60f;
        EStats.SightSize = 60f;
        EStats.Range = 50f;
        Player = GameObject.FindGameObjectWithTag("Player");
        target = Player.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 2;
        PStats = Player.GetComponent<PlayerStats>();
        recentSighting = false;
    }

    void Update()
    {
        agent.speed = MovementSpeed;
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        distanceToTargetLastPos = Vector3.Distance(playerLastSighting, transform.position);
        angleToTarget = Vector3.Angle(transform.forward, target.transform.position - transform.position);

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

        //Check if the enemy can currently hear or see the player
        if ((distanceToTarget <= 5) || ((distanceToTarget <= EStats.SightSize) && (angleToTarget <= EStats.SightRadius)))
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
                else if (hit.collider.CompareTag("Bullet") || hit.collider.CompareTag("Range_M")) { }
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

        //Attack cooldown
        if (EStats.RangeAttackCooldown > 0)
        {
            EStats.RangeAttackCooldown -= Time.deltaTime;
        }

        //If enemy is aware of player it will face the player. If within range it attacks, if not it will move towards player
        if (inSight && (PStats.Health > 0))
        {
            FaceTarget();
            if (distanceToTarget <= EStats.Range)
            {
                if (EStats.RangeAttackCooldown <= 0)
                {
                    Attack();
                    EStats.RangeAttackCooldown = 0;
                    EStats.RangeAttackCooldown += EStats.RangeAttackSpeed;
                }
            }
            else
            {
                NavMeshPath path = new NavMeshPath();
                if (agent.CalculatePath(target.position, path))
                {
                    agent.SetDestination(target.position);
                }
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
        agent.ResetPath();
        GameObject bulletClone = Instantiate(Bullet, transform.GetChild(0).position, Quaternion.identity) as GameObject;
        Rigidbody bulletCloneRB = bulletClone.GetComponent<Rigidbody>();
        bulletCloneRB.AddForce(transform.forward * 2000);
    }
}