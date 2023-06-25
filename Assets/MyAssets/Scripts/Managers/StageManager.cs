using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    /// <summary>テロップを表示させる時間</summary>
    const byte _TELOP_APPEAR_TIME = 5;


    /// <summary>ステージの総数</summary>
    static int _NumberOfAllStage = 5;

    /// <summary>現在のステージ番号</summary>
    static int _NumberOfCurrentStage = 0;


    [SerializeField, Tooltip("ステージ開始時に表示するテロップ")]
    GameObject _stageStartTelop = null;

    [SerializeField, Tooltip("ステージクリア時に表示するテロップ")]
    GameObject _stageClearTelop = null;

    [SerializeField, Tooltip("ステージゲームオーバー時に表示するテロップ")]
    GameObject _stageFailureTelop = null;

    [SerializeField, Tooltip("ポーズボタン押下時に実行したいメソッド")]
    UnityEvent _OnPushPauseButton = null;


    /// <summary>テロップを表示するタイマー</summary>
    float _telopAppearTimer = 0f;

    /// <summary>プレイヤーキャラクターのステータス値</summary>
    IGetStatus _player = null;

    /// <summary>true : プレイヤーが敵に見つかっている</summary>
    bool _isPlayerTargeted = false;

    /// <summary>true : 何らかのステージの終了条件を満たした</summary>
    bool _isStageEnd = false;



    /// <summary>ステージの総数</summary>
    public static int NumberOfAllStage => _NumberOfAllStage;

    /// <summary>現在のステージ番号</summary>
    public static int NumberOfCurrentStage => _NumberOfCurrentStage;



    void Awake()
    {
        GameManager.SetDifficulty(_NumberOfCurrentStage, out _NumberOfAllStage);
        _player = FindObjectOfType<PlayerParameter>();
        _NumberOfCurrentStage++;

        GameManager.PauseMode(false);
        GameManager.CursorMode(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        GameManager.SlowMode(false);

        _telopAppearTimer = _TELOP_APPEAR_TIME;

        _stageStartTelop.SetActive(true);
        _stageClearTelop.SetActive(false);
        _stageFailureTelop.SetActive(false);

        BGMManager.Instance.BGMCallField();
    }

    // Update is called once per frame
    void Update()
    {
        //ポーズ時は止める
        if (GameManager.IsPose)
        {
            return;
        }

        //ステージの終了条件を満たしている
        if (_isStageEnd)
        {
            GameManager.SlowMode(true);
            return;
        }

        //ステージ開始時のテロップ表示
        if (_telopAppearTimer > 0f)
        {
            _telopAppearTimer -= Time.deltaTime;
            if (_telopAppearTimer < 0f)
            {
                _stageStartTelop.SetActive(false);
            }
        }

        //ポーズ
        if (InputUtility.GetDownPause)
        {
            PauseMode(true);
        }

        //クリア条件（ノルマ分敵を倒す）
        if (ComputerParameter.DefeatedEnemyQuota < ComputerParameter.DefeatedEnemyCount + 1)
        {
            _isStageEnd = true;
            _stageClearTelop.SetActive(true);

            //すべてのステージを完了
            if(_NumberOfCurrentStage > _NumberOfAllStage - 1)
            {
                ResetCurrentStage();
                SceneManager.Instance.ChangeScene(SceneManager._SCENE_NAME_RESULT);
            }
            else
            {
                SceneManager.Instance.ChangeScene(SceneManager._SCENE_NAME_STAGE);
            }
        }
        //ゲームオーバー条件（プレイヤーキャラクターのライフが尽きる）
        else if(_player.LifeRatio <= 0)
        {
            _isStageEnd = true;
            BGMManager.Instance.BGMOff();
            ResetCurrentStage();
            _stageFailureTelop.SetActive(true);
            SceneManager.Instance.ChangeScene(SceneManager._SCENE_NAME_TITLE);
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
        GameManager.PauseMode(isPause);
        GameManager.CursorMode(isPause);
    }

    /// <summary>ステージの進行状況をリセットする</summary>
    public void ResetCurrentStage()
    {
        _NumberOfCurrentStage = 0;
    }
}
