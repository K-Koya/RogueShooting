using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.Utilities;

public class GameManager : Singleton<GameManager>
{
    /// <summary>  </summary>
    const float _SLOW_SPEED_RATE = 0.1f;

    /// <summary>true : ポーズ中</summary>
    static bool _IsPose = false;

    /// <summary>TimeScaleをポーズ用に保管</summary>
    float _timeScaleCache = 1f;

    /// <summary>選択したゲームの種類</summary>
    SceneType _gameType = SceneType.Title;

    [SerializeField, Tooltip("初めてシーンに入った時に実行するメソッドをアサイン")]
    UnityEvent _DoInitialize = null;


    [SerializeField, Tooltip("敵討伐モードの難易度データ入力")]
    StageManager_Raid.DifficultyDataColumn[] _difficultyData_Raid = null;



    /// <summary>true : ポーズ中</summary>
    public static bool IsPose => _IsPose;

    /// <summary>敵討伐モードの難易度データ</summary>
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

    /// <summary>ポーズ処理</summary>
    /// <param name="isActive">true : ポーズを起動</param>
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

    /// <summary>スロー演出処理</summary>
    /// <param name="isActive">true : 演出起動</param>
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

    /// <summary>マウスカーソルの表示設定</summary>
    /// <param name="isActive">true : 表示する</param>
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

    /// <summary>ゲームの種類を指定</summary>
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



/// <summary>現シーンの分類</summary>
public enum SceneType : byte
{
    /// <summary>タイトル</summary>
    Title = 0,
    /// <summary>共通チュートリアル</summary>
    Tutorial_Basic,
    /// <summary>チュートリアル:敵討伐</summary>
    Tutorial_Raid = 10,
    /// <summary>ゲームモード:敵討伐</summary>
    InGame_Raid,
    /// <summary>リザルト:敵討伐</summary>
    Result_Raid,
}
