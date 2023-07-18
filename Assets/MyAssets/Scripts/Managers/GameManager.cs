using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;

public class GameManager : Singleton<GameManager>
{
    /// <summary>  </summary>
    const float _SLOW_SPEED_RATE = 0.1f;

    /// <summary>true : �|�[�Y��</summary>
    static bool _IsPose = false;

    /// <summary>TimeScale���|�[�Y�p�ɕۊ�</summary>
    float _timeScaleCache = 1f;

    /// <summary>�I�������Q�[���̎��</summary>
    SceneType _gameType = SceneType.Title;

    [SerializeField, Tooltip("���߂ăV�[���ɓ��������Ɏ��s���郁�\�b�h���A�T�C��")]
    UnityEvent _DoInitialize = null;


    [SerializeField, Tooltip("�G�������[�h�̓�Փx�f�[�^����")]
    StageManager_Raid.DifficultyDataColumn[] _difficultyData_Raid = null;



    /// <summary>true : �|�[�Y��</summary>
    public static bool IsPose => _IsPose;

    /// <summary>�G�������[�h�̓�Փx�f�[�^</summary>
    public ReadOnlyArray<StageManager_Raid.DifficultyDataColumn> DifficultyData_Raid => _difficultyData_Raid;



    protected override void Awake()
    {
        switch (_gameType)
        {
            case SceneType.Title:
            case SceneType.Result_Raid:
                CursorMode(true);
                PauseMode(false);
                break;
            case SceneType.Tutorial_Basic:
            case SceneType.Tutorial_Raid:
            case SceneType.InGame_Raid:
                CursorMode(false);
                PauseMode(false);
                break;
            default: break;
        }
    }

    void Start()
    {
        SlowMode(false);
        _DoInitialize?.Invoke();

        BGMManager.Instance.BGMCallBaseSpace();
    }

    /// <summary>�|�[�Y����</summary>
    /// <param name="isActive">true : �|�[�Y���N��</param>
    public void PauseMode(bool isActive)
    {
        _IsPose = isActive;

        if (isActive)
        {
            _timeScaleCache = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = _timeScaleCache;
        }
    }

    /// <summary>�X���[���o����</summary>
    /// <param name="isActive">true : ���o�N��</param>
    public void SlowMode(bool isActive)
    {
        if (isActive)
        {
            Time.timeScale = _SLOW_SPEED_RATE;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    /// <summary>�}�E�X�J�[�\���̕\���ݒ�</summary>
    /// <param name="isActive">true : �\������</param>
    public void CursorMode(bool isActive)
    {
        if (isActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    /// <summary>�Q�[���̎�ނ��w��</summary>
    public void SetGame(int enumId)
    {
        _gameType = (SceneType)enumId;
        switch (_gameType)
        {
            case SceneType.Tutorial_Raid:
            case SceneType.InGame_Raid:
            case SceneType.Result_Raid:
                StageManager.SetStageData(DifficultyData_Raid.Count, 0);
                break;
            default: break;
        }
    }
}



/// <summary>���V�[���̕���</summary>
public enum SceneType : byte
{
    /// <summary>�^�C�g��</summary>
    Title = 0,
    /// <summary>���ʃ`���[�g���A��</summary>
    Tutorial_Basic,
    /// <summary>�`���[�g���A��:�G����</summary>
    Tutorial_Raid = 10,
    /// <summary>�Q�[�����[�h:�G����</summary>
    InGame_Raid,
    /// <summary>���U���g:�G����</summary>
    Result_Raid,
}
