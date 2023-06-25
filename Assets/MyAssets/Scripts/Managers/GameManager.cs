using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    /// <summary>  </summary>
    const float _SLOW_SPEED_RATE = 0.1f;

    /// <summary>true : ポーズ中</summary>
    static bool _IsPose = false;

    /// <summary>TimeScaleをポーズ用に保管</summary>
    static float _TimeScaleCache = 1f;

    /// <summary>指定された難易度データ</summary>
    static DifficultyDataColumn[] _DifficultyData = null;

    [SerializeField, Tooltip("初めてシーンに入った時に実行するメソッドをアサイン")]
    UnityEvent _DoInitialize = null;

    [SerializeField, Tooltip("難易度データ入力")]
    DifficultyDataColumn[] _difficultyData_Inner = null;



    /// <summary>true : ポーズ中</summary>
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

    /// <summary>ポーズ処理</summary>
    /// <param name="isActive">true : ポーズを起動</param>
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

    /// <summary>スロー演出処理</summary>
    /// <param name="isActive">true : 演出起動</param>
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

    /// <summary>マウスカーソルの表示設定</summary>
    /// <param name="isActive">true : 表示する</param>
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

    /// <summary>難易度データに格納されている基準値を各要素に反映</summary>
    public static void SetDifficulty(int numberOfStage, out int numberOfAllStage)
    {
        numberOfAllStage = _DifficultyData.Length;
        ComputerParameter.SetEnemyData(_DifficultyData[numberOfStage]._quotaDefeated, _DifficultyData[numberOfStage]._baseAccuracyAim);
        MapRandomizer_Plant.MapSize = _DifficultyData[numberOfStage]._stageSize;
        EnemySpawnerPortal.Capacity = _DifficultyData[numberOfStage]._enemyCount;
    }
}

[System.Serializable]
/// <summary>難易度テーブルの一項目</summary>
struct DifficultyDataColumn
{
    /// <summary>マップの縦横マス数</summary>
    public Vector2Int _stageSize;

    /// <summary>同時出現人数</summary>
    public byte _enemyCount;

    /// <summary>クリアノルマ</summary>
    public byte _quotaDefeated;

    /// <summary>基本照準精度（ブレの基本値）</summary>
    public float _baseAccuracyAim;
}


