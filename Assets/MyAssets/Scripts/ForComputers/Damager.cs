using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    /// <summary>対象キャラクターのパラメータ</summary>
    CharacterParameter _param = null;

    [SerializeField, Tooltip("被ダメージ倍率")]
    float _damageRatio = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _param = GetComponentInParent<CharacterParameter>();
    }

    /// <summary>ダメージ発生</summary>
    /// <param name="damage">基本ダメージ</param>
    /// <param name="impact">衝撃の大きさ</param>
    public void GetDamage(short damage, float impact)
    {
        _param.GaveDamage((short)(damage * _damageRatio), impact);
    }
}
