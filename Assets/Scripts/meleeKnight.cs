using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeKnight : Knight
{
    private float attackRadius = 2;

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
        if (attacked)
        {
            attacked = false;
            Attacked();
        }
    }

    protected override bool CanAttackMinotaur()
    {
        return Distance2D(minotaur, this.gameObject) <= attackRadius;
    }
}
