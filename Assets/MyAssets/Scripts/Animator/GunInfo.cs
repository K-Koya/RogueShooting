using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : MonoBehaviour
{
    /// <summary>アニメーターパラメータ名 : ResultSpeed</summary>
    string PARAM_NAME_RESULT_SPEED = "ResultSpeed";

    /// <summary>アニメーターパラメータ名 : IsCation</summary>
    string PARAM_NAME_IS_CATION = "IsCation";

    /// <summary>アニメーターパラメータ名 : DoShot</summary>
    string PARAM_NAME_DO_SHOT = "DoShot";

    /// <summary>アニメーターパラメータ名 : IsReload</summary>
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
    [SerializeField, Tooltip("該当の銃型")]
    GunType type = GunType.HandGun;

    /// <summary>銃のアニメーター</summary>
    Animator _anim = null; 

    [SerializeField, Tooltip("銃の持ち手の内、トリガーを引く方を指示する位置姿勢情報")]
    Transform _gunTriggerHands = null;

    [SerializeField, Tooltip("銃の持ち手の内、支える方を指示する位置姿勢情報")]
    Transform _gunSupportHands = null;

    [SerializeField, Tooltip("弾の最大装填数")]
    byte _maxLoadAmmo = 15;

    [SerializeField, Tooltip("現在の弾の装填数")]
    byte _currentLoadAmmo = 15;

    [SerializeField, Tooltip("射撃間隔")]
    float _shotInterval = 0.3f;

    /// <summary>射撃間隔計測タイマー</summary>
    float _intervalTimer = 0.0f;

    /// <summary>true : セミオート式</summary>
    bool _isSemiAuto = false;



    #region デリゲート
    /// <summary>発射処理</summary>
    System.Action _DoShot = null;

    /// <summary>リロード処理</summary>
    System.Action _DoReload = null;

    #endregion

    #region プロパティ
    /// <summary>銃の持ち手の内、トリガーを引く方を指示する位置姿勢情報</summary>
    public Transform GunTriggerHands => _gunTriggerHands;
    /// <summary>銃の持ち手の内、支える方を指示する位置姿勢情報</summary>
    public Transform GunSupportHands => _gunSupportHands;
    /// <summary>弾の最大装填数</summary>
    public byte MaxLoadAmmo { get => _maxLoadAmmo; }
    /// <summary>現在の弾の装填数</summary>
    public byte CurrentLoadAmmo { get => _currentLoadAmmo; }

    #endregion

    /// <summary>発射処理</summary>
    public void DoShot()
    {
        _DoShot?.Invoke();
    }

    /// <summary>リロード処理</summary>
    public void DoReload()
    {
        _DoReload?.Invoke();
    }

    /// <summary>持つ銃器を変更</summary>
    public void DoSwitch()
    {
        _anim.SetBool(PARAM_NAME_IS_RELOAD, false);
    }

    /// <summary>移動速度情報をこちらも取得</summary>
    /// <param name="spd">移動速度</param>
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

    /// <summary>警戒モードを起動</summary>
    /// <param name="flag">true : 起動</param>
    public void CallCationMode(bool flag = true)
    {
        _anim.SetBool(PARAM_NAME_IS_CATION, flag);
    }

    /// <summary>セミオート武器の射撃</summary>
    void DoShotSemiAuto()
    {
        //残弾が0発なら射撃しない
        if(_currentLoadAmmo < 1)
        {

            return;
        }

        //射撃中は受け付けない
        if (_intervalTimer > 0f)
        {

            return;
        }

        _currentLoadAmmo--;
        _anim.SetTrigger(PARAM_NAME_DO_SHOT);
        _intervalTimer = _shotInterval;
    }

    /// <summary>セミオート武器のリロード</summary>
    void DoReloadSemiAuto()
    {
        //残弾が減っていたら実行
        if (_currentLoadAmmo < _maxLoadAmmo)
        {
            _anim.SetBool(PARAM_NAME_IS_RELOAD, true);
        }
    }

    /// <summary>フルオート武器の射撃</summary>
    void DoShotFullAuto()
    {
        //残弾が0発なら射撃しない
        if (_currentLoadAmmo < 1)
        {

            return;
        }

        //射撃中は受け付けない
        if (_intervalTimer > 0f)
        {

            return;
        }

        _currentLoadAmmo--;
        _anim.SetTrigger(PARAM_NAME_DO_SHOT);
        _intervalTimer = _shotInterval;
    }

    /// <summary>フルオート武器のリロード</summary>
    void DoReloadFullAuto()
    {
        //残弾が減っていたら実行
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
