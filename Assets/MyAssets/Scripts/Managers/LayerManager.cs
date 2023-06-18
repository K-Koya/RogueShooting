using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager>
{
    [Header("以下に対応レイヤを指定")]

    #region メンバ
    [SerializeField, Tooltip("地面レイヤ")]
    LayerMask _Ground = default;
    [SerializeField, Tooltip("カメラがすり抜ける地面レイヤ")]
    LayerMask _SeeThroughGround = default;
    [SerializeField, Tooltip("敵レイヤ")]
    LayerMask _Enemy = default;
    [SerializeField, Tooltip("プレイヤーのレイヤ")]
    LayerMask _Player = default;
    [SerializeField, Tooltip("味方レイヤ")]
    LayerMask _Allies = default;
    [SerializeField, Tooltip("インタラクト可能な物レイヤ")]
    LayerMask _Prop = default;
    [SerializeField, Tooltip("味方のダメージレイヤ")]
    LayerMask _AlliseDamager = default;
    [SerializeField, Tooltip("敵のダメージレイヤ")]
    LayerMask _EnemyDamager = default;
    #endregion

    #region プロパティ
    /// <summary>地面レイヤ</summary>
    public LayerMask Ground { get => _Ground | _SeeThroughGround; }
    /// <summary>カメラがすり抜ける地面レイヤ</summary>
    public LayerMask SeeThroughGround { get => _SeeThroughGround; }
    /// <summary>敵レイヤ</summary>
    public LayerMask Enemy { get => _Enemy; }
    /// <summary>プレイヤーのレイヤ</summary>
    public LayerMask Player { get => _Player; }
    /// <summary>味方レイヤ</summary>
    public LayerMask Allies { get => _Allies; }
    /// <summary>全ての地面レイヤ</summary>
    public LayerMask AllGround { get => _Ground | _SeeThroughGround; }
    /// <summary>全てのキャラクターのレイヤ</summary>
    public LayerMask AllCharacter { get => _Enemy | _Player | _Allies; }
    /// <summary>全ての味方キャラクターのレイヤ</summary>
    public LayerMask AllAllies { get => _Allies | _Player; }
    /// <summary>レティクルが乗る（ターゲットできるオブジェクト）のレイヤ</summary>
    public LayerMask OnTheReticle { get => _Allies | _Enemy | _Ground | _Prop; }
    /// <summary>敵がターゲットできるオブジェクトのレイヤ</summary>
    public LayerMask EnemyFocusable { get => _Player | _Allies | _Ground | _Prop; }
    /// <summary>銃弾が当たるレイヤ</summary>
    public LayerMask BulletHit { get => _AlliseDamager | _EnemyDamager | _Ground; }

    #endregion
}
