using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAttackSphere : MonoBehaviour
{
    private void OnEnable()
    {
        // Perform your intersection check logic here
        Collider[] colliders = Physics.OverlapSphere(transform.position, 6);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("knight"))
            {
                // have to accomodate for different scripts
                MeleeKnight meleeKnight = collider.GetComponent<MeleeKnight>();
                DistanceKnight distanceKnight = collider.GetComponent<DistanceKnight>();

                // Check if either script exists and has treasure
                if (meleeKnight != null)
                {
                    meleeKnight.attacked = true;
                }

                else if (distanceKnight != null)
                {
                    distanceKnight.attacked = true;
                }
            }
        }
    }
}
