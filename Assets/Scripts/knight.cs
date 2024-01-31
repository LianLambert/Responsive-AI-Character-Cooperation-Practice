using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using TMPro;

public abstract class Knight : HelperMethods
{
    [SerializeField] public GameObject treasure;
    [SerializeField] public GameObject minotaur;
    [SerializeField] public NavMeshAgent agent;
    [SerializeField] public TMP_Text taskText;
    protected List<GameObject> otherKnights = new List<GameObject>();
    protected Animator myAnimator;
    protected float attackCooldown = 1;
    public float attackCooldownTimer = 1f;
    protected float treasurePickUpCooldown = 3;
    public float treasurePickUpCooldownTimer = 3f;
    protected int maxHits = 5;
    protected int hits = 0;
    public bool isThief = false;
    public bool hasTreasure = false;
    public bool attacked = false;
    public UnityEvent treasureDropped = new UnityEvent();
    public UnityEvent knightDied = new UnityEvent();
    public UnityEvent gameWon = new UnityEvent();
    
    public abstract bool CanAttack();
    public abstract void Attack();

    protected void Attacked()
    {
        // being attacked prevents attacking for 1s, which is also the length of the cooldown
        attackCooldownTimer = 0f;
        hits += 1;

        // if the knight has the treasure
        if (hasTreasure)
        {
            // they drop it and can't pick it up for 3s
            hasTreasure = false;
            treasurePickUpCooldown = 0f;
            treasureDropped.Invoke();

            // drop treasure
            treasure.transform.parent = null;
            treasure.transform.position = new Vector3(this.transform.position.x, 2.4f, this.transform.position.z);
            
        }

        // if the knight dies
        if (hits >= maxHits)
        {
            knightDied.Invoke();
            Destroy(gameObject);
        }
    }

    // if another knight died, update list of knights
    protected void OnOtherKnightDied()
    {
        otherKnights.RemoveAll(knight => knight == null);
    }

    // find closest corner to the knight
    public Vector3 ClosestCorner()
    {
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

        return closestCornerPosition;
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

    public void ExecuteTask(HTNCompositeTask task)
    {
        StartCoroutine(ExecuteTaskCoroutine(task));
    }

    private IEnumerator ExecuteTaskCoroutine(HTNCompositeTask task)
    {
        yield return StartCoroutine(task.ExecuteTask(this));
    }

}