using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunInfo : MonoBehaviour
{
    /// <summary>アニメーターパラメータ名 : ResultSpeed</summary>
    string _PARAM_NAME_RESULT_SPEED = "ResultSpeed";

    /// <summary>アニメーターパラメータ名 : IsCation</summary>
    string _PARAM_NAME_IS_CATION = "IsCation";

    /// <summary>アニメーターパラメータ名 : DoShot</summary>
    string _PARAM_NAME_DO_SHOT = "DoShot";

    /// <summary>アニメーターパラメータ名 : IsReload</summary>
    string _PARAM_NAME_IS_RELOAD = "IsReload";

    /// <summary>アニメーターパラメータ名 : IsFreedom</summary>
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
    [SerializeField, Tooltip("該当の銃型")]
    GunType type = GunType.HandGun;


    /// <summary>銃のアニメーター</summary>
    Animator _anim = null; 

    [SerializeField, Tooltip("銃の持ち手の内、トリガーを引く方を指示する位置姿勢情報")]
    Transform _gunTriggerHands = null;

    [SerializeField, Tooltip("銃の持ち手の内、支える方を指示する位置姿勢情報")]
    Transform _gunSupportHands = null;

    [SerializeField, Tooltip("射撃音を発するスピーカー")]
    AudioSource _shotSESource = null;

    [SerializeField, Tooltip("リロード音を発するスピーカー")]
    AudioSource _reloadSESource = null;

    [SerializeField, Tooltip("弾の最大装填数")]
    byte _maxLoadAmmo = 15;

    [SerializeField, Tooltip("現在の弾の装填数")]
    byte _currentLoadAmmo = 15;

    [SerializeField, Tooltip("射撃間隔")]
    float _shotInterval = 0.3f;

    /// <summary>銃弾を飛ばしたい方向</summary>
    Vector3 _targetPoint = Vector3.zero;

    /// <summary>true : リロード中</summary>
    bool _isReloadInterval = false;



    /// <summary>射撃間隔計測タイマー</summary>
    float _intervalTimer = 0.0f;

    /// <summary>true : セミオート式</summary>
    bool _isSemiAuto = false;


    #region デリゲート
    /// <summary>発射処理</summary>
    System.Action<Vector3> _DoShot = null;

    /// <summary>リロード処理</summary>
    System.Action _DoReload = null;

    /// <summary>銃弾生成処理</summary>
    System.Func<GameObject> _Instantiate = null;
    #endregion

    #region プロパティ
    /// <summary>銃の持ち手の内、トリガーを引く方を指示する位置姿勢情報</summary>
    public Transform GunTriggerHands => _gunTriggerHands;
    /// <summary>銃の持ち手の内、支える方を指示する位置姿勢情報</summary>
    public Transform GunSupportHands => _gunSupportHands;
    /// <summary>弾の最大装填数</summary>
    public byte MaxLoadAmmo => _maxLoadAmmo;
    /// <summary>現在の弾の装填数</summary>
    public byte CurrentLoadAmmo => _currentLoadAmmo;
    #endregion

    /// <summary>発射処理</summary>
    /// <param name="target">照準方向にあるもの</param>
    public void DoShot(Vector3 target)
    {
        _DoShot?.Invoke(target);
    }

    /// <summary>リロード処理</summary>
    public void DoReload()
    {
        _DoReload?.Invoke();
    }

    /// <summary>銃器を解放</summary>
    public void DoRelease()
    {
        _anim.SetBool(_PARAM_NAME_IS_FREEDOM, true);
    }

    /// <summary>銃器を持つ</summary>
    public void DoGet()
    {
        _anim.SetBool(_PARAM_NAME_IS_FREEDOM, false);
    }

    /// <summary>持つ銃器を変更</summary>
    public void DoSwitch()
    {
        _anim.SetBool(_PARAM_NAME_IS_RELOAD, false);
    }

    /// <summary>移動速度情報をこちらも取得</summary>
    /// <param name="spd">移動速度</param>
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
        //ポーズ時は止める
        if (GameManager.IsPose)
        {
            return;
        }

        if (_intervalTimer > 0f)
        {
            _intervalTimer -= Time.deltaTime;
        }
    }

    /// <summary>警戒モードを起動</summary>
    /// <param name="flag">true : 起動</param>
    public void CallCationMode(bool flag = true)
    {
        _anim.SetBool(_PARAM_NAME_IS_CATION, flag);
    }

    /// <summary>セミオート武器の射撃</summary>
    void DoShotSemiAuto(Vector3 target)
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

        _targetPoint = target;
        _anim.SetTrigger(_PARAM_NAME_DO_SHOT);
        _intervalTimer = _shotInterval;
    }

    /// <summary>セミオート武器のリロード</summary>
    void DoReloadSemiAuto()
    {
        //残弾が減っていたら実行
        if (!_isReloadInterval && _currentLoadAmmo < _maxLoadAmmo)
        {
            _currentLoadAmmo = 0;
            _anim.SetBool(_PARAM_NAME_IS_RELOAD, true);
            _isReloadInterval = true;
        }
    }

    /// <summary>ショットガンの射撃</summary>
    void DoShotShotGun(Vector3 target)
    {
        _anim.SetBool(_PARAM_NAME_IS_RELOAD, false);

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

        _targetPoint = target;
        _anim.SetTrigger(_PARAM_NAME_DO_SHOT);
        _intervalTimer = _shotInterval;
    }

    /// <summary>ショットガンのリロード</summary>
    void DoReloadShotGun()
    {
        //残弾が減っていたら実行
        if (!_isReloadInterval && _currentLoadAmmo < _maxLoadAmmo)
        {
            _anim.SetTrigger(_PARAM_NAME_IS_RELOAD);
            _isReloadInterval = true;
        }
    }

    /// <summary>フルオート武器の射撃</summary>
    void DoShotFullAuto(Vector3 target)
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

        _targetPoint = target;
        _anim.SetTrigger(_PARAM_NAME_DO_SHOT);
        _intervalTimer = _shotInterval;
    }

    /// <summary>フルオート武器のリロード</summary>
    void DoReloadFullAuto()
    {
        //残弾が減っていたら実行
        if (!_isReloadInterval && _currentLoadAmmo < _maxLoadAmmo)
        {
            _currentLoadAmmo = 0;
            _anim.SetBool(_PARAM_NAME_IS_RELOAD, true);
            _isReloadInterval = true;
        }
    }

    #region アニメーターイベント用
    /// <summary>発砲して銃弾を射出</summary>
    public void EmitBulletFromGun()
    {
        GameObject ins = _Instantiate();
        ins.transform.position = _shotSESource.transform.position;
        ins.transform.forward = Vector3.Normalize(_targetPoint - _shotSESource.transform.position);
        _currentLoadAmmo--;
    }

    /// <summary>ショットガンを発砲して銃弾を射出</summary>
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

    /// <summary>リロード完了</summary>
    public void ReloadComprete()
    {
        _currentLoadAmmo = _maxLoadAmmo;
        _anim.SetBool(_PARAM_NAME_IS_RELOAD, false);
        _isReloadInterval = false;
    }

    /// <summary>一発分だけリロード</summary>
    public void ReloadWithOne()
    {
        _currentLoadAmmo++;
        _anim.SetBool(_PARAM_NAME_IS_RELOAD, false);
        _isReloadInterval = false;
    }

    /// <summary>小さい発砲音</summary>
    public void EmitSEShotSmall()
    {
        _shotSESource.PlayOneShot(SEManager.Instance.GunShotSmall);
    }

    /// <summary>大きい発砲音</summary>
    public void EmitSEShotLarge()
    {
        _shotSESource.PlayOneShot(SEManager.Instance.GunShotLarge);
    }

    /// <summary>マガジンを外す音</summary>
    public void EmitSERemoveMagazine()
    {
        _reloadSESource.PlayOneShot(SEManager.Instance.MagazineRemove);
    }

    /// <summary>マガジンを着ける音</summary>
    public void EmitSEConnectMagazine()
    {
        _reloadSESource.PlayOneShot(SEManager.Instance.MagazineConnect);
    }

    /// <summary>プルバック音</summary>
    public void EmitSEPullBack()
    {
        _reloadSESource.PlayOneShot(SEManager.Instance.PullBack);
    }

    #endregion
}
