using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetWeaponRig : MonoBehaviour
{
    [SerializeField, Tooltip("初期装備銃器")]
    GunInfo _initializeWeapon = null;

    [SerializeField, Tooltip("右手用IK設定")]
    TwoBoneIKConstraint _rightHandIk = null;

    [SerializeField, Tooltip("左手用IK設定")]
    TwoBoneIKConstraint _leftHandIk = null;

    [SerializeField, Tooltip("右手の位置姿勢情報")]
    Transform _rightHand = null;

    [SerializeField, Tooltip("左手の位置姿勢情報")]
    Transform _leftHand = null;

    [SerializeField, Tooltip("右手のIKのターゲット位置")]
    Transform _rightHandTarget = null;

    [SerializeField, Tooltip("左手のIKのターゲット位置")]
    Transform _leftHandTarget = null;

    /// <summary>武器が持つ右手のIKのターゲット位置</summary>
    Transform _dataRightHandTarget = null;

    /// <summary>武器が持つ左手のIKのターゲット位置</summary>
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

    /// <summary>取得した銃</summary>
    /// <param name="gunObj">銃オブジェクト</param>
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
