using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    /// <summary>�ΏۃL�����N�^�[�̃p�����[�^</summary>
    CharacterParameter _param = null;

    [SerializeField, Tooltip("��_���[�W�{��")]
    float _damageRatio = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _param = GetComponentInParent<CharacterParameter>();
    }

    /// <summary>�_���[�W����</summary>
    /// <param name="damage">��{�_���[�W</param>
    /// <param name="impact">�Ռ��̑傫��</param>
    public void GetDamage(short damage, float impact, Vector3 impactDirection)
    {
        //�X�e�[�^�X������A������Ă��Ȃ��ꍇ�Ƀ_���[�W����
        if (_param && _param.State.Kind != MotionState.StateKind.Defeat)
        {
            _param.GaveDamage((short)(damage * _damageRatio), impact, impactDirection);
        }
    }
}
