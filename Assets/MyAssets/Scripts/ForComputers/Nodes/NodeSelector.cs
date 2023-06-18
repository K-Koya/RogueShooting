using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class NodeSelector : INodeConnecter
    {
        /*
         * �z���m�[�h�������ꂩ������{�s����m�[�h\n�t�Ɍ����΁A�ǂꂩ�̃m�[�h����������܂Ŏ{�s��������
         */

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("�z���m�[�h : �ォ�珇�Ƀm�[�h�]��")]
        INodeConnecter[] _selector = null;

        /// <summary>���J�菈�����̔z���m�[�h</summary>
        List<INodeConnecter> _runningSequences = null;

        /// <summary>�z���m�[�h�����J��Ɏ{�s���ARunning��Success�Ȃ瑦���f</summary>
        /// <returns>����or���sor���s��</returns>
        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            //�Y���L�����N�^�[���|���ꂽ�瑦�I��
            if (param.State.Kind is MotionState.StateKind.Defeat)
            {
                _runningSequences = null;
                return Status.Failure;
            }

            //���̃m�[�h�ɏ��߂ē�������
            if (_runningSequences is null)
            {
                _runningSequences = _selector.ToList();
            }

            //��{�S�����s����܂Ŏ{�s
            if (_runningSequences.Count > 0)
            {
                //���s
                Status returnal = _runningSequences.First().NextNode(param, move);

                switch (returnal)
                {
                    //���s��
                    case Status.Running:
                        return returnal;
                    //����
                    case Status.Success:
                        _runningSequences = null;
                        return returnal;
                    default: break;
                }

                //���s�����A���̃t���[���ɏ����������z��
                _runningSequences.RemoveAt(0);
                return Status.Running;
            }

            //�S�����s
            _runningSequences = null;
            return Status.Failure;
        }
    }
}
