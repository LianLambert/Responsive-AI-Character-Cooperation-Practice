using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAttackSphere : MonoBehaviour
{
    [SerializeField] private GameObject minotaur;

    private void OnEnable()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, minotaur.GetComponent<Minotaur>().attackRadius);

        foreach (Collider collider in colliders)
        {
            // check if collided with a knight
            if (collider.gameObject.CompareTag("knight"))
            {
                // have to accomodate for different scripts
                KnightMelee meleeKnight = collider.GetComponent<KnightMelee>();
                KnightDistance distanceKnight = collider.GetComponent<KnightDistance>();

                // if either script exists, set the knight to attacked
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
