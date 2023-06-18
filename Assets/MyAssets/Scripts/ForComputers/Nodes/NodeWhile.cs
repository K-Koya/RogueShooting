using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class NodeWhile : INodeConnecter
    {
        /*
         * �z���m�[�h���w������𖞂����Ԏ{�s����m�[�h
         */

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("���[�v���p�����锻��C���^�[�t�F�[�X�i0 : false, 1�ȏ� : true, -1�ȉ� : Failure�j")]
        IIfWhileConditionMethod _while = null;

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("���[�v������m�[�h�z��")]
        INodeConnecter[] _doLoop = null;

        /// <summary>���s���m�[�h�̔z��ԍ��i�}�C�i�X�l : �������O�j</summary>
        short _runningIndex = -1;

        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            //�Y���L�����N�^�[���|���ꂽ�瑦�I��
            if (param.State.Kind is MotionState.StateKind.Defeat)
            {
                _runningIndex = -1;
                return Status.Failure;
            }

            //��������E���[�v�Ώۃm�[�h�����w��
            if (_while is null || _doLoop is null || _doLoop.Length < 1)
            {
                _runningIndex = -1;
                return Status.Failure;
            }

            //���̃m�[�h�ɏ��߂ē�������
            if (_runningIndex < 0)
            {
                _runningIndex = (short)(_doLoop.Length - 1);
            }

            //���[�v�����𖞂����Ȃ��Ȃ�܂ŌJ��Ԃ�
            bool result = _while.Condition(param, move);
            if (result)
            {
                //���s
                Status returnal = _doLoop[_runningIndex].NextNode(param, move);

                switch (returnal)
                {
                    //���s��
                    case Status.Running:
                        return returnal;
                    //���s
                    case Status.Failure:
                        _runningIndex = -1;
                        return returnal;
                    default: break;
                }

                _runningIndex--;
                if (_runningIndex < 0) _runningIndex = (short)(_doLoop.Length - 1);

                //1�t���[���ҋ@
                return Status.Running;
            }

            _runningIndex = -1;
            return result ? Status.Success : Status.Failure;
        }
    }

    public interface IIfWhileConditionMethod
    {
        /// <summary>�������</summary>
        /// <returns>true : �����Efalse : ���s</returns>
        public bool Condition(ComputerParameter param, ComputerMove move);
    }
}
