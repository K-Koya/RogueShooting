using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Header("�ȉ��ɋ��ʃG�t�F�N�g�̃v���n�u���A�T�C��")]

    [SerializeField, Tooltip("AKM�̎ˏo�e�G�t�F�N�g")]
    GameObject _bulletAKMPref = null;

    [SerializeField, Tooltip("M10�̎ˏo�e�G�t�F�N�g")]
    GameObject _bulletM10Pref = null;

    [SerializeField, Tooltip("SAR2000�̎ˏo�e�G�t�F�N�g")]
    GameObject _bulletSAR2000Pref = null;

    [SerializeField, Tooltip("�e���L�����N�^�[�ɓ����������ɔ���������G�t�F�N�g")]
    GameObject _bulletHitCharacterPref = null;

    [SerializeField, Tooltip("�e���n�`�ɓ����������ɔ���������G�t�F�N�g")]
    GameObject _bulletHitGroundPref = null;


    /// <summary>AKM�̎ˏo�e�G�t�F�N�g�̃v�[��</summary>
    GameObjectPool _bulletAKMEffects = null;

    /// <summary>M10�̎ˏo�e�G�t�F�N�g�̃v�[��</summary>
    GameObjectPool _bulletM10Effects = null;

    /// <summary>SAR2000�̎ˏo�e�G�t�F�N�g�̃v�[��</summary>
    GameObjectPool _bulletSAR2000Effects = null;

    /// <summary>�e���L�����N�^�[�ɓ����������̃G�t�F�N�g�̃v�[��</summary>
    GameObjectPool _bulletHitCharacterEffects = null;

    /// <summary>�e���n�`�ɓ����������̃G�t�F�N�g�̃v�[��</summary>
    GameObjectPool _bulletHitGroundEffects = null;



    /// <summary>AKM�̎ˏo�e�G�t�F�N�g�̃v�[��</summary>
    public GameObjectPool BulletAKMEffects => _bulletAKMEffects;

    /// <summary>M10�̎ˏo�e�G�t�F�N�g�̃v�[��</summary>
    public GameObjectPool BulletM10Effects => _bulletM10Effects;

    /// <summary>SAR2000�̎ˏo�e�G�t�F�N�g�̃v�[��</summary>
    public GameObjectPool BulletSAR2000Effects => _bulletSAR2000Effects;

    /// <summary>�e���L�����N�^�[�ɓ����������̃G�t�F�N�g�̃v�[��</summary>
    public GameObjectPool BulletHitCharacterEffects => _bulletHitCharacterEffects;

    /// <summary>�e���n�`�ɓ����������̃G�t�F�N�g�̃v�[��</summary>
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
