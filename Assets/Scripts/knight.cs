using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public abstract class Knight : HelperMethods
{
    [SerializeField] protected GameObject treasure;
    [SerializeField] protected GameObject minotaur;
    [SerializeField] protected NavMeshAgent agent;
    protected List<Vector3> cornerPositions = new List<Vector3>
    {
        new Vector3(2f, 0.5f, 2f),
        new Vector3(2f, 0.5f, 48f),
        new Vector3(78f, 0.5f, 2f),
        new Vector3(78f, 0.5f, 48f)
    };
    protected List<GameObject> otherKnights = new List<GameObject>();
    protected Animator myAnimator;
    protected float attackCooldown = 1;
    protected float attackCooldownTimer = 1f;
    protected float treasurePickUpCooldown = 3;
    protected float treasurePickUpCooldownTimer = 3f;
    protected int maxHits = 5;
    protected int hits;
    protected int treasureDroppedCooldown = 3;
    public bool hasTreasure = false;
    public bool attacked = false;
    public UnityEvent knightDied = new UnityEvent();
    public UnityEvent gameWon = new UnityEvent();
    
    protected abstract bool CanAttack();
    protected abstract void Attack();

    protected void Attacked()
    {
        // being attacked prevents attacking for 1s, which is also the length of the cooldown
        attackCooldownTimer = 0f;
        hits += 1;

        // if the knight has the treasure, they drop it
        if (hasTreasure)
        {
            treasurePickUpCooldown = 0f;
            treasure.transform.position = this.transform.position;
            treasure.SetActive(true);
            hasTreasure = false;
        }

        // if the knight dies
        if (hits >= maxHits)
        {
            knightDied.Invoke();
            Destroy(gameObject);
        }
    }

    protected void OnOtherKnightDied()
    {
        // if another knight died, update list of knights
        otherKnights.RemoveAll(knight => knight == null);

        // replan!
    }

    protected void RunWithTreasure()
    {
        // find closest corner and head towards it
        float closestDistance = float.MaxValue;
        Vector3 closestCornerPosition = Vector3.zero;
        
        foreach (Vector3 position in cornerPositions)
        {
            float distance = Vector3.Distance(this.transform.position, position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestCornerPosition = position;
            }
        }

        agent.SetDestination(closestCornerPosition);
    }

    protected void UpdateTimers()
    {
        // update attack cooldown
        if (attackCooldownTimer < attackCooldown)
        {
            attackCooldownTimer += Time.deltaTime;
        }

        // update treasure cooldown
        if (treasurePickUpCooldownTimer < treasurePickUpCooldown)
        {
            treasurePickUpCooldownTimer += Time.deltaTime;
        }
    }
}