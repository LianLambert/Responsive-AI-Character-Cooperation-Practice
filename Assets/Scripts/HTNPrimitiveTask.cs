using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HTNPrimitiveTask
{
    public abstract IEnumerator ExecuteTask(Knight knight);

    public class ApproachTreasure : HTNPrimitiveTask
    {
        public override IEnumerator ExecuteTask(Knight knight)
        {
            Knight script = GetKnightScript(knight);
            script.taskText.text = "Approach T";

            // if the knight doesn't already have the treasure, move towards it until close enough
            while (!knight.hasTreasure && Distance2D(knight, script.treasure) > 3)
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

            // if the knight does not already have the treasure, wait for cooldown timer to fill then pick it up (3s)
            if (knight.hasTreasure)
            {
                yield break;
            }
            else
            {
                if (knight.treasurePickUpCooldownTimer < 3)
                {
                    yield return null;
                }

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

            // move towards the closest corner to the knight
            Vector3 closestCorner = script.ClosestCorner();
            script.agent.SetDestination(closestCorner);

            // while the game has not been won yet
            while (Time.timeScale != 0)
            {
                // check if knight is at corner
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

            // while the knight cannot attack, move closer to minotaur
            while (!script.CanAttack())
            {
                script.agent.SetDestination(script.minotaur.transform.position);

                // if CanAttack() is then true, stop moving and attack
                if (script.CanAttack())
                {
                    break;
                }

                yield return null;
            }

            // if CanAttack() is then true, stop moving and attack
            script.agent.ResetPath();
            script.Attack();
            yield return new WaitForSeconds(1f);

        }
    }

    public class RunFromMinotaur : HTNPrimitiveTask
    {
        public override IEnumerator ExecuteTask(Knight knight)
        {
            Knight script = GetKnightScript(knight);
            script.taskText.text = "Run From M";

            // set destination to the furthest corner from the minotaur
            Vector3 newDestination = script.minotaur.GetComponent<Minotaur>().FurthestCornerFromMinotaur();
            script.agent.SetDestination(newDestination);

            // while the game is not over
            while (Time.timeScale != 0)
            {
                //// if the knight is at the furthest corner from the minotaur, recalculate the furthest corner
                //if (Vector3.Distance(script.agent.transform.position, script.agent.destination) == 0)
                //{
                //    newDestination = script.minotaur.GetComponent<Minotaur>().FurthestCornerFromMinotaur();
                //}
                

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
