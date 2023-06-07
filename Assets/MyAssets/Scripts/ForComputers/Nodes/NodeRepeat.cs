using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class NodeRepeat : INodeConnecter
    {
        /*
         * 配下ノードを指定回数施行するノード
         */

        [SerializeField, Tooltip("施行ループ回数")]
        int _count = 0;

        /// <summary>ループ回数カウンタ</summary>
        int _counter = -1;

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("ループさせるノード配列")]
        INodeConnecter[] _repeat = null;

        /// <summary>実行中ノードの配列番号（マイナス値 : 初期化前）</summary>
        short _runningIndex = -1;

        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            //ループ対象ノードが未指定
            if (_repeat is null || _repeat.Length < 1)
            {
                return Status.Failure;
            }

            //このノードに初めて入った時
            if (_runningIndex < 0 || _counter < 0)
            {
                _runningIndex = 0;
                _counter = _count;
            }

            //指定回数ループを繰り返す
            while (_counter > 0)
            {
                while (_runningIndex < _repeat.Length)
                {
                    //実行
                    Status returnal = _repeat[_runningIndex].NextNode(param, move);
                    switch (returnal)
                    {
                        //実行中
                        case Status.Running:
                            return returnal;
                        //失敗
                        case Status.Failure:
                            _runningIndex = -1;
                            _counter = -1;
                            return returnal;
                        default: break;
                    }

                    _runningIndex++;

                    //1フレーム待機
                    return Status.Running;
                }

                _runningIndex = 0;
                _counter--;
            }

            _runningIndex = -1;
            _counter = -1;
            return Status.Success;
        }
    }
}
