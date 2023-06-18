using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ComputerMove : CharacterMove
{
    /// <summary>該当キャラクターのNavMeshAgent</summary>
    NavMeshAgent _nav = null;

    /// <summary>指定する移動先座標</summary>
    Vector3? _destination = null;

    /// <summary>発見していてたどり着ける移動先座標</summary>
    Vector3? _resultDestination = null;

    /// <summary>力をかける補正値</summary>
    Vector3 _forceCorrection = Vector3.zero;

    /// <summary>移動先を定めるコルーチン</summary>
    Coroutine _setDestinationCoroutine = null;


    #region ナビメッシュ用メンバ
    [SerializeField, Tooltip("目的地に接近したとみなす距離")]
    float _closeDistance = 3f;

    [SerializeField, Tooltip("true : 移動先をNavMesh上に見つけられた")]
    bool _isFoundDestination = false;

    [SerializeField, Tooltip("true : 移動先座標に接近した")]
    bool _isCloseDestination = true;


    #endregion

    #region プロパティ
    /// <summary>指定する移動先座標</summary>
    public Vector3? Destination { set => _destination = value; }

    /// <summary>発見していてたどり着ける移動先座標</summary>
    public Vector3? ResultDestination { get => _resultDestination; }

    /// <summary>力をかける補正値</summary>
    public Vector3 ForceCorrection { set => _forceCorrection = value; }

    /// <summary>ナビメッシュ上における移動先座標</summary>
    public Vector3 DestinationOnNavMesh { get => _nav.destination; }

    /// <summary>true : 移動先をNavMesh上に見つけられた</summary>
    public bool IsFoundDestination { get => _isFoundDestination; }

    /// <summary>true : 移動先座標に接近した</summary>
    public bool IsCloseDestination { get => _isCloseDestination; }

    #endregion

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _nav = GetComponent<NavMeshAgent>();
        _nav.enabled = false;
        _rb.useGravity = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        //ポーズ時は止める
        if (GameManager.IsPose)
        {
            return;
        }

        base.Update();

        //ナビメッシュエージェントを、わずかに時間をおいてから有効化
        if (!_nav.enabled)
        {
            _nav.enabled = true;
            _nav.isStopped = true;

            _destination = null;
            _resultDestination = null;
        }

        if (_param.State.Kind is MotionState.StateKind.Defeat)
        {
            _isCloseDestination = true;
            _setDestinationCoroutine = null;
            _nav.ResetPath();
            _destination = null;
            _resultDestination = null;

            return;
        }

        MoveByNavMesh();
        SetResultDestination();
    }

    /// <summary>ナビメッシュを利用した移動メソッド</summary>
    void MoveByNavMesh()
    {
        _isCloseDestination = false;
        if (_destination == null)
        {
            _setDestinationCoroutine = null;
        }
        //目的地指定があればコルーチンを実行
        else
        {
            //目的地についたならコルーチンは止める
            if (_resultDestination is not null && Vector3.SqrMagnitude((Vector3)_resultDestination - transform.position) < _closeDistance * _closeDistance)
            {
                _isCloseDestination = true;
                _setDestinationCoroutine = null;
                _nav.ResetPath();
                _destination = null;
                _resultDestination = null;
            }
            //コルーチン未実行であれば実行
            else if (_setDestinationCoroutine == null)
            {
                _setDestinationCoroutine = StartCoroutine(DestinationSetOnAgent());
            }
        }

        //経路パス一覧より、極めて近すぎでない、直近の位置を取得する
        Vector3 currentNextPassing = transform.position;
        foreach (Vector3 passing in _nav.path.corners)
        {
            if (Vector3.SqrMagnitude(passing - transform.position) > 0.01f)
            {
                currentNextPassing = passing;
                break;
            }
        }

        //移動先座標を指定していれば、直近の通過ポイントに向けて力をかける
        if ((_param.Can & MotionEnableFlag.Walk) == MotionEnableFlag.Walk && _destination == null)
        {
            _param.MoveDirection = Vector3.zero;
        }
        else
        {
            _param.MoveDirection = Vector3.Normalize(currentNextPassing - transform.position);
        }

        //移動力補正を合算
        _param.MoveDirection += _forceCorrection;

        //入力があれば移動力の処理
        if (_param.MoveDirection.sqrMagnitude > 0f)
        {
            //移動入力の大きさを取得
            _moveInputRate = _param.MoveDirection.magnitude;
            //移動方向を取得
            _param.MoveDirection *= 1f / _moveInputRate;
            //移動力を計算
            _movePower = 5f;
        }
        else
        {
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

    /// <summary>NavMeshAgentのDestinationに一定間隔で目的地を指示するコルーチン</summary>
    IEnumerator DestinationSetOnAgent()
    {
        while (true)
        {
            _isFoundDestination = false;

            if (_destination == null)
            {
                yield return null;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast((Vector3)_destination + Vector3.up * 0.2f, Vector3.down, out hit, 10f, LayerManager.Instance.AllGround))
                {
                    _nav.destination = hit.point;
                    _isFoundDestination = true;
                }

                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    /// <summary>たどり着ける移動先座標を指定</summary>
    void SetResultDestination()
    {
        if (_nav.path.corners.Length > 1)
        {
            _resultDestination = _nav.path.corners.Last();
        }
        else
        {
            _resultDestination = null;
        }
    }

#if UNITY_EDITOR
    /// <summary>ナビメッシュによる移動経路を書き出し</summary>
    void OnDrawGizmos()
    {
        if (_nav && _nav.enabled)
        {
            Gizmos.color = Color.red;
            var prefPos = transform.position;

            foreach (var pos in _nav.path.corners)
            {
                Gizmos.DrawLine(prefPos, pos);
                prefPos = pos;
            }
        }
    }
#endif

}
