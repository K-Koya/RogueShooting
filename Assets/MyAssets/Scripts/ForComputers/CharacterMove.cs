using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(GroundChecker))]
public class CharacterMove : MonoBehaviour
{
    #region 定数
    /// <summary>速度が0であるとみなす数値</summary>
    protected const float VELOCITY_ZERO_BORDER = 0.5f;

    #endregion

    [SerializeField, Tooltip("歩行時最高速")]
    protected float _limitSpeedWalk = 2f;

    [SerializeField, Tooltip("走行時最高速")]
    protected float _limitSpeedRun = 4f;


    #region メンバ

    /// <summary>該当キャラクターのリジッドボディ</summary>
    protected Rigidbody _rb = null;

    /// <summary>接地判定コンポーネント</summary>
    protected GroundChecker _gc = null;

    /// <summary>該当キャラクターのパラメータ</summary>
    protected CharacterParameter _param = null;
    

    /// <summary>移動向けに力をかける時の力の大きさ</summary>
    protected float _movePower = 3.0f;

    /// <summary>結果の移動速度</summary>
    protected float _speed = 0.0f;

    /// <summary>キャラクターの、武器を出したりして臨戦態勢になる継続タイマー</summary>
    protected float _armedTimer = 0.0f;

    /// <summary>True : ジャンプ直後</summary>
    protected bool _jumpFlag = false;

    /// <summary>true : 各種アクションを実施直後</summary>
    protected bool _doAction = false;

    /// <summary>移動入力の大きさ</summary>
    protected float _moveInputRate = 0f;

    /// <summary>かけるブレーキ力</summary>
    protected Vector3 _ForceOfBrake = Vector3.zero;
    #endregion

    #region プロパティ
    /// <summary>true : 各種アクション入力があった</summary>
    public bool DoAction { get => _doAction; }
    /// <summary>キャラクターの、武器を出したりして臨戦態勢になる継続タイマー</summary>
    public float ArmedTimer { get => _armedTimer; }
    /// <summary>True : 着地している</summary>
    public bool IsGround { get => _gc.IsGround; }
    /// <summary>重力方向</summary>
    public Vector3 GravityDirection { get => _gc.GravityDirection; }
    /// <summary>Rigidbodyのvelocityを移動方向平面に換算したもの</summary>
    public Vector3 VelocityOnPlane { get => Vector3.ProjectOnPlane(_rb.velocity, -GravityDirection); }
    /// <summary>移動向けに力をかける時の力の大きさ</summary>
    public float MovePower { set => _movePower = value; }
    /// <summary>結果の移動速度</summary>
    public float Speed { get => _speed; }
    /// <summary>ジャンプ直後フラグ</summary>
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
        //速度測定
        _speed = VelocityOnPlane.magnitude;
    }

    void FixedUpdate()
    {
        //着地しているか否かで分岐
        //着地中
        if (IsGround)
        {
            //回転する
            Vector3 supVec = Vector3.ProjectOnPlane(_param.LookDirection, -GravityDirection);
            CharacterRotation(supVec, -GravityDirection, 720f);

            //移動力がかかっている
            if (_moveInputRate > 0f)
            {
                //速度制限をかけつつ力をかける
                if(_param.State.Kind == MotionState.StateKind.Run && _speed < _limitSpeedRun)
                {
                    _rb.AddForce(_param.MoveDirection * _moveInputRate * _movePower, ForceMode.Acceleration);
                }
                else if (_speed < _limitSpeedWalk)
                {
                    _rb.AddForce(_param.MoveDirection * _moveInputRate * _movePower, ForceMode.Acceleration);
                }

                //速度(向き)を、入力方向へ設定
                _rb.velocity = Quaternion.FromToRotation(Vector3.ProjectOnPlane(_rb.velocity, -GravityDirection), _param.MoveDirection) * _rb.velocity;

                //重力をかける
                _rb.AddForce(GravityDirection * 2f, ForceMode.Acceleration);
            }
            //移動力がない
            else
            {
                //速度を0に
                _rb.velocity = Vector3.Project(_rb.velocity, -GravityDirection);

                //重力をかける
                _rb.AddForce(GravityDirection * 1f, ForceMode.Acceleration);
            }
        }
        //空中
        else
        {
            //移動力がかかっている
            if (_moveInputRate > 0f)
            {
                //回転する
                CharacterRotation(_param.MoveDirection, -GravityDirection, 90f);

                //力をかける
                _rb.AddForce(_param.MoveDirection * _moveInputRate * _movePower, ForceMode.Acceleration);
            }

            //重力をかける
            _rb.AddForce(GravityDirection * 9.8f, ForceMode.Acceleration);
        }

        //速度減衰をかける
        if (_ForceOfBrake.sqrMagnitude > 0f)
        {
            _rb.AddForce(_ForceOfBrake, ForceMode.Acceleration);
        }
    }

    /// <summary> キャラクターを指定向きに回転させる </summary>
    /// <param name="targetDirection">目標向き</param>
    /// <param name="up">上方向（Vector.Zeroなら上方向を指定しない）</param>
    /// <param name="rotateSpeed">回転速度</param>
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


