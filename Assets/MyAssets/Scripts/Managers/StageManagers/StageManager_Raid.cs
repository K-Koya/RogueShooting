using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager_Raid : StageManager
{
    [System.Serializable]
    /// <summary>���̃X�e�[�W�̓�Փx�e�[�u���̈ꍀ��</summary>
    public struct DifficultyDataColumn
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

    protected override void Awake()
    {
        //���̃X�e�[�W�̃f�[�^���w��
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

        //���ׂẴX�e�[�W������
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
