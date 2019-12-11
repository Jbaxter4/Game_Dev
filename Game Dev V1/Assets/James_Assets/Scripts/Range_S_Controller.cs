using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Range_S_Controller : BaseEnemyMovement
{
    GameObject Player;
    PlayerStats PStats;
    Enemy_Stats EStats;
    private bool inSight;
    private bool fearful;
    private Vector3 playerLastSighting;
    private bool recentSighting;
    private float distanceToTargetLastPos;
    public GameObject Bullet;

    void Start()
    {
        EStats = GetComponent<Enemy_Stats>();
        EStats.MaxHealth = 50f;
        EStats.CurrentHealth = EStats.MaxHealth;
        EStats.RangeDamage = 10f;
        EStats.RangeAttackSpeed = 1f;
        EStats.RangeAttackCooldown = 0f;
        EStats.SightRadius = 60f;
        EStats.SightSize = 45f;
        EStats.Range = 20f;
        EStats.HealthRegenCooldown = 0f;
        EStats.HealthRegenRate = 2f;
        Player = CharacterCombat.instance.gameObject;
        target = Player.transform;
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = 2;
        PStats = Player.GetComponent<PlayerStats>();
        fearful = false;
        recentSighting = false;
    }

    void Update()
    {
        agent.speed = MovementSpeed;
        distanceToTarget = Vector3.Distance(target.position, transform.position);
        distanceToTargetLastPos = Vector3.Distance(playerLastSighting, transform.position);
        angleToTarget = Vector3.Angle(transform.forward, target.transform.position - transform.position);

        //If enemy has not seen the player, wander around
        if (!inSight && !recentSighting && !fearful)
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
        else if (!inSight && recentSighting && !fearful)
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

        //If low on health, enemy is fearful of player
        if (EStats.CurrentHealth <= EStats.MaxHealth/10)
        {
            fearful = true;
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
                else if (hit.collider.CompareTag("Bullet")) { }
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
        
        //Attack and HealthRegen cooldowns
        if (EStats.RangeAttackCooldown > 0)
        {
            EStats.RangeAttackCooldown -= Time.deltaTime;
        }
        if (EStats.HealthRegenCooldown > 0)
        {
            EStats.HealthRegenCooldown -= Time.deltaTime;
        }

        //If enemy is aware of player and not fearful it will face the player. If within range it attacks, if not it will move towards player
        if (inSight && (PStats.Health > 0) && (!fearful))
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
        //If enemy is fearful it will run away to heal. The enemy will stop being fearful once health is above 50%, but won't stop healing unless aware of player again
        else if (fearful)
        {
            if ((distanceToTarget < 20f) && (EStats.CurrentHealth <= EStats.MaxHealth/2))
            {
                runAway();
            }
            else if (inSight && (EStats.CurrentHealth >= EStats.MaxHealth / 2))
            {
                fearful = false;
            }
            else if (EStats.HealthRegenCooldown <= 0)
            {
                EStats.HealthRegenCooldown = 0f;
                EStats.HealthRegenCooldown += EStats.HealthRegenRate;
                EStats.CurrentHealth += 5;
                Debug.Log("Small Range Enemy Recovered 5 Health");
                Debug.Log("Small range Health is " + EStats.CurrentHealth);
                if (EStats.CurrentHealth >= EStats.MaxHealth)
                {
                    EStats.CurrentHealth = EStats.MaxHealth;
                    fearful = false;
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
        bulletCloneRB.AddForce(transform.forward * 1000);
        bulletCloneRB.rotation = transform.rotation;
    }

    void runAway()
    {
        agent.speed = 2f;
        transform.rotation = Quaternion.LookRotation(transform.position - target.position);
        Vector3 runDistance = transform.position + transform.forward * 3f;
        NavMesh.SamplePosition(runDistance, out NavMeshHit hit, 5, 1 << NavMesh.GetAreaFromName("Walkable"));
        NavMeshPath path = new NavMeshPath();
        if (agent.CalculatePath(hit.position, path))
        {
            agent.SetDestination(hit.position);
        }
    }
}