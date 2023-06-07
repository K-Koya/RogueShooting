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
        [Tooltip("�{�s���锻��C���^�[�t�F�[�X�i0�ȉ� : Failure�j")]
        IIfWhileConditionMethod _if = null;

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("�����̃m�[�h")]
        INodeConnecter _doNext = null;

        /// <summary>����l�̃L���b�V���i�}�C�i�X�l : ���������j</summary>
        short result = -1;


        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            //������������w��
            if (_if is null)
            {
                return Status.Failure;
            }

            //���̃m�[�h�ɏ��߂ē�����
            if (result < 0)
            {
                result = _if.Condition(param, move);

                //�����ɓ��Ă͂܂�Ȃ��ꍇ�͎��s
                if (result < 1)
                {
                    return Status.Failure;
                }
            }

            Status status = _doNext.NextNode(param, move);

            //�I�������f
            if(status != Status.Running)
            {
                result = -1;
            }

            return status;
        }
    }
}
