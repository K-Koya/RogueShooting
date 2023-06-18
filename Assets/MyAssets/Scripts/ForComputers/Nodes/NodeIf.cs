using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class NodeIf : INodeConnecter
    {
        /*
         * �z���m�[�h���������Ŏ{�s����m�[�h
         */

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("�{�s���锻��C���^�[�t�F�[�X")]
        IIfWhileConditionMethod _if = null;

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("�����̃m�[�h")]
        INodeConnecter _doNext = null;

        /// <summary>����l�̃L���b�V��</summary>
        bool? result = null;


        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            //�Y���L�����N�^�[���|���ꂽ�瑦�I��
            if(param.State.Kind is MotionState.StateKind.Defeat)
            {
                result = null;
                return Status.Failure;
            }

            //������������w��Ȃ玸�s
            if (_if is null)
            {
                result = null;
                return Status.Failure;
            }

            //���̃m�[�h�ɏ��߂ē�����
            if (result is null)
            {
                result = _if.Condition(param, move);

                //�����ɓ��Ă͂܂�Ȃ��ꍇ�͎��s
                if (!result.Value)
                {
                    result = null;
                    return Status.Failure;
                }
            }

            Status status = _doNext.NextNode(param, move);

            //�I�������f
            if(status != Status.Running)
            {
                result = null;
            }

            return status;
        }
    }
}
