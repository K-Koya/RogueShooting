using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class ShotTargetWithDodgeStep : IExecutionMethod
    {
        [SerializeField, Tooltip("照準ブレの大きさ")]
        float _noiseSize = 3.0f;

        [SerializeField, Tooltip("ステップ距離")]
        float _sideStepDistance = 2f;

        [SerializeField, Tooltip("移動先座標を見つけられない時の動作キャンセル待機時間")]
        float _timeOut = 1f;

        /// <summary>時間計測</summary>
        float _timer = 0f;

        /// <summary>true : 初期化処理済み</summary>
        bool _isInitialized = false;

        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //ターゲットとの相対位置
            Vector3 relative = param.Target.EyePoint.position - param.EyePoint.position;

            //常にターゲットを注視
            param.LookDirection = Vector3.Normalize(relative);

            //このノードに初めて入った
            if (!_isInitialized)
            {
                _isInitialized = true;
                param.State.Kind = MotionState.StateKind.Run;

                //ターゲットに対していずれか横方向
                Vector3 sideDirection = Vector3.Cross(param.LookDirection, -move.GravityDirection);
                if (Random.value > 0.5f) sideDirection *= -1;

                //目的地を設定
                _timer = _timeOut;
                move.Destination = param.transform.position + sideDirection * _sideStepDistance;
            }

            //ターゲットを見失うか装填した弾が尽きたら失敗
            if (!param.IsThroughLineOfSight || param.UsingGun.CurrentLoadAmmo < 1)
            {
                _isInitialized = false;
                move.Destination = null;
                param.State.Kind = MotionState.StateKind.Stay;
                return Status.Failure;
            }

            //ターゲットに向けて撃つ
            Vector3 noise = _noiseSize * ComputerParameter.BaseAccuracyAim * new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * 2f;
            param.UsingGun.DoShot(noise + param.Target.EyePoint.position);


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
