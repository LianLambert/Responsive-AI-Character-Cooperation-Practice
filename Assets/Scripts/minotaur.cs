using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class minotaur : helperMethods
{
    [SerializeField] public float speed;
    [SerializeField] private GameObject treasure;
    [SerializeField] private GameObject attackSphere;
    private List<GameObject> knights = new List<GameObject>();
    private float treasureDangerDistance = 2;
    private float maxDistanceToTreasure = 5;
    private float attackRadius = 2;
    private int attackCooldown = 1;
    private bool coolingDown = false;
    private float coolingDownTimer = 0f;
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
        UpdateTimers();
        // if a player has the treasure
            // if we're not already within attack range, move towards them
            // if we're in attack range, attack

        // if a player is attacking us, attack them


        // if we can see a player and it won't bring us too far from the treasure, attack them
        

        // if not, don't do anything (idle mode)
        
    }

    private void Attack()
    {
        if (!coolingDown)
        {
            myAnimator.SetTrigger("Attack");
            coolingDown = true;
            coolingDownTimer = 0f;
            StartCoroutine(AttackSequence());

        }
    }

    private IEnumerator AttackSequence()
    {
        attackSphere.SetActive(true);
        yield return new WaitForSeconds(1f);
        attackSphere.SetActive(false);
    }

    private void UpdateTimers()
    {
        coolingDownTimer += Time.deltaTime;
        if(coolingDownTimer >= attackCooldown)
        {
            coolingDown = false;
        }
    }

    // returns the first knight that is too close to the treasure, if they exist, else null
    private GameObject closestKnight()
    {
        GameObject closestKnight = null;
        float closestDistance = float.MaxValue;

        foreach (GameObject knight in knights)
        {
            float knightDistance = distance2D(treasure, knight);

            if (knightDistance < closestDistance)
            {
                closestKnight = knight;
                closestDistance = knightDistance;
            }
        }

        return closestKnight;
    }

    // returns boolean indiicating whether knight is visible
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


    // if a knight has the treasure, returns the night, otherwise returns null
    private GameObject KnightWithTreasure()
    {
        foreach (GameObject knight in knights)
        {
            if (knight.GetComponent<knight>().hasTreasure)
            {
                return knight;
            }
        }

        return null;
    }
}
