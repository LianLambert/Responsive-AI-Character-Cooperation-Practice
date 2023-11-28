using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Knight : HelperMethods
{
    [SerializeField] protected GameObject treasure;
    [SerializeField] protected GameObject minotaur;
    [SerializeField] protected GameObject minotaurAttackSphere;
    protected List<GameObject> otherKnights = new List<GameObject>();
    protected float attackCooldown = 1;
    protected float timeToGrabTreasure = 3;
    protected int maxHits = 5;
    protected int hits;
    protected int treasureDroppedCooldown = 3;
    public bool hasTreasure = false;
    public bool attacked = false;
    public UnityEvent knightDied = new UnityEvent();
    public UnityEvent gameWon = new UnityEvent();

    protected abstract bool CanAttackMinotaur();

    protected void Attacked()
    {
        hits += 1;
        if (hits >= maxHits)
        {
            Debug.Log("invoked knight died");
            knightDied.Invoke();
            gameObject.SetActive(false);
        }
    }
}
