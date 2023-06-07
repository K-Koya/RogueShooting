using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class AtRandom : IIfWhileConditionMethod
    {
        [SerializeField, Tooltip("true”ä—¦")]
        float _numberOfSelection = 0.5f;

        public short Condition(ComputerParameter param, ComputerMove move)
        {
            return (short)(Random.value < _numberOfSelection ? 1 : 0);
        }
    }
}
