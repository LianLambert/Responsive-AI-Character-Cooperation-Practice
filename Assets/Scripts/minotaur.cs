using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minotaur : HelperMethods
{
    [SerializeField] private GameObject treasure;
    [SerializeField] private GameObject attackSphere;
    [SerializeField] private NavMeshAgent agent;
    private Animator myAnimator;
    private List<GameObject> knights = new List<GameObject>();
    private float maxDistanceToTreasure = 40;
    private float attackRadius = 10;
    private int attackCooldown = 3;
    private float cooldownTimer = 3f;
    public GameObject knightAttacking = null;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("knight"))
        {
            knights.Add(knight);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(cooldownTimer < attackCooldown)
        {
            cooldownTimer += Time.deltaTime;
        }

        GameObject target = FindAttackTarget();
        
        // if there is a knight we should attack
        if (target != null)
        {
            Attack();
            // if we're not within attack range, move towards them
            if (distance2D(target, this.gameObject) > attackRadius)
            {
                agent.SetDestination(target.transform.position);
            }
            // if we're in attack range, attack
            else
            {
                Attack();
            }
        }

        // if there is not a knight we should attack
        agent.ResetPath();
    }

    // determine an attack target based on the following priorities:
          // 1) if a knight has the treasure, attack them
          // 2) if a knight is attacking the minotaur, attack them
          // 3) if a knight is visible and not too far from the treasure, attack them
    private GameObject FindAttackTarget()
    {
        if (KnightWithTreasure() != null)
        {
            return KnightWithTreasure();
        }

        else if (knightAttacking != null)
        {
            GameObject target = knightAttacking;
            knightAttacking = null;
            return target;
        }

        else if (KnightToAttack() != null)
        {
            return KnightToAttack();
        }

        return null;

    }

    private void Attack()
    {
        // can only attack if already cooled down
        if (cooldownTimer >= attackCooldown)
        {
            myAnimator.SetTrigger("Attack");
            cooldownTimer = 0f;
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator AttackSequence()
    {
        attackSphere.SetActive(true);
        yield return new WaitForSeconds(1f);
        attackSphere.SetActive(false);
    }

    // if a knight has the treasure, returns the night, otherwise returns null
    private GameObject KnightWithTreasure()
    {
        foreach (GameObject knight in knights)
        {
            // have to accomodate for different scripts
            MeleeKnight meleeKnight = knight.GetComponent<MeleeKnight>();
            DistanceKnight distanceKnight = knight.GetComponent<DistanceKnight>();

            // Check if either script exists and has treasure
            if ((meleeKnight != null && meleeKnight.hasTreasure) || (distanceKnight != null && distanceKnight.hasTreasure))
            {
                return knight;
            }
        }

        return null;
    }

    // picks a knight to attack based on the closest to the minotaur and not requiring the minotaur to leave the treasure
    private GameObject KnightToAttack()
    {
        GameObject closestVisibleKnight = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject knight in knights)
        {
            if (CanSeeKnight(knight))
            {
                float distance = distance2D(this.gameObject, knight);

                if (distance < closestDistance && distance <= maxDistanceToTreasure)
                {
                    closestDistance = distance;
                    closestVisibleKnight = knight;
                }
            }
        }

        return closestVisibleKnight;
    }

    // returns boolean indicating whether knight is visible
    private bool CanSeeKnight(GameObject knight)
    {
        // cast a ray
        Vector3 direction = knight.transform.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
        {
            // if the ray hits the knight, it is visible
            if (hit.collider.gameObject == knight)
            {
                return true;
            }
        }

        return false;
    }
}
