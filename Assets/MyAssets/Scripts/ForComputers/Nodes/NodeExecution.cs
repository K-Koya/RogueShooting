using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class NodeExecution : INodeConnecter
    {
        /*
         * このノードに到達すると、登録したメソッドを施行する
         */

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("施行する行動の制御インターフェース")]
        IExecutionMethod _execution = null;

        /// <summary>メソッドを施行しその結果を返す</summary>
        /// <returns>成功or失敗or実行中</returns>
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
