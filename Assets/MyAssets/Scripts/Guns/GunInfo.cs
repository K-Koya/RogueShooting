using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : MonoBehaviour
{
    /// <summary>�A�j���[�^�[�p�����[�^�� : ResultSpeed</summary>
    string _PARAM_NAME_RESULT_SPEED = "ResultSpeed";

    /// <summary>�A�j���[�^�[�p�����[�^�� : IsCation</summary>
    string _PARAM_NAME_IS_CATION = "IsCation";

    /// <summary>�A�j���[�^�[�p�����[�^�� : DoShot</summary>
    string _PARAM_NAME_DO_SHOT = "DoShot";

    /// <summary>�A�j���[�^�[�p�����[�^�� : IsReload</summary>
    string _PARAM_NAME_IS_RELOAD = "IsReload";

    /// <summary>�A�j���[�^�[�p�����[�^�� : IsFreedom</summary>
    string _PARAM_NAME_IS_FREEDOM = "IsFreedom";



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

    [SerializeField, Tooltip("�ˌ����𔭂���X�s�[�J�[")]
    AudioSource _shotSESource = null;

    [SerializeField, Tooltip("�����[�h���𔭂���X�s�[�J�[")]
    AudioSource _reloadSESource = null;

    [SerializeField, Tooltip("�e�̍ő呕�U��")]
    byte _maxLoadAmmo = 15;

    [SerializeField, Tooltip("���݂̒e�̑��U��")]
    byte _currentLoadAmmo = 15;

    [SerializeField, Tooltip("�ˌ��Ԋu")]
    float _shotInterval = 0.3f;

    /// <summary>�e�e���΂���������</summary>
    Vector3 _targetPoint = Vector3.zero;

    /// <summary>true : �����[�h��</summary>
    bool _isReloadInterval = false;



    /// <summary>�ˌ��Ԋu�v���^�C�}�[</summary>
    float _intervalTimer = 0.0f;

    /// <summary>true : �Z�~�I�[�g��</summary>
    bool _isSemiAuto = false;


    #region �f���Q�[�g
    /// <summary>���ˏ���</summary>
    System.Action<Vector3> _DoShot = null;

    /// <summary>�����[�h����</summary>
    System.Action _DoReload = null;

    /// <summary>�e�e��������</summary>
    System.Func<GameObject> _Instantiate = null;
    #endregion

    #region �v���p�e�B
    /// <summary>�e�̎�����̓��A�g���K�[�����������w������ʒu�p�����</summary>
    public Transform GunTriggerHands => _gunTriggerHands;
    /// <summary>�e�̎�����̓��A�x��������w������ʒu�p�����</summary>
    public Transform GunSupportHands => _gunSupportHands;
    /// <summary>�e�̍ő呕�U��</summary>
    public byte MaxLoadAmmo => _maxLoadAmmo;
    /// <summary>���݂̒e�̑��U��</summary>
    public byte CurrentLoadAmmo => _currentLoadAmmo;
    #endregion

    /// <summary>���ˏ���</summary>
    /// <param name="target">�Ə������ɂ������</param>
    public void DoShot(Vector3 target)
    {
        _DoShot?.Invoke(target);
    }

    /// <summary>�����[�h����</summary>
    public void DoReload()
    {
        _DoReload?.Invoke();
    }

    /// <summary>�e������</summary>
    public void DoRelease()
    {
        _anim.SetBool(_PARAM_NAME_IS_FREEDOM, true);
    }

    /// <summary>�e�������</summary>
    public void DoGet()
    {
        _anim.SetBool(_PARAM_NAME_IS_FREEDOM, false);
    }

    /// <summary>���e���ύX</summary>
    public void DoSwitch()
    {
        _anim.SetBool(_PARAM_NAME_IS_RELOAD, false);
    }

    /// <summary>�ړ����x������������擾</summary>
    /// <param name="spd">�ړ����x</param>
    public void SetResultSpeed(float spd)
    {
        _anim.SetFloat(_PARAM_NAME_RESULT_SPEED, spd);
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
                _Instantiate = EffectManager.Instance.BulletSAR2000Effects.Instansiate;
                _isSemiAuto = true;

                break;
            case GunType.Magnum:
                _maxLoadAmmo = (byte)Mathf.Clamp(_maxLoadAmmo, 5, 8);
                _DoShot = DoShotSemiAuto;
                _DoReload = DoReloadSemiAuto;
                _isSemiAuto = true;

                break;
            case GunType.ShotGun:
                _maxLoadAmmo = (byte)Mathf.Clamp(_maxLoadAmmo, 2, 12);
                _DoShot = DoShotShotGun;
                _DoReload = DoReloadShotGun;
                _Instantiate = EffectManager.Instance.BulletM590Effects.Instansiate;
                _isSemiAuto = true;

                break;
            case GunType.SubMachineGun:
                _maxLoadAmmo = (byte)Mathf.Clamp(_maxLoadAmmo, 30, 100);
                _DoShot = DoShotFullAuto;
                _DoReload = DoReloadFullAuto;
                _Instantiate = EffectManager.Instance.BulletM10Effects.Instansiate;
                _isSemiAuto = false;

                break;
            case GunType.AssultRifle:
                _maxLoadAmmo = (byte)Mathf.Clamp(_maxLoadAmmo, 20, 60);
                _DoShot = DoShotFullAuto;
                _DoReload = DoReloadFullAuto;
                _Instantiate = EffectManager.Instance.BulletAKMEffects.Instansiate;
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
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        if (_intervalTimer > 0f)
        {
            _intervalTimer -= Time.deltaTime;
        }
    }

    /// <summary>�x�����[�h���N��</summary>
    /// <param name="flag">true : �N��</param>
    public void CallCationMode(bool flag = true)
    {
        _anim.SetBool(_PARAM_NAME_IS_CATION, flag);
    }

    /// <summary>�Z�~�I�[�g����̎ˌ�</summary>
    void DoShotSemiAuto(Vector3 target)
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

        _targetPoint = target;
        _anim.SetTrigger(_PARAM_NAME_DO_SHOT);
        _intervalTimer = _shotInterval;
    }

    /// <summary>�Z�~�I�[�g����̃����[�h</summary>
    void DoReloadSemiAuto()
    {
        //�c�e�������Ă�������s
        if (!_isReloadInterval && _currentLoadAmmo < _maxLoadAmmo)
        {
            _currentLoadAmmo = 0;
            _anim.SetBool(_PARAM_NAME_IS_RELOAD, true);
            _isReloadInterval = true;
        }
    }

    /// <summary>�V���b�g�K���̎ˌ�</summary>
    void DoShotShotGun(Vector3 target)
    {
        _anim.SetBool(_PARAM_NAME_IS_RELOAD, false);

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

        _targetPoint = target;
        _anim.SetTrigger(_PARAM_NAME_DO_SHOT);
        _intervalTimer = _shotInterval;
    }

    /// <summary>�V���b�g�K���̃����[�h</summary>
    void DoReloadShotGun()
    {
        //�c�e�������Ă�������s
        if (!_isReloadInterval && _currentLoadAmmo < _maxLoadAmmo)
        {
            _anim.SetTrigger(_PARAM_NAME_IS_RELOAD);
            _isReloadInterval = true;
        }
    }

    /// <summary>�t���I�[�g����̎ˌ�</summary>
    void DoShotFullAuto(Vector3 target)
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

        _targetPoint = target;
        _anim.SetTrigger(_PARAM_NAME_DO_SHOT);
        _intervalTimer = _shotInterval;
    }

    /// <summary>�t���I�[�g����̃����[�h</summary>
    void DoReloadFullAuto()
    {
        //�c�e�������Ă�������s
        if (!_isReloadInterval && _currentLoadAmmo < _maxLoadAmmo)
        {
            _currentLoadAmmo = 0;
            _anim.SetBool(_PARAM_NAME_IS_RELOAD, true);
            _isReloadInterval = true;
        }
    }

    #region �A�j���[�^�[�C�x���g�p
    /// <summary>���C���ďe�e���ˏo</summary>
    public void EmitBulletFromGun()
    {
        GameObject ins = _Instantiate();
        ins.transform.position = _shotSESource.transform.position;
        ins.transform.forward = Vector3.Normalize(_targetPoint - _shotSESource.transform.position);
        _currentLoadAmmo--;
    }

    /// <summary>�V���b�g�K���𔭖C���ďe�e���ˏo</summary>
    public void EmitBulletFromShotGun()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject ins = _Instantiate();
            ins.transform.position = _shotSESource.transform.position;
            Vector3 spread = new Vector3(Random.value - 0.5f, Random.value - 0.5f, Random.value - 0.5f) * 6f;
            ins.transform.forward = Vector3.Normalize(_targetPoint + spread - _shotSESource.transform.position);
        }
        _currentLoadAmmo--;
    }

    /// <summary>�����[�h����</summary>
    public void ReloadComprete()
    {
        _currentLoadAmmo = _maxLoadAmmo;
        _anim.SetBool(_PARAM_NAME_IS_RELOAD, false);
        _isReloadInterval = false;
    }

    /// <summary>�ꔭ�����������[�h</summary>
    public void ReloadWithOne()
    {
        _currentLoadAmmo++;
        _anim.SetBool(_PARAM_NAME_IS_RELOAD, false);
        _isReloadInterval = false;
    }

    /// <summary>���������C��</summary>
    public void EmitSEShotSmall()
    {
        _shotSESource.PlayOneShot(SEManager.Instance.GunShotSmall);
    }

    /// <summary>�傫�����C��</summary>
    public void EmitSEShotLarge()
    {
        _shotSESource.PlayOneShot(SEManager.Instance.GunShotLarge);
    }

    /// <summary>�}�K�W�����O����</summary>
    public void EmitSERemoveMagazine()
    {
        _reloadSESource.PlayOneShot(SEManager.Instance.MagazineRemove);
    }

    /// <summary>�}�K�W���𒅂��鉹</summary>
    public void EmitSEConnectMagazine()
    {
        _reloadSESource.PlayOneShot(SEManager.Instance.MagazineConnect);
    }

    /// <summary>�v���o�b�N��</summary>
    public void EmitSEPullBack()
    {
        _reloadSESource.PlayOneShot(SEManager.Instance.PullBack);
    }

    #endregion
}
