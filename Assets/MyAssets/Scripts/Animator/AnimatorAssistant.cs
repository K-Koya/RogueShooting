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

    [SerializeField, Tooltip("足音を発するスピーカー")]
    AudioSource _footSESource = null;

    /// <summary>該当キャラクターのパラメータ</summary>
    CharacterParameter _param = null;

    /// <summary>該当キャラクターの動作メソッド</summary>
    CharacterMove _move = null;

    /// <summary>該当キャラクターのアニメーター</summary>
    Animator _anim = null;


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
        _anim.SetFloat(_PARAM_NAME_RESULT_SPEED, _move.Speed);

        //足回りの動き
        Vector3 lookSupVec = Vector3.ProjectOnPlane(_param.LookDirection, -_move.GravityDirection);
        _anim.SetFloat(_PARAM_NAME_RELATIVE_MOVE_DIRECTION_Y, Vector3.Dot(lookSupVec, _move.VelocityOnPlane));
        lookSupVec = Vector3.Cross(_param.LookDirection, _move.GravityDirection);
        _anim.SetFloat(_PARAM_NAME_RELATIVE_MOVE_DIRECTION_X, Vector3.Dot(lookSupVec, _move.VelocityOnPlane));
    }


    void OnAnimatorIK(int layerIndex)
    {
        _anim.SetLookAtPosition(_param.EyePoint.position + _param.LookDirection * 10f);
        _anim.SetLookAtWeight(0.5f, 0.5f, 0.5f);
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
