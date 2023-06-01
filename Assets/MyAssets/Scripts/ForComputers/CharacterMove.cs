using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundChecker))]
public class CharacterMove : MonoBehaviour
{
    #region �萔
    /// <summary>���x��0�ł���Ƃ݂Ȃ����l</summary>
    protected const float VELOCITY_ZERO_BORDER = 0.5f;

    #endregion

    [SerializeField, Tooltip("���s�ō���")]
    protected float _limitSpeedWalk = 2f;

    [SerializeField, Tooltip("���s�ō���")]
    protected float _limitSpeedRun = 4.5f;


    #region �����o

    /// <summary>�Y���L�����N�^�[�̃��W�b�h�{�f�B</summary>
    protected Rigidbody _rb = null;

    /// <summary>�ڒn����R���|�[�l���g</summary>
    protected GroundChecker _gc = null;

    /// <summary>�Y���L�����N�^�[�̃p�����[�^</summary>
    protected CharacterParameter _param = null;
    

    /// <summary>�ړ������ɗ͂������鎞�̗͂̑傫��</summary>
    protected float _movePower = 3.0f;

    /// <summary>���ʂ̈ړ����x</summary>
    protected float _speed = 0.0f;

    /// <summary>�L�����N�^�[�́A������o�����肵�ėՐ�Ԑ��ɂȂ�p���^�C�}�[</summary>
    protected float _armedTimer = 0.0f;

    /// <summary>True : �W�����v����</summary>
    protected bool _jumpFlag = false;

    /// <summary>true : �e��A�N�V���������{����</summary>
    protected bool _doAction = false;

    /// <summary>�ړ����͂̑傫��</summary>
    protected float _moveInputRate = 0f;

    /// <summary>������u���[�L��</summary>
    protected Vector3 _ForceOfBrake = Vector3.zero;
    #endregion

    #region �v���p�e�B
    /// <summary>true : �e��A�N�V�������͂�������</summary>
    public bool DoAction { get => _doAction; }
    /// <summary>�L�����N�^�[�́A������o�����肵�ėՐ�Ԑ��ɂȂ�p���^�C�}�[</summary>
    public float ArmedTimer { get => _armedTimer; }
    /// <summary>True : ���n���Ă���</summary>
    public bool IsGround { get => _gc.IsGround; }
    /// <summary>�d�͕���</summary>
    public Vector3 GravityDirection { get => _gc.GravityDirection; }
    /// <summary>Rigidbody��velocity���ړ��������ʂɊ��Z��������</summary>
    public Vector3 VelocityOnPlane { get => Vector3.ProjectOnPlane(_rb.velocity, -GravityDirection); }
    /// <summary>�ړ������ɗ͂������鎞�̗͂̑傫��</summary>
    public float MovePower { set => _movePower = value; }
    /// <summary>���ʂ̈ړ����x</summary>
    public float Speed { get => _speed; }
    /// <summary>�W�����v����t���O</summary>
    public bool JumpFlag { get => _jumpFlag; }
    /// 
    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _gc = GetComponent<GroundChecker>();
        _param = GetComponent<CharacterParameter>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //���x����
        _speed = VelocityOnPlane.magnitude;
    }

    void FixedUpdate()
    {
        //���n���Ă��邩�ۂ��ŕ���
        //���n��
        if (IsGround)
        {
            //�ړ��͂��������Ă���
            if (_moveInputRate > 0f)
            {
                //��]����
                CharacterRotation(_param.MoveDirection, -GravityDirection, 720f);

                //���x�����������͂�������
                if (_param.State.Kind == MotionState.StateKind.Walk
                    && _speed < _limitSpeedWalk)
                {
                    _rb.AddForce(transform.forward * _moveInputRate * _movePower, ForceMode.Acceleration);
                }
                else if (_param.State.Kind == MotionState.StateKind.Run
                            && _speed < _limitSpeedRun)
                {
                    _rb.AddForce(transform.forward * _moveInputRate * _movePower, ForceMode.Acceleration);
                }

                //���x(����)���A���͕����֐ݒ�
                _rb.velocity = Quaternion.FromToRotation(Vector3.ProjectOnPlane(_rb.velocity, -GravityDirection), transform.forward) * _rb.velocity;

                //�d�͂�������
                _rb.AddForce(GravityDirection * 2f, ForceMode.Acceleration);
            }
            //�ړ��͂��Ȃ�
            else
            {
                //���݂̑��x��臒l�������������0�ɂ���
                if (VelocityOnPlane.sqrMagnitude < VELOCITY_ZERO_BORDER)
                {
                    _rb.velocity = Vector3.Project(_rb.velocity, -GravityDirection);
                }
                //�u���[�L��������
                else
                {
                    _ForceOfBrake = -_rb.velocity;
                }

                //�d�͂�������
                _rb.AddForce(GravityDirection * 1f, ForceMode.Acceleration);
            }
        }
        //��
        else
        {
            //�ړ��͂��������Ă���
            if (_moveInputRate > 0f)
            {
                //��]����
                CharacterRotation(_param.MoveDirection, -GravityDirection, 90f);

                //�͂�������
                _rb.AddForce(_param.MoveDirection * _moveInputRate * _movePower, ForceMode.Acceleration);
            }

            //�d�͂�������
            _rb.AddForce(GravityDirection * 9.8f, ForceMode.Acceleration);
        }

        //���x������������
        if (_ForceOfBrake.sqrMagnitude > 0f)
        {
            _rb.AddForce(_ForceOfBrake, ForceMode.Acceleration);
        }
    }

    /// <summary> �L�����N�^�[���w������ɉ�]������ </summary>
    /// <param name="targetDirection">�ڕW����</param>
    /// <param name="up">������iVector.Zero�Ȃ��������w�肵�Ȃ��j</param>
    /// <param name="rotateSpeed">��]���x</param>
    protected void CharacterRotation(Vector3 targetDirection, Vector3 up, float rotateSpeed)
    {
        if (targetDirection.sqrMagnitude > 0.0f)
        {
            Vector3 trunDirection = transform.right;
            Quaternion charDirectionQuaternion = Quaternion.identity;
            if (up.sqrMagnitude > 0f) charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f), up);
            else charDirectionQuaternion = Quaternion.LookRotation(targetDirection + (trunDirection * 0.001f));
            transform.rotation = Quaternion.RotateTowards(transform.rotation, charDirectionQuaternion, rotateSpeed * Time.deltaTime);
        }
    }
}


