using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class ShotTarget : IExecutionMethod
    {
        [SerializeField, Tooltip("�Ə��u���̑傫��")]
        float _noiseSize = 3.0f;

        /// <summary>true : �����������ς�</summary>
        bool _isInitialized = false;


        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //���̃m�[�h�ɏ��߂ē�����
            if (!_isInitialized)
            {
                _isInitialized = true;
            }

            //�^�[�Q�b�g�����������玸�s
            if (!param.Target)
            {
                _isInitialized = false;
                return Status.Failure;
            }

            //���U�����e���s�����玸�s
            if (param.UsingGun.CurrentLoadAmmo < 1)
            {
                _isInitialized = false;
                return Status.Failure;
            }

            //�^�[�Q�b�g�Ɍ����Č���
            Vector3 noise = new Vector3((Random.value - 0.5f) * 2f, (Random.value - 0.5f) * 2f, (Random.value - 0.5f) * 2f) * _noiseSize * ComputerParameter.BaseAccuracyAim;
            param.UsingGun.DoShot(noise + param.Target.EyePoint.position);

            //��Ƀ^�[�Q�b�g�𒍎�
            param.LookDirection = Vector3.Normalize(param.Target.EyePoint.position - param.EyePoint.position);

            return Status.Running;
        }
    }
}
