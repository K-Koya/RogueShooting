using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class SetWeaponRig : MonoBehaviour
{
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

    }

    void Update()
    {
        //ポーズ時は止める
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

    /// <summary>取得した銃を持つ処理</summary>
    /// <param name="gunObj">銃オブジェクト</param>
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
