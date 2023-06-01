using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class NodeSequencer : INodeConnecter
    {
        [Header("Sequencer")]

        /*
         * 配下ノードを順繰りに施行するノード\n逆に言えば、どれかのノードが失敗したら即処理をやめる
         */

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("配下ノード : 上から順にノード施行")]
        INodeConnecter[] _doSequences = null;

        /// <summary>順繰り処理中の配下ノード</summary>
        List<INodeConnecter> _runningSequences = null;


        /// <summary>配下ノードを順繰りに施行し、RunningかFailureなら即中断</summary>
        /// <returns>成功or失敗or実行中</returns>
        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            //このノードに初めて入った時
            if (_runningSequences is null)
            {
                _runningSequences = _doSequences.ToList();
            }

            //基本全部成功するまで施行
            while (_runningSequences.Count > 0)
            {
                //実行
                Status returnal = _runningSequences.First().NextNode(param, move);

                switch (returnal)
                {
                    //実行中
                    case Status.Running:
                        return returnal;
                    //失敗
                    case Status.Failure:
                        _runningSequences = null;
                        return returnal;
                    default: break;
                }

                //成功
                _runningSequences.RemoveAt(0);
            }

            //全部成功
            _runningSequences = null;
            return Status.Success;
        }
    }
}
