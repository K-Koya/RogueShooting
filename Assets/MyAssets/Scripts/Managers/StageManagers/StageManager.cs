using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class StageManager : MonoBehaviour
{
    /// <summary>テロップを表示させる時間</summary>
    const byte _TELOP_APPEAR_TIME = 5;


    /// <summary>ステージの総数</summary>
    static protected int _NumberOfAllStage = 5;

    /// <summary>現在のステージ番号</summary>
    static protected int _NumberOfCurrentStage = 0;


    [SerializeField, Tooltip("ステージ開始時に表示するテロップ")]
    protected GameObject _stageStartTelop = null;

    [SerializeField, Tooltip("ステージクリア時に表示するテロップ")]
    protected GameObject _stageClearTelop = null;

    [SerializeField, Tooltip("ステージゲームオーバー時に表示するテロップ")]
    protected GameObject _stageFailureTelop = null;

    [SerializeField, Tooltip("ポーズボタン押下時に実行したいメソッド")]
    protected UnityEvent _OnPushPauseButton = null;


    /// <summary>テロップを表示するタイマー</summary>
    protected float _telopAppearTimer = 0f;

    /// <summary>プレイヤーキャラクターのステータス値</summary>
    protected IGetStatus _player = null;

    /// <summary>true : プレイヤーが敵に見つかっている</summary>
    protected bool _isPlayerTargeted = false;

    /// <summary>true : 何らかのステージの終了条件を満たした</summary>
    protected bool _isStageEnd = false;



    /// <summary>ステージの総数</summary>
    public static int NumberOfAllStage => _NumberOfAllStage;

    /// <summary>現在のステージ番号</summary>
    public static int NumberOfCurrentStage => _NumberOfCurrentStage;



    /// <summary>ステージクリア条件</summary>
    abstract protected bool StageClearBorder();
    /// <summary>ステージクリア時に行わせる動作</summary>
    abstract protected void StageCleared();
    /// <summary>ゲームオーバー条件</summary>
    abstract protected bool GameOverBorder();
    /// <summary>ゲームオーバー時に行わせる動作</summary>
    abstract protected void GameOvered();


    /// <summary>ステージ情報を指定</summary>
    public static void SetStageData(int numberOfAllStage, int numberOfCurrentStage)
    {
        _NumberOfAllStage = numberOfAllStage;
        _NumberOfCurrentStage = numberOfCurrentStage;
    }



    protected virtual void Awake()
    {
        _player = FindObjectOfType<PlayerParameter>();
        
        GameManager.Instance.PauseMode(false);
        GameManager.Instance.CursorMode(false);
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        GameManager.Instance.SlowMode(false);

        _telopAppearTimer = _TELOP_APPEAR_TIME;

        _stageStartTelop?.SetActive(true);
        _stageClearTelop?.SetActive(false);
        _stageFailureTelop?.SetActive(false);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //ポーズ時は止める
        if (GameManager.IsPose)
        {
            return;
        }

        //ステージの終了条件を満たしている
        if (_isStageEnd)
        {
            return;
        }

        //ステージ開始時のテロップ表示
        if (_telopAppearTimer > 0f)
        {
            _telopAppearTimer -= Time.deltaTime;
            if (_telopAppearTimer < 0f)
            {
                _stageStartTelop?.SetActive(false);
            }
        }

        //ポーズ
        if (InputUtility.GetDownPause)
        {
            PauseMode(true);
        }

        //クリア条件（ノルマ分敵を倒す）
        if (StageClearBorder())
        {
            StageCleared();
        }
        //ゲームオーバー条件（プレイヤーキャラクターのライフが尽きる）
        else if(GameOverBorder())
        {
            GameOvered();
        }

        //プレイヤーが見つかったらBGMを変える
        bool isTarget = false;
        foreach(CharacterParameter param in CharacterParameter.Enemies)
        {
            if((param as ComputerParameter).Target)
            {
                isTarget = true;
                break;
            }
        }
        if(isTarget != _isPlayerTargeted || CharacterParameter.Enemies.Count < 1)
        {
            BGMManager.Instance.SwitchCallCation(isTarget);
            _isPlayerTargeted = isTarget;
        }
    }

    /// <summary>ポーズ起動・解除</summary>
    /// <param name="isPause">true : ポーズ起動</param>
    public void PauseMode(bool isPause)
    {
        if (isPause)
        {
            _OnPushPauseButton?.Invoke();
        }
        GameManager.Instance.PauseMode(isPause);
        GameManager.Instance.CursorMode(isPause);
    }

    /// <summary>ステージの進行状況をリセットする</summary>
    public void ResetCurrentStage()
    {
        _NumberOfAllStage = 0;
        _NumberOfCurrentStage = 0;
    }
}
