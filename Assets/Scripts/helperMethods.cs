using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HelperMethods : MonoBehaviour
{
    public static List<Vector3> cornerPositions = new List<Vector3>
    {
        new Vector3(2, 0.5f, 2),
        new Vector3(2, 0.5f, 48),
        new Vector3(78, 0.5f, 2),
        new Vector3(78, 0.5f, 48)
    };

    // returns a game object's position with the y value set to 0
    public static Vector3 PosXZ(GameObject obj)
    {
        return new Vector3(obj.transform.position.x, 0, obj.transform.position.z);
    }

    // returns distance between two gameobjects disregarding y positions
    public static float Distance2D(GameObject obj1, GameObject obj2)
    {
        float xDifference = obj1.transform.position.x - obj2.transform.position.x;
        float zDifference = obj1.transform.position.z - obj2.transform.position.z;
        return Mathf.Sqrt((xDifference * xDifference) + (zDifference * zDifference));
    }


    public static float Distance2D(Knight obj1, GameObject obj2)
    {
        float xDifference = obj1.transform.position.x - obj2.transform.position.x;
        float zDifference = obj1.transform.position.z - obj2.transform.position.z;
        return Mathf.Sqrt((xDifference * xDifference) + (zDifference * zDifference));
    }

    public static float Distance2D(Knight obj1, Vector3 position)
    {
        float xDifference = obj1.transform.position.x - position.x;
        float zDifference = obj1.transform.position.z - position.z;
        return Mathf.Sqrt((xDifference * xDifference) + (zDifference * zDifference));
    }

    public static bool IsMeleeKnight(GameObject knight)
    {
        if (knight.GetComponent<KnightMelee>() != null)
        {
            return true;
        }

        return false;
    }

    public static bool IsMeleeKnight(Knight knight)
    {
        if (knight.GetComponent<KnightMelee>() != null)
        {
            return true;
        }

        return false;
    }

    public static Knight GetKnightScript(GameObject knight)
    {
        if (IsMeleeKnight(knight))
        {
            return knight.GetComponent<KnightMelee>();
        }
        else
        {
            return knight.GetComponent<KnightDistance>();
        }
    }

    public static Knight GetKnightScript(Knight knight)
    {
        if (IsMeleeKnight(knight))
        {
            return knight.GetComponent<KnightMelee>();
        }
        else
        {
            return knight.GetComponent<KnightDistance>();
        }
    }
}
