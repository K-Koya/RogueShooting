using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class AtRandom : IIfWhileConditionMethod
    {
        [SerializeField, Tooltip("true�䗦")]
        float _numberOfSelection = 0.5f;

        public bool Condition(ComputerParameter param, ComputerMove move)
        {
            return Random.value < _numberOfSelection ? true : false;
        }
    }
}
