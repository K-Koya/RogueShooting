using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

namespace BehaviorTreeNode
{
    [System.Serializable]
    public class ShotTarget : IExecutionMethod
    {
        [SerializeField, Tooltip("照準ブレの大きさ")]
        float _noiseSize = 3.0f;

        /// <summary>true : 初期化処理済み</summary>
        bool _isInitialized = false;


        public Status Method(ComputerParameter param, ComputerMove move)
        {
            //このノードに初めて入った
            if (!_isInitialized)
            {
                _isInitialized = true;
            }

            //ターゲットを見失ったら失敗
            if (!param.Target)
            {
                _isInitialized = false;
                return Status.Failure;
            }

            //装填した弾が尽きたら失敗
            if (param.UsingGun.CurrentLoadAmmo < 1)
            {
                _isInitialized = false;
                return Status.Failure;
            }

            //ターゲットに向けて撃つ
            Vector3 noise = new Vector3((Random.value - 0.5f) * 2f, (Random.value - 0.5f) * 2f, (Random.value - 0.5f) * 2f) * _noiseSize * ComputerParameter.BaseAccuracyAim;
            param.UsingGun.DoShot(noise + param.Target.EyePoint.position);

            //常にターゲットを注視
            param.LookDirection = Vector3.Normalize(param.Target.EyePoint.position - param.EyePoint.position);

            return Status.Running;
        }
    }
}
