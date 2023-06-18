using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeNode;

public class BehaviorTreeBase : MonoBehaviour
{
    [SerializeReference, SelectableSerializeReference]
    [Tooltip("一番初めのノード")]
    INodeConnecter _rootNode = null;

    /// <summary>該当キャラクターのパラメータ</summary>
    ComputerParameter _param = null;

    /// <summary>該当キャラクターの移動メソッド</summary>
    ComputerMove _move = null;


    // Start is called before the first frame update
    void Start()
    {
        _param = GetComponent<ComputerParameter>();
        _move = GetComponent<ComputerMove>();
    }

    void Update()
    {
        //ポーズ時は止める
        if (GameManager.IsPose)
        {
            return;
        }

        _rootNode.NextNode(_param, _move);
    }
}

namespace BehaviorTreeNode
{
    /// <summary>子ノードのアクセス用インターフェース</summary>
    public interface INodeConnecter
    {
        public Status NextNode(ComputerParameter param, ComputerMove move);
    }

    /// <summary>ノードの状態</summary>
    public enum Status
    {
        Success,
        Failure,
        Running,
    }
}
