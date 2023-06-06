using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : CharacterMove
{
    /// <summary>MainCameraの位置等の情報</summary>
    Transform _MainCameraTransform = null;



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _MainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        _param.LookDirection = _MainCameraTransform.forward;

        MoveByPlayer();
        ShotProcess();
    }

    /// <summary>移動方向を入力値から計算して求める</summary>
    /// <param name="input">入力値</param>
    /// <returns>移動方向</returns>
    Vector3 CalculateMoveDirection(Vector2 input)
    {
        //前方向と右方向を取得し、移動入力値を反映
        Vector3 vertical = Vector3.ProjectOnPlane(_MainCameraTransform.forward, -GravityDirection);
        vertical = vertical.normalized * input.y;
        Vector3 horizontal = Vector3.ProjectOnPlane(_MainCameraTransform.right, -GravityDirection);
        horizontal = horizontal.normalized * input.x;

        return vertical + horizontal;
    }

    /// <summary>標準の移動操作メソッド</summary>
    void MoveByPlayer()
    {
        //移動入力
        if ((_param.Can & MotionEnableFlag.Walk) == MotionEnableFlag.Walk)
        {
            _param.MoveDirection = CalculateMoveDirection(InputUtility.GetMoveDirection);
            _param.CharDirection = _param.MoveDirection;

            if (InputUtility.GetRun)
            {
                _param.State.Kind = MotionState.StateKind.Run;
            }
            else
            {
                _param.State.Kind = MotionState.StateKind.Walk;
            }
        }
        else
        {
            _param.MoveDirection = Vector3.zero;
            _param.State.Kind = MotionState.StateKind.Stay;
        }

        //入力があれば移動力の処理
        if (_param.MoveDirection.sqrMagnitude > 0)
        {
            //移動入力の大きさを取得
            _moveInputRate = _param.MoveDirection.magnitude;
            //移動方向を取得
            _param.MoveDirection *= 1 / _moveInputRate;
            //移動力指定
            _movePower = _limitSpeedRun;
        }
        else
        {
            _movePower = 0f;
            _moveInputRate = 0f;
            _param.MoveDirection = Vector3.zero;
        }

        //重力方向以外で移動量成分があった場合、ブレーキ量を計算する
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -VelocityOnPlane.normalized * 0.2f;
        }
    }

    /// <summary>銃器の動作メソッド</summary>
    void ShotProcess()
    {
        if (InputUtility.GetFire)
        {
            _param.UsingGun.DoShot();
        }

        if (InputUtility.GetDownReload)
        {
            _param.UsingGun.DoReload();
        }
    }
}
