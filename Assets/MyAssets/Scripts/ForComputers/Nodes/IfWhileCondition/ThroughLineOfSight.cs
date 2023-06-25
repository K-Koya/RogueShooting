using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class ThroughLineOfSight : IIfWhileConditionMethod
    {
        public bool Condition(ComputerParameter param, ComputerMove move)
        {
            return param.IsThroughLineOfSight ? true : false;
        }
    }
}
