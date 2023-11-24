using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class knight : helperMethods
{
    [SerializeField] protected GameObject treasure;
    [SerializeField] protected GameObject minotaur;
    protected List<GameObject> otherKnights;
    protected float speed;
    protected float attackCooldown = 1;
    protected float timeToGrabTreasure = 3;
    protected int maxHits = 5;
    protected int hits;
    protected int treasureDroppedCooldown = 3;
    protected helperMethods helper = new helperMethods();
    public bool hasTreasure = false;

    // Start is called before the first frame update
    void Start()
    {
        hits = 0;
        speed = minotaur.GetComponent<minotaur>().speed * 0.75f;
        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("knight"))
        {
            if (knight != this)
            {
                otherKnights.Add(knight);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected abstract bool CanAttackMinotaur();
}
