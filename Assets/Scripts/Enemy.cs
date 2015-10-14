using UnityEngine;
using System.Collections;
[RequireComponent (typeof(NavMeshAgent))]
public class Enemy : LivingEntity {
    
    public enum State { Idle, Chasing,Attacking};
    State currentState;

    NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;
    Material skinMaterial;
    Color originalColour;

    float attackDistanceThreshold = 1;
    float timeBetweenAttacks = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float damage = 1;
    float targetCollisionRadius;
    bool hasTarget;
	protected override void Start () {
        base.Start();
        
        pathfinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColour = skinMaterial.color;
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;
            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;
            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;
       
             StartCoroutine(UpdatePath());
        }
    }
    void OnTargetDeath() {
        hasTarget = false;
        currentState = State.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasTarget) { 
            if (Time.time > nextAttackTime) { 
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
               // if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold, 2)) //this freezes if next line is uncommented
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold +myCollisionRadius + targetCollisionRadius, 2))
                {

                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
               
                }
             }
        }
    }
    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathfinder.enabled = false;

        print("pathfinder set to false");
        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
         Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0.0f;

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;
      while (percent <= 1) {
            if(percent >= .5f && !hasAppliedDamage){
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed ;
            float interpolation = (-Mathf.Pow(percent,2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            yield return null;
        }
        currentState = State.Chasing;
        skinMaterial.color = originalColour;
        pathfinder.enabled = true;
        
    }
   IEnumerator UpdatePath() {
        float refreshRate = 0.25f;

        while (hasTarget){
            if (currentState == State.Chasing){
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
                //Vector3 targetPosition = new Vector3(target.position.x, 0, target.position.z);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
               
            }
            yield return new WaitForSeconds(refreshRate);
        }

    }
}
