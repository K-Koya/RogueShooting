using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : MonoBehaviour
{
    /// <summary>�A�j���[�^�[�p�����[�^�� : ResultSpeed</summary>
    string PARAM_NAME_RESULT_SPEED = "ResultSpeed";

    /// <summary>�A�j���[�^�[�p�����[�^�� : IsCation</summary>
    string PARAM_NAME_IS_CATION = "IsCation";

    /// <summary>�A�j���[�^�[�p�����[�^�� : DoShot</summary>
    string PARAM_NAME_DO_SHOT = "DoShot";

    /// <summary>�A�j���[�^�[�p�����[�^�� : IsReload</summary>
    string PARAM_NAME_IS_RELOAD = "IsReload";



    public enum GunType : byte
    {
        Prop = 0,
        HandGun = 1,
        Magnum = 2,
        ShotGun = 3,
        SubMachineGun = 4,
        AssultRifle = 5,
        SniperRifle = 6,
        Launcher = 7,
        FlagGrenade = 8,
        SmokeGrenade = 9,
    }
    [SerializeField, Tooltip("�Y���̏e�^")]
    GunType type = GunType.HandGun;

    /// <summary>�e�̃A�j���[�^�[</summary>
    Animator _anim = null; 

    [SerializeField, Tooltip("�e�̎�����̓��A�g���K�[�����������w������ʒu�p�����")]
    Transform _gunTriggerHands = null;

    [SerializeField, Tooltip("�e�̎�����̓��A�x��������w������ʒu�p�����")]
    Transform _gunSupportHands = null;

    [SerializeField, Tooltip("�e�̍ő呕�U��")]
    byte _maxLoadAmmo = 15;

    [SerializeField, Tooltip("���݂̒e�̑��U��")]
    byte _currentLoadAmmo = 15;

    [SerializeField, Tooltip("�ˌ��Ԋu")]
    float _shotInterval = 0.3f;

    /// <summary>�ˌ��Ԋu�v���^�C�}�[</summary>
    float _intervalTimer = 0.0f;

    /// <summary>true : �Z�~�I�[�g��</summary>
    bool _isSemiAuto = false;



    #region �f���Q�[�g
    /// <summary>���ˏ���</summary>
    System.Action _DoShot = null;

    /// <summary>�����[�h����</summary>
    System.Action _DoReload = null;

    #endregion

    #region �v���p�e�B
    /// <summary>�e�̎�����̓��A�g���K�[�����������w������ʒu�p�����</summary>
    public Transform GunTriggerHands => _gunTriggerHands;
    /// <summary>�e�̎�����̓��A�x��������w������ʒu�p�����</summary>
    public Transform GunSupportHands => _gunSupportHands;
    /// <summary>�e�̍ő呕�U��</summary>
    public byte MaxLoadAmmo { get => _maxLoadAmmo; }
    /// <summary>���݂̒e�̑��U��</summary>
    public byte CurrentLoadAmmo { get => _currentLoadAmmo; }

    #endregion

    /// <summary>���ˏ���</summary>
    public void DoShot()
    {
        _DoShot?.Invoke();
    }

    /// <summary>�����[�h����</summary>
    public void DoReload()
    {
        _DoReload?.Invoke();
    }

    /// <summary>���e���ύX</summary>
    public void DoSwitch()
    {
        _anim.SetBool(PARAM_NAME_IS_RELOAD, false);
    }

    /// <summary>�ړ����x������������擾</summary>
    /// <param name="spd">�ړ����x</param>
    public void SetResultSpeed(float spd)
    {
        _anim.SetFloat(PARAM_NAME_RESULT_SPEED, spd);
    }

    void Start()
    {
        _anim = GetComponent<Animator>();

        switch (type)
        {
            case GunType.Prop:
                _maxLoadAmmo = 0;
                _isSemiAuto = true;
                break;
            case GunType.HandGun:
                _maxLoadAmmo = (byte)Mathf.Clamp(_maxLoadAmmo, 6, 16);
                _DoShot = DoShotSemiAuto;
                _DoReload = DoReloadSemiAuto;
                _isSemiAuto = true;

                break;
            case GunType.Magnum:
                _maxLoadAmmo = (byte)Mathf.Clamp(_maxLoadAmmo, 5, 8);
                _DoShot = DoShotSemiAuto;
                _DoReload = DoReloadSemiAuto;
                _isSemiAuto = true;

                break;
            case GunType.AssultRifle:
                _maxLoadAmmo = (byte)Mathf.Clamp(_maxLoadAmmo, 20, 60);
                _DoShot = DoShotFullAuto;
                _DoReload = DoReloadFullAuto;
                _isSemiAuto = false;

                break;
            case GunType.FlagGrenade:
            case GunType.SmokeGrenade:
                _maxLoadAmmo = 1;
                _DoShot = DoShotSemiAuto;
                _DoReload = DoReloadSemiAuto;
                _isSemiAuto = true;

                break;
            default: break;
        }
    }

    void Update()
    {
        if (_intervalTimer > 0f)
        {
            _intervalTimer -= Time.deltaTime;
        }
    }

    /// <summary>�x�����[�h���N��</summary>
    /// <param name="flag">true : �N��</param>
    public void CallCationMode(bool flag = true)
    {
        _anim.SetBool(PARAM_NAME_IS_CATION, flag);
    }

    /// <summary>�Z�~�I�[�g����̎ˌ�</summary>
    void DoShotSemiAuto()
    {
        //�c�e��0���Ȃ�ˌ����Ȃ�
        if(_currentLoadAmmo < 1)
        {

            return;
        }

        //�ˌ����͎󂯕t���Ȃ�
        if (_intervalTimer > 0f)
        {

            return;
        }

        _currentLoadAmmo--;
        _anim.SetTrigger(PARAM_NAME_DO_SHOT);
        _intervalTimer = _shotInterval;
    }

    /// <summary>�Z�~�I�[�g����̃����[�h</summary>
    void DoReloadSemiAuto()
    {
        //�c�e�������Ă�������s
        if (_currentLoadAmmo < _maxLoadAmmo)
        {
            _anim.SetBool(PARAM_NAME_IS_RELOAD, true);
        }
    }

    /// <summary>�t���I�[�g����̎ˌ�</summary>
    void DoShotFullAuto()
    {
        //�c�e��0���Ȃ�ˌ����Ȃ�
        if (_currentLoadAmmo < 1)
        {

            return;
        }

        //�ˌ����͎󂯕t���Ȃ�
        if (_intervalTimer > 0f)
        {

            return;
        }

        _currentLoadAmmo--;
        _anim.SetTrigger(PARAM_NAME_DO_SHOT);
        _intervalTimer = _shotInterval;
    }

    /// <summary>�t���I�[�g����̃����[�h</summary>
    void DoReloadFullAuto()
    {
        //�c�e�������Ă�������s
        if (_currentLoadAmmo < _maxLoadAmmo)
        {
            _anim.SetBool(PARAM_NAME_IS_RELOAD, true);
        }
    }

    public void ReloadComprete()
    {
        _currentLoadAmmo = _maxLoadAmmo;
        _anim.SetBool(PARAM_NAME_IS_RELOAD, false);
    }
}
