using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Header("以下に共通エフェクトのプレハブをアサイン")]

    [SerializeField, Tooltip("AKMの射出弾エフェクト")]
    GameObject _bulletAKMPref = null;

    [SerializeField, Tooltip("SAR2000の射出弾エフェクト")]
    GameObject _bulletSAR2000Pref = null;



    /// <summary>AKMの射出弾エフェクトのプール</summary>
    GameObjectPool _bulletAKMEffects = null;

    /// <summary>SAR2000の射出弾エフェクトのプール</summary>
    GameObjectPool _bulletSAR2000Effects = null;



    /// <summary>AKMの射出弾エフェクトのプール</summary>
    public GameObjectPool BulletAKMEffects => _bulletAKMEffects;

    /// <summary>SAR2000の射出弾エフェクトのプール</summary>
    public GameObjectPool BulletSAR2000Effects => _bulletSAR2000Effects;




    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        _bulletAKMEffects = new GameObjectPool(_bulletAKMPref, transform, 60);
        _bulletSAR2000Effects = new GameObjectPool(_bulletSAR2000Pref, transform, 20);
    }

}
