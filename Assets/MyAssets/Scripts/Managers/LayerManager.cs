using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerManager : Singleton<LayerManager>
{
    [Header("�ȉ��ɑΉ����C�����w��")]

    #region �����o
    [SerializeField, Tooltip("�n�ʃ��C��")]
    LayerMask _Ground = default;
    [SerializeField, Tooltip("�J���������蔲����n�ʃ��C��")]
    LayerMask _SeeThroughGround = default;
    [SerializeField, Tooltip("�G���C��")]
    LayerMask _Enemy = default;
    [SerializeField, Tooltip("�v���C���[�̃��C��")]
    LayerMask _Player = default;
    [SerializeField, Tooltip("�������C��")]
    LayerMask _Allies = default;
    [SerializeField, Tooltip("�C���^���N�g�\�ȕ����C��")]
    LayerMask _Prop = default;
    [SerializeField, Tooltip("�����̃_���[�W���C��")]
    LayerMask _AlliseDamager = default;
    [SerializeField, Tooltip("�G�̃_���[�W���C��")]
    LayerMask _EnemyDamager = default;
    #endregion

    #region �v���p�e�B
    /// <summary>�n�ʃ��C��</summary>
    public LayerMask Ground { get => _Ground | _SeeThroughGround; }
    /// <summary>�J���������蔲����n�ʃ��C��</summary>
    public LayerMask SeeThroughGround { get => _SeeThroughGround; }
    /// <summary>�G���C��</summary>
    public LayerMask Enemy { get => _Enemy; }
    /// <summary>�v���C���[�̃��C��</summary>
    public LayerMask Player { get => _Player; }
    /// <summary>�������C��</summary>
    public LayerMask Allies { get => _Allies; }
    /// <summary>�S�Ă̒n�ʃ��C��</summary>
    public LayerMask AllGround { get => _Ground | _SeeThroughGround; }
    /// <summary>�S�ẴL�����N�^�[�̃��C��</summary>
    public LayerMask AllCharacter { get => _Enemy | _Player | _Allies; }
    /// <summary>�S�Ă̖����L�����N�^�[�̃��C��</summary>
    public LayerMask AllAllies { get => _Allies | _Player; }
    /// <summary>���e�B�N�������i�^�[�Q�b�g�ł���I�u�W�F�N�g�j�̃��C��</summary>
    public LayerMask OnTheReticle { get => _Allies | _Enemy | _Ground | _Prop; }
    /// <summary>�G���^�[�Q�b�g�ł���I�u�W�F�N�g�̃��C��</summary>
    public LayerMask EnemyFocusable { get => _Player | _Allies | _Ground | _Prop; }
    /// <summary>�e�e�������郌�C��</summary>
    public LayerMask BulletHit { get => _AlliseDamager | _EnemyDamager | _Ground; }

    #endregion
}
