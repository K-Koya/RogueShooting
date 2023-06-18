using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class WalkWanderLookForward : IExecutionMethod
    {
        [SerializeField, Tooltip("������͈̔�")]
        float _wanderDistance = 20f;

        [SerializeField, Tooltip("�ړ�����W���������Ȃ����̓���L�����Z���ҋ@����")]
        float _timeOut = 1f;

        /// <summary>���Ԍv��</summary>
        float _timer = 0f;

        /// <summary>true : �����������ς�</summary>
        bool _isInitialized = false;


        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //���̃m�[�h�ɏ��߂ē�����
            if (!_isInitialized)
            {
                float radius = Random.Range(-_wanderDistance, _wanderDistance);
                float ratio = Random.Range(-1f, 1f);

                move.Destination = new Vector3(radius * ratio, 0f, radius * (1f - ratio)) + param.transform.position;
                _timer = _timeOut;
            }

            //�s�悪������܂Ŏ��s
            //��莞�Ԍ�����Ȃ���Ύ��s
            if (move.IsFoundDestination)
            {
                _isInitialized = true;
                param.State.Kind = MotionState.StateKind.Walk;
            }
            else
            {
                _timer -= Time.deltaTime;
                if (_timer < 0f)
                {
                    move.Destination = null;
                    _isInitialized = false;
                    return Status.Failure;
                }
            }

            //��Ɉړ������𒍎�
            param.LookDirection = param.MoveDirection;

            //���������琬��
            if (move.IsCloseDestination)
            {
                move.Destination = null;
                _isInitialized = false;
                return Status.Success;
            }

            return Status.Running;
        }
    }
}
