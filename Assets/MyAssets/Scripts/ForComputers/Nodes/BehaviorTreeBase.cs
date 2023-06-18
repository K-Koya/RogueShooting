using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeNode;

public class BehaviorTreeBase : MonoBehaviour
{
    [SerializeReference, SelectableSerializeReference]
    [Tooltip("��ԏ��߂̃m�[�h")]
    INodeConnecter _rootNode = null;

    /// <summary>�Y���L�����N�^�[�̃p�����[�^</summary>
    ComputerParameter _param = null;

    /// <summary>�Y���L�����N�^�[�̈ړ����\�b�h</summary>
    ComputerMove _move = null;


    // Start is called before the first frame update
    void Start()
    {
        _param = GetComponent<ComputerParameter>();
        _move = GetComponent<ComputerMove>();
    }

    void Update()
    {
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        _rootNode.NextNode(_param, _move);
    }
}

namespace BehaviorTreeNode
{
    /// <summary>�q�m�[�h�̃A�N�Z�X�p�C���^�[�t�F�[�X</summary>
    public interface INodeConnecter
    {
        public Status NextNode(ComputerParameter param, ComputerMove move);
    }

    /// <summary>�m�[�h�̏��</summary>
    public enum Status
    {
        Success,
        Failure,
        Running,
    }
}
