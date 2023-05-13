using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ComputerParameter))]
[RequireComponent(typeof(ComputerMove))]
public class AIEnemyBase : MonoBehaviour
{
    [SerializeField, Tooltip("うろつきの範囲")]
    float _wanderDistance = 20f;

    /// <summary>該当キャラクターのパラメータ</summary>
    ComputerParameter _param = null;

    /// <summary>該当キャラクターの移動指示コンポーネント</summary>
    ComputerMove _move = null;

    /// <summary>自分の持ち場の基準になる位置</summary>
    Vector3 _basePosition = default;

    // Start is called before the first frame update
    void Start()
    {
        _param = GetComponent<ComputerParameter>();
        _move = GetComponent<ComputerMove>();

        _basePosition = transform.position;

        WanderingFromCurrentPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (_move.IsCloseDestination || !_move.IsFoundDestination)
        {
            _param.State.Kind = MotionState.StateKind.Walk;
            WanderingFromBasePos();
        }
    }

    /// <summary>正面を見つつその場で待機</summary>
    void IdleAndLookForward()
    {
        _move.Destination = null;
    }

    /// <summary>周囲を見つつその場で待機</summary>
    void IdleAndLookAround()
    {
        IdleAndLookForward();
    }

    /// <summary>ベースポジション付近をうろつく動作</summary>
    void WanderingFromBasePos()
    {
        float radius = Random.Range(0f, _wanderDistance);
        float ratio = Random.value;

        _move.Destination = new Vector3(radius * ratio, 0f, radius * (1f - ratio)) + _basePosition;
    }

    /// <summary>現在地付近をうろつく動作</summary>
    void WanderingFromCurrentPos()
    {
        float radius = Random.Range(0f, _wanderDistance);
        float ratio = Random.value;

        _move.Destination = new Vector3(radius * ratio, 0f, radius * (1f - ratio)) + transform.position;
    }
}
