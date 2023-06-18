using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetWeaponRig : MonoBehaviour
{
    [SerializeField, Tooltip("�E��̈ʒu�p�����")]
    Transform _rightHand = null;

    [SerializeField, Tooltip("����̈ʒu�p�����")]
    Transform _leftHand = null;

    [SerializeField, Tooltip("�E���IK�̃^�[�Q�b�g�ʒu")]
    Transform _rightHandTarget = null;

    [SerializeField, Tooltip("�����IK�̃^�[�Q�b�g�ʒu")]
    Transform _leftHandTarget = null;

    /// <summary>���킪���E���IK�̃^�[�Q�b�g�ʒu</summary>
    Transform _dataRightHandTarget = null;

    /// <summary>���킪�������IK�̃^�[�Q�b�g�ʒu</summary>
    Transform _dataLeftHandTarget = null;

    void Start()
    {

    }

    void Update()
    {
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        if (_dataRightHandTarget)
        {
            _rightHandTarget.position = _dataRightHandTarget.position;
        }

        if (_dataLeftHandTarget)
        {
            _leftHandTarget.position = _dataLeftHandTarget.position;
        }
    }

    /// <summary>�擾�����e��������</summary>
    /// <param name="gunObj">�e�I�u�W�F�N�g</param>
    public void DoSet(GameObject gunObj)
    {
        GunInfo gg = gunObj.GetComponent<GunInfo>();

        if (_rightHandTarget && _rightHand)
        {
            _dataRightHandTarget = gg.GunTriggerHands;
            _rightHandTarget.rotation = _rightHand.transform.rotation;
        }
        else
        {
            _dataRightHandTarget = null;
        }

        if (_leftHandTarget && _leftHand)
        {
            _dataLeftHandTarget = gg.GunSupportHands;
            _leftHandTarget.rotation = _leftHand.transform.rotation;
        }
        else
        {
            _dataLeftHandTarget = null;
        }
    }

    public void DoRelease()
    {
        _dataRightHandTarget = null;
        _dataLeftHandTarget = null;
    }
}
