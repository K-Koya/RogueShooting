using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : Singleton<TagManager>
{
    [Header("�^�O�����ȉ��ɃA�T�C��")]

    #region �����o
    [SerializeField, Tooltip("���C���J�����̃^�O")]
    string _mainCamera = "MainCamera";
    [SerializeField, Tooltip("�v���C���[�̃^�O")]
    string _player = "Player";
    [SerializeField, Tooltip("�G�̃^�O")]
    string _enemy = "Enemy";
    [SerializeField, Tooltip("�����L�����N�^�[�̃^�O")]
    string _allies = "Allies";
    [SerializeField, Tooltip("�G�̏o���n�_�^�O")]
    string _spawner = "Spawner";
    [SerializeField, Tooltip("���^�O")]
    string _floor = "Floor";
    #endregion

    #region �v���p�e�B
    /// <summary>���C���J�����̃^�O</summary>
    public string MainCamera { get => _mainCamera; }
    /// <summary>�v���C���[�̃^�O</summary>
    public string Player { get => _player; }
    /// <summary>�G�̃^�O</summary>
    public string Enemy { get => _enemy; }
    /// <summary>�����L�����N�^�[�̃^�O</summary>
    public string Allies { get => _allies; }
    /// <summary>�U���������R���C�_�[�̃^�O</summary>
    public string Spawner { get => _spawner; }
    /// <summary>���ƂȂ�R���C�_�[�̃^�O</summary>
    public string Floor { get => _floor; }
    #endregion
}

public static partial class ForRegisterExtensionMethods
{
    public static bool CompareTags(this MonoBehaviour mb, params string[] tags)
    {
        return tags.Count(tag => mb.CompareTag(tag)) > 0;
    }
} 
