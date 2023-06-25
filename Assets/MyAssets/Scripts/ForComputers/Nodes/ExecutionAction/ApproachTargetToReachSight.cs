using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class ApproachTargetToReachSight : IExecutionMethod
    {
        

        /// <summary>true : �����������ς�</summary>
        bool _isInitialized = false;

        [SerializeField, Tooltip("�ǐՐ�������")]
        float _timeOut = 20f;

        /// <summary>���Ԍv��</summary>
        float _timer = 0f;

        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //���̃m�[�h�ɏ��߂ē�����
            if (!_isInitialized)
            {
                _timer = _timeOut;
                _isInitialized = true;
                param.State.Kind = MotionState.StateKind.Run;
            }

            //�^�[�Q�b�g�����������玸�s
            if (!param.Target)
            {
                move.Destination = null;
                _isInitialized = false;
                return Status.Failure;
            }

            //��Ƀ^�[�Q�b�g��ړI�n��
            move.Destination = param.Target.transform.position;

            //��Ƀ^�[�Q�b�g�𒍎�
            param.LookDirection = Vector3.Normalize(param.Target.EyePoint.position - param.EyePoint.position);

            _timer -= Time.deltaTime;

            //�ː����ʂ邩�A�ڋ߂����琬��
            if (move.IsCloseDestination
                || param.IsThroughLineOfSight)
            {
                _isInitialized = false;
                return Status.Success;
            }
            else if (_timer < 0f)
            {
                param.State.Kind = MotionState.StateKind.Stay;
                move.Destination = null;
                _isInitialized = false;
                return Status.Failure;
            }

            return Status.Running;
        }
    }
}
