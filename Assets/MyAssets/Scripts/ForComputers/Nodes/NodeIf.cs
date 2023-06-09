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
        [Tooltip("施行する判定インターフェース")]
        IIfWhileConditionMethod _if = null;

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("判定先のノード")]
        INodeConnecter _doNext = null;

        /// <summary>判定値のキャッシュ</summary>
        bool? result = null;


        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            //該当キャラクターが倒されたら即終了
            if(param.State.Kind is MotionState.StateKind.Defeat)
            {
                result = null;
                return Status.Failure;
            }

            //分岐条件が未指定なら失敗
            if (_if is null)
            {
                result = null;
                return Status.Failure;
            }

            //このノードに初めて入った
            if (result is null)
            {
                result = _if.Condition(param, move);

                //条件に当てはまらない場合は失敗
                if (!result.Value)
                {
                    result = null;
                    return Status.Failure;
                }
            }

            Status status = _doNext.NextNode(param, move);

            //終了か中断
            if(status != Status.Running)
            {
                result = null;
            }

            return status;
        }
    }
}
