using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class NodeExecution : INodeConnecter
    {
        /*
         * ���̃m�[�h�ɓ��B����ƁA�o�^�������\�b�h���{�s����
         */

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("�{�s����s���̐���C���^�[�t�F�[�X")]
        IExecutionMethod _execution = null;

        /// <summary>���\�b�h���{�s�����̌��ʂ�Ԃ�</summary>
        /// <returns>����or���sor���s��</returns>
        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            if (_execution is null)
            {
                return Status.Failure;
            }

            return _execution.Method(param, move);
        }
    }

    public interface IExecutionMethod
    {
        public Status Method(ComputerParameter param, ComputerMove move);
    }
}
