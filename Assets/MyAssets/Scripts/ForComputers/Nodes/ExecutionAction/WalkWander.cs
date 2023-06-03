using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeNode;

[System.Serializable]
public class WalkWander : IExecutionMethod
{
    [SerializeField, Tooltip("うろつきの範囲")]
    float _wanderDistance = 20f;

    [SerializeField, Tooltip("自分の持ち場の基準になる位置")]
    Vector3 _basePosition = default;

    [SerializeField, Tooltip("移動先座標を見つけられない時の動作キャンセル待機時間")]
    float _timeOut = 1f;

    /// <summary>時間計測</summary>
    float _timer = 0f;

    /// <summary>true : 初期化処理済み</summary>
    bool _isInitialized = false;


    public Status Method(ComputerParameter param, ComputerMove move)
    {
        if (!_isInitialized)
        {
            float radius = Random.Range(0f, _wanderDistance);
            float ratio = Random.value;

            move.Destination = new Vector3(radius * ratio, 0f, radius * (1f - ratio)) + _basePosition;
            _timer = _timeOut;
        }

        if (move.IsFoundDestination)
        {
            _isInitialized = true;
        }
        else
        {
            _timer -= Time.deltaTime;
            if(_timer < 0f)
            {
                move.Destination = null;
                return Status.Failure;
            }
        }

        if (move.IsCloseDestination)
        {
            move.Destination = null;
            _isInitialized = false;
            return Status.Success;
        }

        return Status.Running;
    }
}
