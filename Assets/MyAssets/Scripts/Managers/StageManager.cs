using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class StageManager : MonoBehaviour
{
    /// <summary>�e���b�v��\�������鎞��</summary>
    const byte _TELOP_APPEAR_TIME = 5;


    /// <summary>�X�e�[�W�̑���</summary>
    static int _NumberOfAllStage = 5;

    /// <summary>���݂̃X�e�[�W�ԍ�</summary>
    static int _NumberOfCurrentStage = 0;


    [SerializeField, Tooltip("�X�e�[�W�J�n���ɕ\������e���b�v")]
    GameObject _stageStartTelop = null;

    [SerializeField, Tooltip("�X�e�[�W�N���A���ɕ\������e���b�v")]
    GameObject _stageClearTelop = null;

    [SerializeField, Tooltip("�X�e�[�W�Q�[���I�[�o�[���ɕ\������e���b�v")]
    GameObject _stageFailureTelop = null;

    [SerializeField, Tooltip("�|�[�Y�{�^���������Ɏ��s���������\�b�h")]
    UnityEvent _OnPushPauseButton = null;


    /// <summary>�e���b�v��\������^�C�}�[</summary>
    float _telopAppearTimer = 0f;

    /// <summary>�v���C���[�L�����N�^�[�̃X�e�[�^�X�l</summary>
    IGetStatus _player = null;

    /// <summary>true : �v���C���[���G�Ɍ������Ă���</summary>
    bool _isPlayerTargeted = false;

    /// <summary>true : ���炩�̃X�e�[�W�̏I�������𖞂�����</summary>
    bool _isStageEnd = false;



    /// <summary>�X�e�[�W�̑���</summary>
    public static int NumberOfAllStage => _NumberOfAllStage;

    /// <summary>���݂̃X�e�[�W�ԍ�</summary>
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
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        //�X�e�[�W�̏I�������𖞂����Ă���
        if (_isStageEnd)
        {
            GameManager.SlowMode(true);
            return;
        }

        //�X�e�[�W�J�n���̃e���b�v�\��
        if (_telopAppearTimer > 0f)
        {
            _telopAppearTimer -= Time.deltaTime;
            if (_telopAppearTimer < 0f)
            {
                _stageStartTelop.SetActive(false);
            }
        }

        //�|�[�Y
        if (InputUtility.GetDownPause)
        {
            PauseMode(true);
        }

        //�N���A�����i�m���}���G��|���j
        if (ComputerParameter.DefeatedEnemyQuota < ComputerParameter.DefeatedEnemyCount + 1)
        {
            _isStageEnd = true;
            _stageClearTelop.SetActive(true);

            //���ׂẴX�e�[�W������
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
        //�Q�[���I�[�o�[�����i�v���C���[�L�����N�^�[�̃��C�t���s����j
        else if(_player.LifeRatio <= 0)
        {
            _isStageEnd = true;
            BGMManager.Instance.BGMOff();
            ResetCurrentStage();
            _stageFailureTelop.SetActive(true);
            SceneManager.Instance.ChangeScene(SceneManager._SCENE_NAME_TITLE);
        }

        //�v���C���[������������BGM��ς���
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

    /// <summary>�|�[�Y�N���E����</summary>
    /// <param name="isPause">true : �|�[�Y�N��</param>
    public void PauseMode(bool isPause)
    {
        if (isPause)
        {
            _OnPushPauseButton?.Invoke();
        }
        GameManager.PauseMode(isPause);
        GameManager.CursorMode(isPause);
    }

    /// <summary>�X�e�[�W�̐i�s�󋵂����Z�b�g����</summary>
    public void ResetCurrentStage()
    {
        _NumberOfCurrentStage = 0;
    }
}
