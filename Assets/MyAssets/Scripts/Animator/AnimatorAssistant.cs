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
    }
}
