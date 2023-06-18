using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class CheckDistance : IIfWhileConditionMethod
    {
        [SerializeField, Tooltip("������Ƃ鋗��")]
        float _borderDistance = 0f;

        [SerializeField, Tooltip("true : �߂�����true ��������false")]
        bool _isCheckNear = true;

        public bool Condition(ComputerParameter param, ComputerMove move)
        {
            //�^�[�Q�b�g�������Ă��Ȃ��Ȃ玸�s
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
