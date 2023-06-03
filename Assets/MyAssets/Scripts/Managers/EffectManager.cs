using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Header("以下に共通パーティクルのプレハブをアサイン")]

    [SerializeField, Tooltip("マズルフラッシュエフェクト")]
    GameObject _MuzzleFlashEffectPref = null;


    /// <summary>マズルフラッシュエフェクトのプール</summary>
    GameObjectPool _MuzzleFlashEffects = null;


    /// <summary>マズルフラッシュエフェクトのプール</summary>
    public GameObjectPool MuzzleFlashEffects => _MuzzleFlashEffects;


    // Start is called before the first frame update
    void Start()
    {
        _MuzzleFlashEffects = new GameObjectPool(_MuzzleFlashEffectPref, transform, 20);
    }

}
