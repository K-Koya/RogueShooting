using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class NodeWhile : INodeConnecter
    {
        [Header("While")]

        /*
         * 配下ノードを指定条件を満たす間施行するノード
         */

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("ループを継続する判定インターフェース（0 : false, 1以上 : true, -1以下 : Failure）")]
        IIfWhileConditionMethod _while = null;

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("ループさせるノード配列")]
        INodeConnecter[] _doLoop = null;

        /// <summary>実行中ノードの配列番号（マイナス値 : 初期化前）</summary>
        short _runningIndex = -1;

        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            //分岐条件・ループ対象ノードが未指定
            if (_while is null || _doLoop is null || _doLoop.Length < 1)
            {
                return Status.Failure;
            }

            //このノードに初めて入った時
            if (_runningIndex < 0)
            {
                _runningIndex = (short)(_doLoop.Length - 1);
            }

            //ループ条件を満たさなくなるまで繰り返し
            int result = _while.Condition(param, move);
            while (result > 0)
            {
                //実行
                Status returnal = _doLoop[_runningIndex].NextNode(param, move);

                switch (returnal)
                {
                    //実行中
                    case Status.Running:
                        return returnal;
                    //失敗
                    case Status.Failure:
                        _runningIndex = -1;
                        return returnal;
                    default: break;
                }

                _runningIndex--;
                if (_runningIndex < 0) _runningIndex = (short)(_doLoop.Length - 1);
            }

            _runningIndex = -1;
            return result < 0 ? Status.Failure : Status.Success;
        }
    }

    public interface IIfWhileConditionMethod
    {
        /// <summary>分岐条件</summary>
        /// <returns>ノード配列の番号（マイナス値 : 失敗）</returns>
        public short Condition(ComputerParameter param, ComputerMove move);
    }
}
