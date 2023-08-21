using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class StageManager : MonoBehaviour
{
    /// <summary>�e���b�v��\�������鎞��</summary>
    const byte _TELOP_APPEAR_TIME = 5;


    /// <summary>�X�e�[�W�̑���</summary>
    static protected int _NumberOfAllStage = 5;

    /// <summary>���݂̃X�e�[�W�ԍ�</summary>
    static protected int _NumberOfCurrentStage = 0;


    [SerializeField, Tooltip("�X�e�[�W�J�n���ɕ\������e���b�v")]
    protected GameObject _stageStartTelop = null;

    [SerializeField, Tooltip("�X�e�[�W�N���A���ɕ\������e���b�v")]
    protected GameObject _stageClearTelop = null;

    [SerializeField, Tooltip("�X�e�[�W�Q�[���I�[�o�[���ɕ\������e���b�v")]
    protected GameObject _stageFailureTelop = null;

    [SerializeField, Tooltip("�|�[�Y�{�^���������Ɏ��s���������\�b�h")]
    protected UnityEvent _OnPushPauseButton = null;


    /// <summary>�e���b�v��\������^�C�}�[</summary>
    protected float _telopAppearTimer = 0f;

    /// <summary>�v���C���[�L�����N�^�[�̃X�e�[�^�X�l</summary>
    protected IGetStatus _player = null;

    /// <summary>true : �v���C���[���G�Ɍ������Ă���</summary>
    protected bool _isPlayerTargeted = false;

    /// <summary>true : ���炩�̃X�e�[�W�̏I�������𖞂�����</summary>
    protected bool _isStageEnd = false;



    /// <summary>�X�e�[�W�̑���</summary>
    public static int NumberOfAllStage => _NumberOfAllStage;

    /// <summary>���݂̃X�e�[�W�ԍ�</summary>
    public static int NumberOfCurrentStage => _NumberOfCurrentStage;



    /// <summary>�X�e�[�W�N���A����</summary>
    abstract protected bool StageClearBorder();
    /// <summary>�X�e�[�W�N���A���ɍs�킹�铮��</summary>
    abstract protected void StageCleared();
    /// <summary>�Q�[���I�[�o�[����</summary>
    abstract protected bool GameOverBorder();
    /// <summary>�Q�[���I�[�o�[���ɍs�킹�铮��</summary>
    abstract protected void GameOvered();


    /// <summary>�X�e�[�W�����w��</summary>
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
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        //�X�e�[�W�̏I�������𖞂����Ă���
        if (_isStageEnd)
        {
            return;
        }

        //�X�e�[�W�J�n���̃e���b�v�\��
        if (_telopAppearTimer > 0f)
        {
            _telopAppearTimer -= Time.deltaTime;
            if (_telopAppearTimer < 0f)
            {
                _stageStartTelop?.SetActive(false);
            }
        }

        //�|�[�Y
        if (InputUtility.GetDownPause)
        {
            PauseMode(true);
        }

        //�N���A�����i�m���}���G��|���j
        if (StageClearBorder())
        {
            StageCleared();
        }
        //�Q�[���I�[�o�[�����i�v���C���[�L�����N�^�[�̃��C�t���s����j
        else if(GameOverBorder())
        {
            GameOvered();
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
        GameManager.Instance.PauseMode(isPause);
        GameManager.Instance.CursorMode(isPause);
    }

    /// <summary>�X�e�[�W�̐i�s�󋵂����Z�b�g����</summary>
    public void ResetCurrentStage()
    {
        _NumberOfAllStage = 0;
        _NumberOfCurrentStage = 0;
    }
}
