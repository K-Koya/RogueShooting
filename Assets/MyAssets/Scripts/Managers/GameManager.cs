using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    /// <summary>  </summary>
    const float _SLOW_SPEED_RATE = 0.1f;

    /// <summary>true : �|�[�Y��</summary>
    static bool _IsPose = false;

    /// <summary>TimeScale���|�[�Y�p�ɕۊ�</summary>
    static float _TimeScaleCache = 1f;

    /// <summary>�w�肳�ꂽ��Փx�f�[�^</summary>
    static DifficultyDataColumn[] _DifficultyData = null;

    [SerializeField, Tooltip("���߂ăV�[���ɓ��������Ɏ��s���郁�\�b�h���A�T�C��")]
    UnityEvent _DoInitialize = null;

    [SerializeField, Tooltip("��Փx�f�[�^����")]
    DifficultyDataColumn[] _difficultyData_Inner = null;



    /// <summary>true : �|�[�Y��</summary>
    public static bool IsPose => _IsPose;

    void Awake()
    {
        CursorMode(true);
        PauseMode(false);
    }

    void Start()
    {
        SlowMode(false);
        _DifficultyData = _difficultyData_Inner;
        _DoInitialize?.Invoke();

        BGMManager.Instance.BGMCallBaseSpace();
    }

    /// <summary>�|�[�Y����</summary>
    /// <param name="isActive">true : �|�[�Y���N��</param>
    public static void PauseMode(bool isActive)
    {
        _IsPose = isActive;

        if (isActive)
        {
            _TimeScaleCache = Time.timeScale;
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = _TimeScaleCache;
        }
    }

    /// <summary>�X���[���o����</summary>
    /// <param name="isActive">true : ���o�N��</param>
    public static void SlowMode(bool isActive)
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
    public static void CursorMode(bool isActive)
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

    /// <summary>��Փx�f�[�^�Ɋi�[����Ă����l���e�v�f�ɔ��f</summary>
    public static void SetDifficulty(int numberOfStage, out int numberOfAllStage)
    {
        numberOfAllStage = _DifficultyData.Length;
        ComputerParameter.SetEnemyData(_DifficultyData[numberOfStage]._quotaDefeated, _DifficultyData[numberOfStage]._baseAccuracyAim);
        MapRandomizer_Plant.MapSize = _DifficultyData[numberOfStage]._stageSize;
        EnemySpawnerPortal.Capacity = _DifficultyData[numberOfStage]._enemyCount;
    }
}

[System.Serializable]
/// <summary>��Փx�e�[�u���̈ꍀ��</summary>
struct DifficultyDataColumn
{
    /// <summary>�}�b�v�̏c���}�X��</summary>
    public Vector2Int _stageSize;

    /// <summary>�����o���l��</summary>
    public byte _enemyCount;

    /// <summary>�N���A�m���}</summary>
    public byte _quotaDefeated;

    /// <summary>��{�Ə����x�i�u���̊�{�l�j</summary>
    public float _baseAccuracyAim;
}


