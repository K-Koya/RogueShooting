using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class CheckDistance : IIfWhileConditionMethod
    {
        [SerializeField, Tooltip("判定をとる距離")]
        float _borderDistance = 0f;

        [SerializeField, Tooltip("true : 近い時にtrue 遠い時にfalse")]
        bool _isCheckNear = true;

        public bool Condition(ComputerParameter param, ComputerMove move)
        {
            //ターゲットを見つけていないなら失敗
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
