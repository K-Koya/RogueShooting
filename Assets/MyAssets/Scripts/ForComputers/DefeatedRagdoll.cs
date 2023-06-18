using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedRagdoll : MonoBehaviour
{
    /// <summary>�͂�������{��</summary>
    const float _ADD_FORCE_RATIO = 10.0f;

    [SerializeField, Tooltip("�̂̒��S�߂��̃��W�b�h�{�f�B")]
    Rigidbody _spineRb = null;

    [SerializeField, Tooltip("���f���������܂ł̑ҋ@����")]
    short _destroyDelay = 7;

    /// <summary>���f���������܂ł̎��ԃJ�E���^�[</summary>
    float _timer = 0;


    /// <summary>���O�h�[���̐�����΂�����</summary>
    /// <param name="force">�̂̒��S����͂����������</param>
    public void BlowAway(Vector3 force)
    {
        _spineRb.AddForce(force * _ADD_FORCE_RATIO, ForceMode.VelocityChange);
    }
        
    void Start()
    {
        _timer = _destroyDelay;
    }

    // Update is called once per frame
    void Update()
    {
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        _timer -= Time.deltaTime;
        if(_timer < 0f)
        {
            Destroy(gameObject);
        }
    }
}
