using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class ReloadAmmoWithDodgeStep : IExecutionMethod
    {
        [SerializeField, Tooltip("�X�e�b�v����")]
        float _sideStepDistance = 2f;

        [SerializeField, Tooltip("�ړ�����W���������Ȃ����̓���L�����Z���ҋ@����")]
        float _timeOut = 1f;

        /// <summary>�^�[�Q�b�g�Ƃ̑��Έʒu</summary>
        Vector3 _relativePos = Vector3.zero;

        /// <summary>���Ԍv��</summary>
        float _timer = 0f;

        /// <summary>true : �����������ς�</summary>
        bool _isInitialized = false;

        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //�^�[�Q�b�g���Ƃ炦�Ă���Β���
            if (param.IsThroughLineOfSight)
            {
                //�^�[�Q�b�g�Ƃ̑��Έʒu
                _relativePos = param.Target.EyePoint.position - param.EyePoint.position;

                //��Ƀ^�[�Q�b�g�𒍎�
                param.LookDirection = Vector3.Normalize(_relativePos);
            }

            //���̃m�[�h�ɏ��߂ē�����
            if (!_isInitialized)
            {
                _isInitialized = true;
                param.State.Kind = MotionState.StateKind.Run;

                //�^�[�Q�b�g���Ƃ炦�Ă��Ȃ���΁A�O���𒍎�
                if (!param.IsThroughLineOfSight)
                {
                    _relativePos = param.transform.forward;
                }

                //�^�[�Q�b�g�ɑ΂��Ă����ꂩ������
                Vector3 sideDirection = Vector3.Cross(param.LookDirection, -move.GravityDirection);
                if (Random.value > 0.5f) sideDirection *= -1;

                //�ړI�n��ݒ�
                _timer = _timeOut;
                move.Destination = param.transform.position + sideDirection * _sideStepDistance;
            }

            //�����[�h�v��
            param.UsingGun.DoReload();

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

            //�����[�h�����������琬��
            if (param.UsingGun.CurrentLoadAmmo >= param.UsingGun.MaxLoadAmmo)
            {
                move.Destination = null;
                _isInitialized = false;
                return Status.Success;
            }

            return Status.Running;
        }
    }
}
