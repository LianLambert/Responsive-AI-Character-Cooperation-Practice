using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Minotaur : HelperMethods
{
    [SerializeField] private GameObject treasure;
    [SerializeField] private GameObject attackSphere;
    [SerializeField] private NavMeshAgent agent;
    private readonly float maxDistanceFromTreasure = 35f;
    public readonly float attackRadius = 6f;
    private readonly float attackCooldown = 2f;
    private float attackCooldownTimer = 2f;
    private List<GameObject> knights = new List<GameObject>();
    public bool attackedThisFrame = false;
    public GameObject knightAttacking = null;
    private Animator myAnimator;

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
        // remove destroyed knights from the list
        knights.RemoveAll(knight => knight == null);

        // update cooldown
        if (attackCooldownTimer < attackCooldown)
        {
            attackCooldownTimer += Time.deltaTime;
        }

        // set the animator parameter
        myAnimator.SetBool("isMoving", agent.velocity.magnitude > 0.05f);

        // check for targets
        GameObject target = FindAttackTarget();
        
        // if there is a knight we should attack
        if (target != null)
        {
            // if we're not within attack range, move towards them
            if (Distance2D(target, this.gameObject) > attackRadius)
            {
                agent.SetDestination(target.transform.position);
            }
            // if we're in attack range, attack
            else
            {
                agent.SetDestination(this.transform.position);
                Attack();
            }
        }

        // if there is not a knight to attack and we are too far from the treasure, return to treasure
        else if (Distance2D(this.gameObject, treasure) > maxDistanceFromTreasure)
        {
            agent.SetDestination(treasure.transform.position);
        }

        // otherwise idle
        else
        {
            agent.SetDestination(this.gameObject.transform.position);
        }
    }

    // determine an attack target based on the following priorities:
        // 1) if a knight is attacking the minotaur/recently attacked, attack them
        // 2) if a knight has the treasure, attack them
        // 3) if a knight is visible and not too far from the treasure, attack them
    private GameObject FindAttackTarget()
    {
        if (knightAttacking != null)
        {
            if (attackedThisFrame)
            {
                myAnimator.SetTrigger("Take Damage");
                attackedThisFrame = false;
            }
            return knightAttacking;
        }
        else if (KnightWithTreasure() != null)
        {
            return KnightWithTreasure();
        }

        else if (ClosestVisibleKnight() != null)
        {
            return ClosestVisibleKnight();
        }

        return null;

    }

    private void Attack()
    {
        // can only attack if already cooled down
        if (attackCooldownTimer >= attackCooldown)
        {
            myAnimator.SetTrigger("Attack");
            attackCooldownTimer = 0f;
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator AttackSequence()
    {
        // activate the attackSphere for 1s
        attackSphere.SetActive(true);
        yield return new WaitForSeconds(1f);
        attackSphere.SetActive(false);
    }

    // if a knight has the treasure, returns the night, otherwise returns null
    public GameObject KnightWithTreasure()
    {

        foreach (GameObject knight in knights)
        {
            // have to accomodate for different scripts
            KnightMelee meleeKnight = knight.GetComponent<KnightMelee>();
            KnightDistance distanceKnight = knight.GetComponent<KnightDistance>();

            // Check if either script exists and has treasure
            if ((meleeKnight != null && meleeKnight.hasTreasure) || (distanceKnight != null && distanceKnight.hasTreasure))
            {
                return knight;
            }
        }

        return null;
    }

    // picks a knight to attack based on the closest visible knight
    private GameObject ClosestVisibleKnight()
    {
        GameObject closestVisibleKnight = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject knight in knights)
        {
            if (CanSeeKnight(knight))
            {
                float distance = Distance2D(this.gameObject, knight);

                // don't attack if it requires being too far from the treasure
                if (distance < closestDistance && Distance2D(treasure, knight) <= maxDistanceFromTreasure)
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

    public Vector3 FurthestCorner()
    {
        // find closest corner and head towards it
        float furthestDistance = float.MinValue;
        Vector3 furthestCornerPosition = Vector3.zero;

        foreach (Vector3 position in cornerPositions)
        {
            float distance = Vector3.Distance(this.transform.position, position);

            if (distance > furthestDistance)
            {
                furthestDistance = distance;
                furthestCornerPosition = position;
            }
        }

        return furthestCornerPosition;
    }
}
