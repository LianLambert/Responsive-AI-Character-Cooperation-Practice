using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeKnight : Knight
{
    private float attackRadius = 2;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override bool CanAttackMinotaur()
    {
        return Distance2D(minotaur, this.gameObject) <= attackRadius;
    }
}
