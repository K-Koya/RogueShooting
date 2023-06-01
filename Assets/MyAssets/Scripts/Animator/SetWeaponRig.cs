using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetWeaponRig : MonoBehaviour
{
    [SerializeField, Tooltip("���������e��")]
    GunInfo _initializeWeapon = null;

    [SerializeField, Tooltip("�E��pIK�ݒ�")]
    TwoBoneIKConstraint _rightHandIk = null;

    [SerializeField, Tooltip("����pIK�ݒ�")]
    TwoBoneIKConstraint _leftHandIk = null;

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
        if (_initializeWeapon)
        {
            DoSet(_initializeWeapon.gameObject);
        }
    }

    void Update()
    {
        
    }

    /// <summary>�擾�����e</summary>
    /// <param name="gunObj">�e�I�u�W�F�N�g</param>
    public void DoSet(GameObject gunObj)
    {
        GunInfo gg = gunObj.GetComponent<GunInfo>();

        if (_rightHandIk && _rightHand)
        {
            _dataRightHandTarget = _rightHandIk.data.target;
            _rightHandIk.data.target = gg.GunTriggerHands;
            _rightHandIk.transform.rotation = _rightHand.transform.rotation;
        }

        if (_leftHandIk && _leftHand)
        {
            _dataLeftHandTarget = _leftHandIk.data.target;
            _leftHandIk.data.target = gg.GunSupportHands;
            _leftHandIk.transform.rotation = _leftHand.transform.rotation;
        }
    }
}
