using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class CheckDistance : IIfWhileConditionMethod
    {
        [SerializeField, Tooltip("”»’è‚ð‚Æ‚é‹——£")]
        float _borderDistance = 0f;

        [SerializeField, Tooltip("true : ‹ß‚¢Žž‚Étrue ‰“‚¢Žž‚Éfalse")]
        bool _isCheckNear = true;

        public bool Condition(ComputerParameter param, ComputerMove move)
        {
            //ƒ^[ƒQƒbƒg‚ðŒ©‚Â‚¯‚Ä‚¢‚È‚¢‚È‚çŽ¸”s
            if (!param.Target) return false;

            bool result = false;
            if(Vector3.SqrMagnitude(param.Target.transform.position - param.transform.position) > _borderDistance * _borderDistance)
            {
                result = true;
            }

            if (_isCheckNear)
            {
                return !result;
            }

            return result;
        }
    }
}
