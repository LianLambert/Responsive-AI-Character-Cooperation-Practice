using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HTNCompositeTask : HelperMethods
{
    [SerializeField] protected static GameObject treasure;
    [SerializeField] protected static GameObject minotaur;
    public bool TaskComplete = false;
    protected List<HTNPrimitiveTask> subtasks;

    public IEnumerator ExecuteTask(Knight knight)
    {
        foreach (HTNPrimitiveTask subtask in subtasks)
        {
            yield return knight.StartCoroutine(subtask.ExecuteTask(knight));
            if (subtask.GetType() == typeof(HTNPrimitiveTask.PickUpTreasure))
            {
                knight.hasTreasure = true;
                knight.treasure.transform.parent = knight.transform;
                knight.treasure.transform.localPosition = new Vector3(0f, 3, 0f);

            }
        }

        TaskComplete = true;
    }

    // Main tasks
    public class StealTreasureTask : HTNCompositeTask
    {
        public StealTreasureTask()
        {
            subtasks = new List<HTNPrimitiveTask>
            {
                new HTNPrimitiveTask.ApproachTreasure(),
                new HTNPrimitiveTask.PickUpTreasure(),
                new HTNPrimitiveTask.RunWithTreasure()
            };
        }
    }

    public class DistractMinotaurTask : HTNCompositeTask
    {
        public DistractMinotaurTask()
        {
            subtasks = new List<HTNPrimitiveTask>
            {
                new HTNPrimitiveTask.AttackMinotaur(),
                new HTNPrimitiveTask.RunFromMinotaur(),
            };
        }
    }
}
