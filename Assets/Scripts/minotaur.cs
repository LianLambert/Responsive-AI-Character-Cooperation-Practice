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
    private float maxDistanceToTreasure = 35f;
    private float attackRadius = 6f;
    private float attackCooldown = 2f;
    private float cooldownTimer = 2f;
    public bool attackedThisFrame = false;
    public GameObject knightAttacking = null;

    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("knight"))
        {
            knight.GetComponent<Knight>().knightDied.AddListener(OnKnightDied);
            knights.Add(knight);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // update cooldown
        if(cooldownTimer < attackCooldown)
        {
            cooldownTimer += Time.deltaTime;
        }

        // Set the animator parameter
        myAnimator.SetBool("isMoving", agent.velocity.magnitude > 0.1f);

        GameObject target = FindAttackTarget();
        Debug.Log("target is null: " + target == null);
        
        // if there is a knight we should attack
        if (target != null)
        {
            // if we're not within attack range, move towards them
            if (Distance2D(target, this.gameObject) > attackRadius)
            {
                Debug.Log("moving towards target");
                agent.SetDestination(target.transform.position);
            }
            // if we're in attack range, attack
            else
            {
                Debug.Log("attacking");
                Attack();
            }
        }
        else
        {
            // if there is not a knight we should attack
            agent.SetDestination(transform.position);
        }
    }

    // determine an attack target based on the following priorities:
          // 1) if a knight has the treasure, attack them
          // 2) if a knight is attacking the minotaur, attack them
          // 3) if a knight is visible and not too far from the treasure, attack them
    private GameObject FindAttackTarget()
    {
        if (KnightWithTreasure() != null)
        {
            Debug.Log("found knight with treasure");
            return KnightWithTreasure();
        }

        else if (knightAttacking != null)
        {
            Debug.Log("found knight that is attacking");
            if (attackedThisFrame)
            {
                myAnimator.SetTrigger("Take Damage");
                attackedThisFrame = false;
            }
            return knightAttacking;
        }

        else if (KnightToAttack() != null)
        {
            Debug.Log("found knight in range");
            return KnightToAttack();
        }

        Debug.Log("returning null");
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

    // picks a knight to attack based on the closest visible knight to the minotaur
    // which does not require the minotaur to be too far from the treasure
    private GameObject KnightToAttack()
    {
        GameObject closestVisibleKnight = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject knight in knights)
        {
            if (CanSeeKnight(knight))
            {
                float distance = Distance2D(this.gameObject, knight);

                if (distance < closestDistance && Distance2D(treasure, knight) <= maxDistanceToTreasure)
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

    private void OnKnightDied()
    {
        knights = new List<GameObject>();
        GameObject[] knightsArray = GameObject.FindGameObjectsWithTag("knight");

        if (knightsArray != null && knightsArray.Length > 0)
        {
            foreach (GameObject knight in knightsArray)
            {
                knights.Add(knight);
            }
        }
    }
}
