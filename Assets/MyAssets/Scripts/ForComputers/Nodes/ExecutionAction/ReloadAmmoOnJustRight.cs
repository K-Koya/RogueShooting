using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class ReloadAmmoOnJustRight : IExecutionMethod
    {
        /// <summary>適正距離とみなす猶予</summary>
        float _DISTANCE_BUFFER = 1f;

        [SerializeField, Tooltip("適性距離")]
        float _justRightDistance = 15f;

        /// <summary>true : 初期化処理済み</summary>
        bool _isInitialized = false;

        /// <summary>true : 適性距離である</summary>
        bool _isJustRight = true;

        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //このノードに初めて入った
            if (!_isInitialized)
            {
                _isInitialized = true;
                param.State.Kind = MotionState.StateKind.Walk;
            }

            //リロード要求
            param.UsingGun.DoReload();

            //ターゲットとの相対位置
            Vector3 relative = param.Target.EyePoint.position - param.EyePoint.position;

            //常にターゲットを注視
            param.LookDirection = Vector3.Normalize(relative);

            //適正距離より近い
            float sqrDistance = Vector3.SqrMagnitude(relative);
            float compare = _justRightDistance - _DISTANCE_BUFFER;
            if (sqrDistance < compare * compare)
            {
                _isJustRight = false;
                param.State.Kind = MotionState.StateKind.Walk;
                move.Destination = param.Target.transform.position - param.LookDirection * _justRightDistance;
            }
            else
            {
                //適正距離より遠い
                compare = _justRightDistance + _DISTANCE_BUFFER;
                if(sqrDistance > compare * compare)
                {
                    _isJustRight = false;
                    param.State.Kind = MotionState.StateKind.Run;
                    move.Destination = param.Target.transform.position - param.LookDirection * _justRightDistance;
                }
                //適正距離である
                else
                {
                    _isJustRight = true;
                    param.State.Kind = MotionState.StateKind.Stay;
                    move.Destination = null;
                }
            }

            //リロードが完了したら成功
            if (param.UsingGun.CurrentLoadAmmo >= param.UsingGun.MaxLoadAmmo)
            {
                move.Destination = null;
                _isInitialized = false;
                param.State.Kind = MotionState.StateKind.Stay;
                return Status.Success;
            }


            return Status.Running;
        }
    }
}
