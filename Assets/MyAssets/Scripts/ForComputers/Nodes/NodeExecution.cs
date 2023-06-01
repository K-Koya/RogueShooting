using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class NodeExecution : INodeConnecter
    {
        [Header("Execution")]

        /*
         * ���̃m�[�h�ɓ��B����ƁA�o�^�������\�b�h���{�s����
         */

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("�{�s����s���̐���R���|�[�l���g")]
        IExecutionMethod executionMethod = null;

        /// <summary>���\�b�h���{�s�����̌��ʂ�Ԃ�</summary>
        /// <returns>����or���sor���s��</returns>
        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            if (executionMethod is null)
            {
                return Status.Failure;
            }

            return executionMethod.Method(param, move);
        }
    }

    public interface IExecutionMethod
    {
        public Status Method(ComputerParameter param, ComputerMove move);
    }
}
