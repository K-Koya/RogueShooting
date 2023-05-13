using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤー用の照準移動処理
/// </summary>
public class AimMovement : MonoBehaviour
{
    #region メンバ

    /// <summary>メインカメラコンポーネント</summary>
    Camera _mainCamera = null;

    /// <summary>プレイヤーのパラメータ</summary>
    PlayerParameter _param = null;


    /// <summary>照準までの距離の実数値</summary>
    float _distance = 0.0f;

    /// <summary>照準までの距離の識別</summary>
    DistanceType _distanceType = DistanceType.OutOfRange;
    #endregion


    #region プロパティ
    /// <summary>照準までの距離の実数値</summary>
    public float Distance { get => _distance; }
    /// <summary>照準までの距離の識別</summary>
    public DistanceType DistType { get => _distanceType; }
    #endregion


    // Start is called before the first frame update
    void Start()
    {
        _mainCamera = Camera.main;
        _param = FindObjectOfType<PlayerParameter>();
    }


    void FixedUpdate()
    {
        //地面レイ検索用クラス
        RaycastHit rayhitGround = default;
        //Rayの地面への接触点
        Vector3 rayhitPos = Vector3.zero;

        //プレイヤー位置からカメラ前方方向に地面を探索
        if (Physics.Raycast(_param.EyePoint.transform.position, _mainCamera.transform.forward, out rayhitGround, _param.LockMaxRange, LayerManager.Instance.OnTheReticle))
        {
            //確認できたら該当座標を保存
            rayhitPos = rayhitGround.point;

            //(所持していれば)対象のステータスコンポーネントを取得
            //_Param.GazeAt = rayhitGround.transform.GetComponent<PlayerParameter>();
            
            //照準位置までの実数距離から識別値を設定
            if (_distance < _param.ProximityRange)
            {
                _distanceType = DistanceType.WithinProximity;
            }
            else if (_distance < _param.LockMaxRange)
            {
                _distanceType = DistanceType.OutOfProximity;
            }
        }
        else
        {
            //確認できなければ、最大射程距離を参照
            rayhitPos = _param.EyePoint.transform.position + _mainCamera.transform.forward * _param.LockMaxRange;
            //照準までの距離の識別値を射程外に
            _distanceType = DistanceType.OutOfRange;
            //ステータスコンポーネントを破棄
            //_Param.GazeAt = null;
        }

        //照準を配置
        transform.position = rayhitPos;
        _param.ReticlePoint = rayhitPos;

        //照準位置までの距離を計算(各プレイヤーの最大射程距離を限界値とする)
        _distance = Vector3.Distance(transform.position, _param.EyePoint.transform.position);
    }
}

/// <summary>
/// 照準までの距離の識別値
/// </summary>
public enum DistanceType : byte
{
    /// <summary>
    /// 射程外
    /// </summary>
    OutOfRange,
    /// <summary>
    /// 近接攻撃範囲外
    /// </summary>
    OutOfProximity,
    /// <summary>
    /// 近接攻撃範囲内
    /// </summary>
    WithinProximity
}
