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
         * このノードに到達すると、登録したメソッドを施行する
         */

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("施行する行動の制御コンポーネント")]
        IExecutionMethod executionMethod = null;

        /// <summary>メソッドを施行しその結果を返す</summary>
        /// <returns>成功or失敗or実行中</returns>
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
