using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : Singleton<EffectManager>
{
    [Header("�ȉ��ɋ��ʃp�[�e�B�N���̃v���n�u���A�T�C��")]

    [SerializeField, Tooltip("�}�Y���t���b�V���G�t�F�N�g")]
    GameObject _MuzzleFlashEffectPref = null;


    /// <summary>�}�Y���t���b�V���G�t�F�N�g�̃v�[��</summary>
    GameObjectPool _MuzzleFlashEffects = null;


    /// <summary>�}�Y���t���b�V���G�t�F�N�g�̃v�[��</summary>
    public GameObjectPool MuzzleFlashEffects => _MuzzleFlashEffects;


    // Start is called before the first frame update
    void Start()
    {
        _MuzzleFlashEffects = new GameObjectPool(_MuzzleFlashEffectPref, transform, 20);
    }

}
