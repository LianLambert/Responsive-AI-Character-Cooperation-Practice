using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// NOTE: entire class for testing purposes only
public class clickToMove : MonoBehaviour
{
    public NavMeshAgent agent;

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(movePosition, out var hitInfo))
            {
                agent.SetDestination(hitInfo.point);
            }
        }
    }
}
