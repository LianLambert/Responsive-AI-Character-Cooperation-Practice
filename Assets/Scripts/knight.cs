using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Knight : HelperMethods
{
    [SerializeField] protected GameObject treasure;
    [SerializeField] protected GameObject minotaur;
    [SerializeField] protected GameObject minotaurAttackSphere;
    protected List<GameObject> otherKnights;
    protected float attackCooldown = 1;
    protected float timeToGrabTreasure = 3;
    protected int maxHits = 5;
    protected int hits;
    protected int treasureDroppedCooldown = 3;
    public bool hasTreasure = false;
    public bool attacked = false;
    public UnityEvent knightDied = new UnityEvent();
    public UnityEvent gameWon = new UnityEvent();

    // Start is called before the first frame update
    void Start()
    {
        hits = 0;
        foreach (GameObject knight in GameObject.FindGameObjectsWithTag("knight"))
        {
            if (knight != this)
            {
                knight.GetComponent<Knight>().knightDied.AddListener(OnOtherKnightDied);
                otherKnights.Add(knight);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected abstract bool CanAttackMinotaur();

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("colided with: " + collision.gameObject);
        if (collision.gameObject == minotaurAttackSphere)
        {
            hits += 1;
            if (hits >= maxHits)
            {
                Debug.Log("knightDied");
                knightDied.Invoke();
                Destroy(gameObject);
            }
        }
    }

    private void OnOtherKnightDied()
    {
        otherKnights = new List<GameObject>();
        GameObject[] knightsArray = GameObject.FindGameObjectsWithTag("knight");

        if (knightsArray != null && knightsArray.Length > 0)
        {
            foreach (GameObject knight in knightsArray)
            {
                if (knight != this)
                {
                    otherKnights.Add(knight);
                }
            }
        }
    }
}
