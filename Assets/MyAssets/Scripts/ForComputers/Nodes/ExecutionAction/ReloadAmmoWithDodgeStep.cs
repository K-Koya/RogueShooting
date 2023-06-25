using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class ReloadAmmoWithDodgeStep : IExecutionMethod
    {
        [SerializeField, Tooltip("ステップ距離")]
        float _sideStepDistance = 2f;

        [SerializeField, Tooltip("移動先座標を見つけられない時の動作キャンセル待機時間")]
        float _timeOut = 1f;

        /// <summary>ターゲットとの相対位置</summary>
        Vector3 _relativePos = Vector3.zero;

        /// <summary>時間計測</summary>
        float _timer = 0f;

        /// <summary>true : 初期化処理済み</summary>
        bool _isInitialized = false;

        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //ターゲットをとらえていれば注視
            if (param.IsThroughLineOfSight)
            {
                //ターゲットとの相対位置
                _relativePos = param.Target.EyePoint.position - param.EyePoint.position;

                //常にターゲットを注視
                param.LookDirection = Vector3.Normalize(_relativePos);
            }

            //このノードに初めて入った
            if (!_isInitialized)
            {
                _isInitialized = true;
                param.State.Kind = MotionState.StateKind.Run;

                //ターゲットをとらえていなければ、前方を注視
                if (!param.IsThroughLineOfSight)
                {
                    _relativePos = param.transform.forward;
                }

                //ターゲットに対していずれか横方向
                Vector3 sideDirection = Vector3.Cross(param.LookDirection, -move.GravityDirection);
                if (Random.value > 0.5f) sideDirection *= -1;

                //目的地を設定
                _timer = _timeOut;
                move.Destination = param.transform.position + sideDirection * _sideStepDistance;
            }

            //リロード要求
            param.UsingGun.DoReload();

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

            //リロードが完了したら成功
            if (param.UsingGun.CurrentLoadAmmo >= param.UsingGun.MaxLoadAmmo)
            {
                move.Destination = null;
                _isInitialized = false;
                return Status.Success;
            }

            return Status.Running;
        }
    }
}
