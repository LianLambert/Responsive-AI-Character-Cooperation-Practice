using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HTN : MonoBehaviour
{
    public class HTNNode
    {
        public string TaskName { get; set; }
        public List<HTNNode> Children { get; set; }

        public HTNNode(string taskName)
        {
            TaskName = taskName;
            Children = new List<HTNNode>();
        }
    }
}
