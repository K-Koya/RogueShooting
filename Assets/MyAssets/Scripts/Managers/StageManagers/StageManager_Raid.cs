using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager_Raid : StageManager
{
    [System.Serializable]
    /// <summary>このステージの難易度テーブルの一項目</summary>
    public struct DifficultyDataColumn
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

    protected override void Awake()
    {
        //次のステージのデータを指定
        DifficultyDataColumn data = GameManager.Instance.DifficultyData_Raid[_NumberOfCurrentStage];
        MapRandomizer_Plant.MapSize = data._stageSize;
        ComputerParameter.SetEnemyData(data._quotaDefeated, data._baseAccuracyAim);
        EnemySpawnerPortal.Capacity = data._enemyCount;

        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        BGMManager.Instance.BGMCallField();
    }

    protected override bool GameOverBorder()
    {
        return _player.LifeRatio <= 0;
    }

    protected override void GameOvered()
    {
        _isStageEnd = true;
        BGMManager.Instance.BGMOff();
        ResetCurrentStage();
        _stageFailureTelop.SetActive(true);
        SceneManager.Instance.ChangeScene(SceneManager._SCENE_NAME_TITLE);
    }

    protected override bool StageClearBorder()
    {
        return ComputerParameter.DefeatedEnemyQuota < ComputerParameter.DefeatedEnemyCount + 1;
    }

    protected override void StageCleared()
    {
        _isStageEnd = true;
        _stageClearTelop.SetActive(true);

        _NumberOfCurrentStage++;

        //すべてのステージを完了
        if (_NumberOfCurrentStage > _NumberOfAllStage - 1)
        {
            ResetCurrentStage();
            SceneManager.Instance.ChangeScene(SceneManager._SCENE_NAME_RESULT);
        }
        else
        {
            SceneManager.Instance.ChangeScene(SceneManager._SCENE_NAME_STAGE);
        }

        GameManager.Instance.SlowMode(true);
    }
}
