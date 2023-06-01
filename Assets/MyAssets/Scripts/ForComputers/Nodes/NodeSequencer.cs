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
         * �z���m�[�h�����J��Ɏ{�s����m�[�h\n�t�Ɍ����΁A�ǂꂩ�̃m�[�h�����s�����瑦��������߂�
         */

        [SerializeReference, SelectableSerializeReference]
        [Tooltip("�z���m�[�h : �ォ�珇�Ƀm�[�h�{�s")]
        INodeConnecter[] _doSequences = null;

        /// <summary>���J�菈�����̔z���m�[�h</summary>
        List<INodeConnecter> _runningSequences = null;


        /// <summary>�z���m�[�h�����J��Ɏ{�s���ARunning��Failure�Ȃ瑦���f</summary>
        /// <returns>����or���sor���s��</returns>
        public Status NextNode(ComputerParameter param, ComputerMove move)
        {
            //���̃m�[�h�ɏ��߂ē�������
            if (_runningSequences is null)
            {
                _runningSequences = _doSequences.ToList();
            }

            //��{�S����������܂Ŏ{�s
            while (_runningSequences.Count > 0)
            {
                //���s
                Status returnal = _runningSequences.First().NextNode(param, move);

                switch (returnal)
                {
                    //���s��
                    case Status.Running:
                        return returnal;
                    //���s
                    case Status.Failure:
                        _runningSequences = null;
                        return returnal;
                    default: break;
                }

                //����
                _runningSequences.RemoveAt(0);
            }

            //�S������
            _runningSequences = null;
            return Status.Success;
        }
    }
}
