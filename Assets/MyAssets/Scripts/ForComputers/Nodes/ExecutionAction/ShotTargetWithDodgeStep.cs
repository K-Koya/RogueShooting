using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class ShotTargetWithDodgeStep : IExecutionMethod
    {
        [SerializeField, Tooltip("�Ə��u���̑傫��")]
        float _noiseSize = 3.0f;

        [SerializeField, Tooltip("�X�e�b�v����")]
        float _sideStepDistance = 2f;

        [SerializeField, Tooltip("�ړ�����W���������Ȃ����̓���L�����Z���ҋ@����")]
        float _timeOut = 1f;

        /// <summary>���Ԍv��</summary>
        float _timer = 0f;

        /// <summary>true : �����������ς�</summary>
        bool _isInitialized = false;

        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //�^�[�Q�b�g�Ƃ̑��Έʒu
            Vector3 relative = param.Target.EyePoint.position - param.EyePoint.position;

            //��Ƀ^�[�Q�b�g�𒍎�
            param.LookDirection = Vector3.Normalize(relative);

            //���̃m�[�h�ɏ��߂ē�����
            if (!_isInitialized)
            {
                _isInitialized = true;
                param.State.Kind = MotionState.StateKind.Run;

                //�^�[�Q�b�g�ɑ΂��Ă����ꂩ������
                Vector3 sideDirection = Vector3.Cross(param.LookDirection, -move.GravityDirection);
                if (Random.value > 0.5f) sideDirection *= -1;

                //�ړI�n��ݒ�
                _timer = _timeOut;
                move.Destination = param.transform.position + sideDirection * _sideStepDistance;
            }

            //�^�[�Q�b�g�������������U�����e���s�����玸�s
            if (!param.IsThroughLineOfSight || param.UsingGun.CurrentLoadAmmo < 1)
            {
                _isInitialized = false;
                move.Destination = null;
                param.State.Kind = MotionState.StateKind.Stay;
                return Status.Failure;
            }

            //�^�[�Q�b�g�Ɍ����Č���
            Vector3 noise = _noiseSize * ComputerParameter.BaseAccuracyAim * new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * 2f;
            param.UsingGun.DoShot(noise + param.Target.EyePoint.position);


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
