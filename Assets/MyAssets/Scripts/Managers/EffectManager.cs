using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Header("�ȉ��ɋ��ʃG�t�F�N�g�̃v���n�u���A�T�C��")]

    [SerializeField, Tooltip("AKM�̎ˏo�e�G�t�F�N�g")]
    GameObject _bulletAKMPref = null;

    [SerializeField, Tooltip("SAR2000�̎ˏo�e�G�t�F�N�g")]
    GameObject _bulletSAR2000Pref = null;



    /// <summary>AKM�̎ˏo�e�G�t�F�N�g�̃v�[��</summary>
    GameObjectPool _bulletAKMEffects = null;

    /// <summary>SAR2000�̎ˏo�e�G�t�F�N�g�̃v�[��</summary>
    GameObjectPool _bulletSAR2000Effects = null;



    /// <summary>AKM�̎ˏo�e�G�t�F�N�g�̃v�[��</summary>
    public GameObjectPool BulletAKMEffects => _bulletAKMEffects;

    /// <summary>SAR2000�̎ˏo�e�G�t�F�N�g�̃v�[��</summary>
    public GameObjectPool BulletSAR2000Effects => _bulletSAR2000Effects;




    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();

        _bulletAKMEffects = new GameObjectPool(_bulletAKMPref, transform, 60);
        _bulletSAR2000Effects = new GameObjectPool(_bulletSAR2000Pref, transform, 20);
    }

}
