using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurAttackSphere : MonoBehaviour
{
    private void OnEnable()
    {
        // Perform your intersection check logic here
        Collider[] colliders = Physics.OverlapSphere(transform.position, GetComponent<SphereCollider>().radius);

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.CompareTag("knight"))
            {
                gameObject.GetComponent<Knight>().attacked = true;
            }
        }
    }
}
