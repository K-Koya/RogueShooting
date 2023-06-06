using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : CharacterMove
{
    /// <summary>MainCamera�̈ʒu���̏��</summary>
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

    /// <summary>�ړ���������͒l����v�Z���ċ��߂�</summary>
    /// <param name="input">���͒l</param>
    /// <returns>�ړ�����</returns>
    Vector3 CalculateMoveDirection(Vector2 input)
    {
        //�O�����ƉE�������擾���A�ړ����͒l�𔽉f
        Vector3 vertical = Vector3.ProjectOnPlane(_MainCameraTransform.forward, -GravityDirection);
        vertical = vertical.normalized * input.y;
        Vector3 horizontal = Vector3.ProjectOnPlane(_MainCameraTransform.right, -GravityDirection);
        horizontal = horizontal.normalized * input.x;

        return vertical + horizontal;
    }

    /// <summary>�W���̈ړ����상�\�b�h</summary>
    void MoveByPlayer()
    {
        //�ړ�����
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

        //���͂�����Έړ��͂̏���
        if (_param.MoveDirection.sqrMagnitude > 0)
        {
            //�ړ����͂̑傫�����擾
            _moveInputRate = _param.MoveDirection.magnitude;
            //�ړ��������擾
            _param.MoveDirection *= 1 / _moveInputRate;
            //�ړ��͎w��
            _movePower = _limitSpeedRun;
        }
        else
        {
            _movePower = 0f;
            _moveInputRate = 0f;
            _param.MoveDirection = Vector3.zero;
        }

        //�d�͕����ȊO�ňړ��ʐ������������ꍇ�A�u���[�L�ʂ��v�Z����
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -VelocityOnPlane.normalized * 0.2f;
        }
    }

    /// <summary>�e��̓��상�\�b�h</summary>
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
