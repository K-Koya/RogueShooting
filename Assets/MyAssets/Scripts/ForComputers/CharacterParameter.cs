using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameter : MonoBehaviour
{
    /// <summary>移動可否ビットフラグ</summary>
    MotionEnableFlag _can = (MotionEnableFlag)byte.MaxValue;

    /// <summary>行動状態</summary>
    MotionState _state = null;


    [SerializeField, Tooltip("キャラクターの目線位置")]
    protected Transform _eyePoint = null;

    /// <summary>照準器の位置</summary>
    protected Vector3 _reticlePoint = Vector3.zero;

    [SerializeField, Tooltip("キャラクターの向き情報")]
    protected Vector3 _characterDirection = Vector3.zero;

    [SerializeField, Tooltip("キャラクターの移動方向情報")]
    protected Vector3 _moveDirection = Vector3.zero;


    #region プロパティ
    /// <summary>移動可否ビットフラグ</summary>
    public MotionEnableFlag Can { get => _can; }

    /// <summary>行動状態</summary>
    public MotionState State { get => _state; }

    /// <summary>キャラクターの目線位置</summary>
    public Transform EyePoint { get => _eyePoint; }

    /// <summary>キャラクターの向き情報</summary>
    public Vector3 CharDirection { get => _characterDirection; set => _characterDirection = value; }

    /// <summary>キャラクターの移動方向情報</summary>
    public Vector3 MoveDirection { get => _moveDirection; set => _moveDirection = value; }

    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _can = 0;
        _state = new MotionState();
        _state.Kind = MotionState.StateKind.Stay;
        _state.Process = MotionState.ProcessKind.Playing;

        if (!_eyePoint)
        {
            _eyePoint = transform;
        }
    }

    // Update is called once per frame
    protected virtual void Update()
    {


        SetMotionEnableFlag();
    }

    /// <summary>ステートに応じて動作許可フラグを指定</summary>
    void SetMotionEnableFlag()
    {
        switch (_state.Kind)
        {
            case MotionState.StateKind.Stay:
            case MotionState.StateKind.Walk:
                _can = MotionEnableFlag.Walk
                        | MotionEnableFlag.Run
                        | MotionEnableFlag.Jump
                        | MotionEnableFlag.FireHipUse;

                break;
            case MotionState.StateKind.Run:
                _can = MotionEnableFlag.Walk
                        | MotionEnableFlag.Run
                        | MotionEnableFlag.Jump
                        | MotionEnableFlag.FireHipUse;

                break;

            case MotionState.StateKind.JumpNoraml:
                _can = MotionEnableFlag.FireHipUse;

                break;
            case MotionState.StateKind.FallNoraml:
                _can = MotionEnableFlag.FireHipUse;

                break;
            case MotionState.StateKind.FireHipUse:
                _can = MotionEnableFlag.Walk
                        | MotionEnableFlag.Run
                        | MotionEnableFlag.Jump
                        | MotionEnableFlag.FireHipUse
                        | MotionEnableFlag.FireLookInto;

                break;
            case MotionState.StateKind.FireLookInto:
                _can = MotionEnableFlag.Walk
                        | MotionEnableFlag.FireHipUse;

                break;
            case MotionState.StateKind.Hurt:
                _can = 0;

                break;
            case MotionState.StateKind.Defeat:
                _can = 0;

                break;
            default: break;
        }
    }

}

/// <summary>移動可否ビットフラグ</summary>
public enum MotionEnableFlag : byte
{
    Walk = 1,
    Run = 2,
    Jump = 4,
    FireHipUse = 8,
    FireLookInto = 16,

}
