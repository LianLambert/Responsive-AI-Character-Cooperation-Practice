using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class distanceKnight : knight
{

    // Start is called before the first frame update
    void Start()
    {
        maxHits = (int) Mathf.Floor(maxHits / 2);
    }

    // Update is called once per frame
    void Update()
    {
        
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

}
