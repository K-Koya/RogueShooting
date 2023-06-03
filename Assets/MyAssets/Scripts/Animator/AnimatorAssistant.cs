using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorAssistant : MonoBehaviour
{
    /// <summary>�A�j���[�^�[�p�����[�^�� : ResultSpeed</summary>
    string _PARAM_NAME_RESULT_SPEED = "ResultSpeed";

    /// <summary>�A�j���[�^�[�p�����[�^�� : RelativeMoveDirectionX</summary>
    string _PARAM_NAME_RELATIVE_MOVE_DIRECTION_X = "RelativeMoveDirectionX";

    /// <summary>�A�j���[�^�[�p�����[�^�� : RelativeMoveDirectionY</summary>
    string _PARAM_NAME_RELATIVE_MOVE_DIRECTION_Y = "RelativeMoveDirectionY";

    /// <summary>�Y���L�����N�^�[�̃p�����[�^</summary>
    CharacterParameter _param = null;

    /// <summary>�Y���L�����N�^�[�̓��상�\�b�h</summary>
    CharacterMove _move = null;

    /// <summary>�Y���L�����N�^�[�̃A�j���[�^�[</summary>
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
