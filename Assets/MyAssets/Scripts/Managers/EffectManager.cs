using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Header("以下に共通エフェクトのプレハブをアサイン")]

    [SerializeField, Tooltip("AKMの射出弾エフェクト")]
    GameObject _bulletAKMPref = null;

    [SerializeField, Tooltip("M10の射出弾エフェクト")]
    GameObject _bulletM10Pref = null;

    [SerializeField, Tooltip("SAR2000の射出弾エフェクト")]
    GameObject _bulletSAR2000Pref = null;

    [SerializeField, Tooltip("弾がキャラクターに当たった時に発生させるエフェクト")]
    GameObject _bulletHitCharacterPref = null;

    [SerializeField, Tooltip("弾が地形に当たった時に発生させるエフェクト")]
    GameObject _bulletHitGroundPref = null;


    /// <summary>AKMの射出弾エフェクトのプール</summary>
    GameObjectPool _bulletAKMEffects = null;

    /// <summary>M10の射出弾エフェクトのプール</summary>
    GameObjectPool _bulletM10Effects = null;

    /// <summary>SAR2000の射出弾エフェクトのプール</summary>
    GameObjectPool _bulletSAR2000Effects = null;

    /// <summary>弾がキャラクターに当たった時のエフェクトのプール</summary>
    GameObjectPool _bulletHitCharacterEffects = null;

    /// <summary>弾が地形に当たった時のエフェクトのプール</summary>
    GameObjectPool _bulletHitGroundEffects = null;



    /// <summary>AKMの射出弾エフェクトのプール</summary>
    public GameObjectPool BulletAKMEffects => _bulletAKMEffects;

    /// <summary>M10の射出弾エフェクトのプール</summary>
    public GameObjectPool BulletM10Effects => _bulletM10Effects;

    /// <summary>SAR2000の射出弾エフェクトのプール</summary>
    public GameObjectPool BulletSAR2000Effects => _bulletSAR2000Effects;

    /// <summary>弾がキャラクターに当たった時のエフェクトのプール</summary>
    public GameObjectPool BulletHitCharacterEffects => _bulletHitCharacterEffects;

    /// <summary>弾が地形に当たった時のエフェクトのプール</summary>
    public GameObjectPool BulletHitGroundEffects => _bulletHitGroundEffects;


    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        _bulletAKMEffects = new GameObjectPool(_bulletAKMPref, transform, 20);
        _bulletM10Effects = new GameObjectPool(_bulletM10Pref, transform, 30);
        _bulletSAR2000Effects = new GameObjectPool(_bulletSAR2000Pref, transform, 20);
        _bulletHitCharacterEffects = new GameObjectPool(_bulletHitCharacterPref, transform, 10);
        _bulletHitGroundEffects = new GameObjectPool(_bulletHitGroundPref, transform, 15);
    }

}
