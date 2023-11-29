using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAttackSphere : MonoBehaviour
{
    [SerializeField] private GameObject minotaur;

    private void OnEnable()
    {
        // Perform your intersection check logic here
        Collider[] colliders = Physics.OverlapSphere(transform.position, minotaur.GetComponent<Minotaur>().attackRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("knight"))
            {
                // have to accomodate for different scripts
                KnightMelee meleeKnight = collider.GetComponent<KnightMelee>();
                KnightDistance distanceKnight = collider.GetComponent<KnightDistance>();

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
