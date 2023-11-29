using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HTNPrimitiveTask
{
    public abstract IEnumerator ExecuteTask(Knight knight);
    public bool TaskComplete = false;

    public class ApproachTreasure : HTNPrimitiveTask
    {
        public override IEnumerator ExecuteTask(Knight knight)
        {
            Knight script = GetKnightScript(knight);
            script.taskText.text = "Approach T";

            while (!knight.hasTreasure && Distance2D(knight, script.treasure) > 4)
            {
                script.agent.SetDestination(script.treasure.transform.position);
                yield return null;
            }

        }
    }

    public class PickUpTreasure : HTNPrimitiveTask
    {
        public override IEnumerator ExecuteTask(Knight knight)
        {
            Knight script = GetKnightScript(knight);
            script.taskText.text = "Pick Up T";
            
            if (knight.hasTreasure)
            {
                yield break;
            }
            else
            {
                yield return new WaitForSeconds(3f);
            }
        }
    }

    public class RunWithTreasure : HTNPrimitiveTask
    {
        public override IEnumerator ExecuteTask(Knight knight)
        {
            Knight script = GetKnightScript(knight);
            script.taskText.text = "Run With T";
            Vector3 closestCorner = script.ClosestCorner();
            script.agent.SetDestination(closestCorner);

            // while the game has not been won yet
            while (Time.timeScale != 0)
            {
                foreach (Vector3 position in HelperMethods.cornerPositions)
                {
                    if (HelperMethods.Distance2D(knight, position) < 0.2f)
                    {
                        script.gameWon.Invoke();
                    }
                }
                yield return null;
            }
        }
    }

    public class AttackMinotaur : HTNPrimitiveTask
    {
        public override IEnumerator ExecuteTask(Knight knight)
        {
            Knight script = HelperMethods.GetKnightScript(knight);
            script.taskText.text = "Attack M";

            while (!script.CanAttack())
            {
                script.agent.SetDestination(script.minotaur.transform.position);

                // Check if CanAttack during the movement
                if (script.CanAttack())
                {
                    script.agent.ResetPath();
                    script.Attack();
                    
                    yield break;
                }

                yield return null;
            }

            script.Attack();
            TaskComplete = true;

        }
    }

    public class RunFromMinotaur : HTNPrimitiveTask
    {
        public override IEnumerator ExecuteTask(Knight knight)
        {
            Knight script = GetKnightScript(knight);
            script.taskText.text = "Run From M";
            Vector3 newDestination = script.minotaur.GetComponent<Minotaur>().FurthestCorner();
            script.agent.ResetPath();
            script.agent.SetDestination(newDestination);

            while (script.agent.transform.position != newDestination)
            {
                yield return null;
            }
        }
    }

    // Helpers
    public static float Distance2D(Knight obj1, GameObject obj2)
    {
        float xDifference = obj1.transform.position.x - obj2.transform.position.x;
        float zDifference = obj1.transform.position.z - obj2.transform.position.z;
        return Mathf.Sqrt((xDifference * xDifference) + (zDifference * zDifference));
    }

    public static bool IsMeleeKnight(Knight knight)
    {
        if (knight.GetComponent<KnightMelee>() != null)
        {
            return true;
        }

        return false;
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
