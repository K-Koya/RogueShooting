using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    public class ApproachTarget : IExecutionMethod
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
            }

            //�^�[�Q�b�g�����������玸�s
            if (!param.Target)
            {
                _isInitialized = false;
                return Status.Failure;
            }

            //��Ƀ^�[�Q�b�g��ړI�n��
            move.Destination = param.Target.transform.position;

            //��Ƀ^�[�Q�b�g�𒍎�
            param.LookDirection = Vector3.Normalize(param.Target.EyePoint.position - param.EyePoint.position);

            _timer -= Time.deltaTime;

            //���������琬��
            if (move.IsCloseDestination)
            {
                move.Destination = null;
                _isInitialized = false;
                return Status.Success;
            }
            else if(_timer < 0f)
            {
                _isInitialized = false;
                return Status.Failure;
            }

            return Status.Running;
        }
    }
}
