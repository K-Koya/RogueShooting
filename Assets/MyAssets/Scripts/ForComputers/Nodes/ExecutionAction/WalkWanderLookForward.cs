using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class WalkWanderLookForward : IExecutionMethod
    {
        [SerializeField, Tooltip("うろつきの範囲")]
        float _wanderDistance = 20f;

        [SerializeField, Tooltip("移動先座標を見つけられない時の動作キャンセル待機時間")]
        float _timeOut = 1f;

        /// <summary>時間計測</summary>
        float _timer = 0f;

        /// <summary>true : 初期化処理済み</summary>
        bool _isInitialized = false;


        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //このノードに初めて入った
            if (!_isInitialized)
            {
                float radius = Random.Range(-_wanderDistance, _wanderDistance);
                float ratio = Random.Range(-1f, 1f);

                move.Destination = new Vector3(radius * ratio, 0f, radius * (1f - ratio)) + param.transform.position;
                _timer = _timeOut;
            }

            //行先が見つかるまで試行
            //一定時間見つからなければ失敗
            if (move.IsFoundDestination)
            {
                _isInitialized = true;
                param.State.Kind = MotionState.StateKind.Walk;
            }
            else
            {
                _timer -= Time.deltaTime;
                if (_timer < 0f)
                {
                    move.Destination = null;
                    _isInitialized = false;
                    return Status.Failure;
                }
            }

            //常に移動方向を注視
            param.LookDirection = param.MoveDirection;

            //到着したら成功
            if (move.IsCloseDestination)
            {
                move.Destination = null;
                _isInitialized = false;
                return Status.Success;
            }

            return Status.Running;
        }
    }
}
