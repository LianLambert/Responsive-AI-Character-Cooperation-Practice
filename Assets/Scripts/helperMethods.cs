using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class helperMethods : MonoBehaviour
{


    // returns a game object's position with the y value set to 0
    public static Vector3 posXZ(GameObject obj)
    {
        return new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
    }

    // returns distance between two gameobjects disregarding y positions
    public static float distance2D(GameObject obj1, GameObject obj2)
    {
        float xDifference = obj1.transform.position.x - obj2.transform.position.x;
        float zDifference = obj1.transform.position.z - obj2.transform.position.z;
        return Mathf.Sqrt((xDifference * xDifference) + (zDifference * zDifference));
    }

    public static void moveToPoint(NavMeshAgent agent, Vector3 point)
    {
        // Set the destination for the NavMeshAgent
        agent.destination = point;
    }
}
