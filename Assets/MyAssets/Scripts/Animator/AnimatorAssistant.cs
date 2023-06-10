using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAssistant : MonoBehaviour
{
    /// <summary>アニメーターパラメータ名 : ResultSpeed</summary>
    string _PARAM_NAME_RESULT_SPEED = "ResultSpeed";

    /// <summary>アニメーターパラメータ名 : RelativeMoveDirectionX</summary>
    string _PARAM_NAME_RELATIVE_MOVE_DIRECTION_X = "RelativeMoveDirectionX";

    /// <summary>アニメーターパラメータ名 : RelativeMoveDirectionY</summary>
    string _PARAM_NAME_RELATIVE_MOVE_DIRECTION_Y = "RelativeMoveDirectionY";

    /// <summary>アニメーターパラメータ名 : GetHit</summary>
    string _PARAM_NAME_GET_HIT = "GetHit";

    /// <summary>アニメーターパラメータ名 : IsDefeat</summary>
    string _PARAM_NAME_IS_DEFEAT = "IsDefeat";


    [SerializeField, Tooltip("足音を発するスピーカー")]
    AudioSource _footSESource = null;

    /// <summary>該当キャラクターのパラメータ</summary>
    CharacterParameter _param = null;

    /// <summary>該当キャラクターの動作メソッド</summary>
    CharacterMove _move = null;

    /// <summary>該当キャラクターのアニメーター</summary>
    Animator _anim = null;

    /// <summary>true : 倒される処理を実行した</summary>
    bool _wasDefeat = false;


    // Start is called before the first frame update
    void Start()
    {
        _param = GetComponentInParent<CharacterParameter>();
        _move = GetComponentInParent<CharacterMove>();
        _anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //倒された
        if (_wasDefeat)
        {
            return;
        }
        else if (_param.State.Kind is MotionState.StateKind.Defeat)
        {
            _anim.SetBool(_PARAM_NAME_IS_DEFEAT, _param.State.Kind is MotionState.StateKind.Defeat);
            _wasDefeat = true;
            return;
        }

        _anim.SetFloat(_PARAM_NAME_RESULT_SPEED, _move.Speed);

        //足回りの動き
        Vector3 lookSupVec = Vector3.ProjectOnPlane(_param.LookDirection, -_move.GravityDirection);
        _anim.SetFloat(_PARAM_NAME_RELATIVE_MOVE_DIRECTION_Y, Vector3.Dot(lookSupVec, _move.VelocityOnPlane));
        lookSupVec = Vector3.Cross(_param.LookDirection, _move.GravityDirection);
        _anim.SetFloat(_PARAM_NAME_RELATIVE_MOVE_DIRECTION_X, Vector3.Dot(lookSupVec, _move.VelocityOnPlane));

        //被弾
        if(_param.IsHurt)
        {
            _anim.SetTrigger(_PARAM_NAME_GET_HIT);
        }
    }


    void OnAnimatorIK(int layerIndex)
    {
        if (_param.State.Kind is MotionState.StateKind.Defeat)
        {
            _anim.SetLookAtWeight(0f, 0f, 0f);
        }
        else
        {
            _anim.SetLookAtPosition(_param.EyePoint.position + _param.LookDirection * 10f);
            _anim.SetLookAtWeight(0.5f, 0.5f, 0.5f);
        }
    }


    /// <summary>石畳の上を歩く音声</summary>
    public void EmitSEFootStampWalkOnRock()
    {
        _footSESource.PlayOneShot(SEManager.Instance.FootStampWalkOnRock);
    }

    /// <summary>石畳の上を走る音声</summary>
    public void EmitSEFootStampRunOnRock()
    {
        _footSESource.PlayOneShot(SEManager.Instance.FootStampRunOnRock);
    }
}
