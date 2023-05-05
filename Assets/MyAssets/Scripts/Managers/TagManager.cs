using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TagManager : Singleton<TagManager>
{
    [Header("�^�O�����ȉ��ɃA�T�C��")]

    #region �����o
    [SerializeField, Tooltip("���C���J�����̃^�O")]
    string _MainCamera = "MainCamera";
    [SerializeField, Tooltip("�v���C���[�̃^�O")]
    string _Player = "Player";
    [SerializeField, Tooltip("�G�̃^�O")]
    string _Enemy = "Enemy";
    [SerializeField, Tooltip("�����L�����N�^�[�̃^�O")]
    string _Allies = "Allies";
    [SerializeField, Tooltip("OffMeshLink�̂����A�i������艺�肷�郂�m�̃^�O")]
    string _OffMeshLinkJumpStep = "OffMeshLinkJumpStep";
    [SerializeField, Tooltip("OffMeshLink�̂����A�������ɃW�����v���郂�m�̃^�O")]
    string _OffMeshLinkJumpFar = "OffMeshLinkJumpFar";
    [SerializeField, Tooltip("�U���������R���C�_�[�̃^�O")]
    string _AttackCollider = "AttackCollider";
    [SerializeField, Tooltip("�U�����󂯂邱�ƂɂȂ�R���C�_�[�̃^�O")]
    string _DamagedCollider = "DamagedCollider";
    #endregion

    #region �v���p�e�B
    /// <summary>���C���J�����̃^�O</summary>
    public string MainCamera { get => _MainCamera; }
    /// <summary>�v���C���[�̃^�O</summary>
    public string Player { get => _Player; }
    /// <summary>�G�̃^�O</summary>
    public string Enemy { get => _Enemy; }
    /// <summary>�����L�����N�^�[�̃^�O</summary>
    public string Allies { get => _Allies; }
    /// <summary>OffMeshLink�̂����A�i������艺�肷�郂�m�̃^�O</summary>
    public string OffMeshLinkJumpStep { get => _OffMeshLinkJumpStep; }
    /// <summary>OffMeshLink�̂����A�������ɃW�����v���郂�m�̃^�O</summary>
    public string OffMeshLinkJumpFar { get => _OffMeshLinkJumpFar; }
    /// <summary>�U���������R���C�_�[�̃^�O</summary>
    public string AttackCollider { get => _AttackCollider; }
    /// <summary>�U�����󂯂邱�ƂɂȂ�R���C�_�[�̃^�O</summary>
    public string DamagedCollider { get => _DamagedCollider; }

    #endregion
}

public static partial class ForRegisterExtensionMethods
{
    public static bool CompareTags(this MonoBehaviour mb, params string[] tags)
    {
        return tags.Count(tag => mb.CompareTag(tag)) > 0;
    }
} 
