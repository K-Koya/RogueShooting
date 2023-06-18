using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class FoundTarget : IIfWhileConditionMethod
    {
        public bool Condition(ComputerParameter param, ComputerMove move)
        {
            return param.Target ? true : false;
        }
    }
}
