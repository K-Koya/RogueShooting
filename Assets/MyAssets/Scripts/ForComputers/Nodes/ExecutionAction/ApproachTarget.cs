using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviorTreeNode
{
    public class ApproachTarget : IExecutionMethod
    {
        /// <summary>true : 初期化処理済み</summary>
        bool _isInitialized = false;

        [SerializeField, Tooltip("追跡制限時間")]
        float _timeOut = 20f;

        /// <summary>時間計測</summary>
        float _timer = 0f;

        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //このノードに初めて入った
            if (!_isInitialized)
            {
                _timer = _timeOut;
                _isInitialized = true;
            }

            //ターゲットを見失ったら失敗
            if (!param.Target)
            {
                _isInitialized = false;
                return Status.Failure;
            }

            //常にターゲットを目的地に
            move.Destination = param.Target.transform.position;

            //常にターゲットを注視
            param.LookDirection = Vector3.Normalize(param.Target.EyePoint.position - param.EyePoint.position);

            _timer -= Time.deltaTime;

            //到着したら成功
            if (move.IsCloseDestination)
            {
                move.Destination = null;
                _isInitialized = false;
                return Status.Success;
            }
            else if(_timer < 0f)
            {
                _isInitialized = false;
                return Status.Failure;
            }

            return Status.Running;
        }
    }
}
