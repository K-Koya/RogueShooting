using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class NodeIf : INodeConnecter
    {
        /*
         * 配下ノードを特定条件で施行するノード
         */

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("施行する判定インターフェース（0以下 : Failure）")]
        IIfWhileConditionMethod _if = null;

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("判定先のノード")]
        INodeConnecter _doNext = null;

        /// <summary>判定値のキャッシュ（マイナス値 : 未初期化）</summary>
        short result = -1;


        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            //分岐条件が未指定
            if (_if is null)
            {
                return Status.Failure;
            }

            //このノードに初めて入った
            if (result < 0)
            {
                result = _if.Condition(param, move);

                //条件に当てはまらない場合は失敗
                if (result < 1)
                {
                    return Status.Failure;
                }
            }

            Status status = _doNext.NextNode(param, move);

            //終了か中断
            if(status != Status.Running)
            {
                result = -1;
            }

            return status;
        }
    }
}
