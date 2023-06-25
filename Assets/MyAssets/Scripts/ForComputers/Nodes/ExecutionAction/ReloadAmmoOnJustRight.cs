using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class ReloadAmmoOnJustRight : IExecutionMethod
    {
        /// <summary>�K�������Ƃ݂Ȃ��P�\</summary>
        float _DISTANCE_BUFFER = 1f;

        [SerializeField, Tooltip("�K������")]
        float _justRightDistance = 15f;

        /// <summary>true : �����������ς�</summary>
        bool _isInitialized = false;

        /// <summary>true : �K�������ł���</summary>
        bool _isJustRight = true;

        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //���̃m�[�h�ɏ��߂ē�����
            if (!_isInitialized)
            {
                _isInitialized = true;
                param.State.Kind = MotionState.StateKind.Walk;
            }

            //�����[�h�v��
            param.UsingGun.DoReload();

            //�^�[�Q�b�g�Ƃ̑��Έʒu
            Vector3 relative = param.Target.EyePoint.position - param.EyePoint.position;

            //��Ƀ^�[�Q�b�g�𒍎�
            param.LookDirection = Vector3.Normalize(relative);

            //�K���������߂�
            float sqrDistance = Vector3.SqrMagnitude(relative);
            float compare = _justRightDistance - _DISTANCE_BUFFER;
            if (sqrDistance < compare * compare)
            {
                _isJustRight = false;
                param.State.Kind = MotionState.StateKind.Walk;
                move.Destination = param.Target.transform.position - param.LookDirection * _justRightDistance;
            }
            else
            {
                //�K��������艓��
                compare = _justRightDistance + _DISTANCE_BUFFER;
                if(sqrDistance > compare * compare)
                {
                    _isJustRight = false;
                    param.State.Kind = MotionState.StateKind.Run;
                    move.Destination = param.Target.transform.position - param.LookDirection * _justRightDistance;
                }
                //�K�������ł���
                else
                {
                    _isJustRight = true;
                    param.State.Kind = MotionState.StateKind.Stay;
                    move.Destination = null;
                }
            }

            //�����[�h�����������琬��
            if (param.UsingGun.CurrentLoadAmmo >= param.UsingGun.MaxLoadAmmo)
            {
                move.Destination = null;
                _isInitialized = false;
                param.State.Kind = MotionState.StateKind.Stay;
                return Status.Success;
            }


            return Status.Running;
        }
    }
}
