using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class NodeRepeat : INodeConnecter
    {
        /*
         * �z���m�[�h���w��񐔎{�s����m�[�h
         */

        [SerializeField, Tooltip("�{�s���[�v��")]
        int _count = 0;

        /// <summary>���[�v�񐔃J�E���^</summary>
        int _counter = -1;

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("���[�v������m�[�h�z��")]
        INodeConnecter[] _repeat = null;

        /// <summary>���s���m�[�h�̔z��ԍ��i�}�C�i�X�l : �������O�j</summary>
        short _runningIndex = -1;

        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            //���[�v�Ώۃm�[�h�����w��
            if (_repeat is null || _repeat.Length < 1)
            {
                return Status.Failure;
            }

            //���̃m�[�h�ɏ��߂ē�������
            if (_runningIndex < 0 || _counter < 0)
            {
                _runningIndex = 0;
                _counter = _count;
            }

            //�w��񐔃��[�v���J��Ԃ�
            while (_counter > 0)
            {
                while (_runningIndex < _repeat.Length)
                {
                    //���s
                    Status returnal = _repeat[_runningIndex].NextNode(param, move);
                    switch (returnal)
                    {
                        //���s��
                        case Status.Running:
                            return returnal;
                        //���s
                        case Status.Failure:
                            _runningIndex = -1;
                            _counter = -1;
                            return returnal;
                        default: break;
                    }

                    _runningIndex++;

                    //1�t���[���ҋ@
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
