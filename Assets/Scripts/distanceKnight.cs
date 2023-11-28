using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceKnight : Knight
{

    // Start is called before the first frame update
    void Start()
    {
        hits = 0;
        maxHits = (int)Mathf.Floor(maxHits / 2);

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
        if (attacked)
        {
            attacked = false;
            Attacked();
        }
    }

    protected override bool CanAttackMinotaur()
    {
        // cast a ray
        Vector3 direction = minotaur.transform.position - transform.position;

        if (Physics.Raycast(transform.position, direction, out RaycastHit hit))
        {
            // if the ray hits the minotaur, it is visible
            if (hit.collider.gameObject == minotaur)
            {
                return true;
            }
        }

        return false;
    }

    private void Attack()
    {
        minotaur.GetComponent<Minotaur>().knightAttacking = this.gameObject;

    }


    protected void OnOtherKnightDied()
    {
        foreach (GameObject knight in otherKnights)
        {
            if (knight.activeSelf)
            {
                otherKnights.Remove(knight);
            }
        }
    }
}
