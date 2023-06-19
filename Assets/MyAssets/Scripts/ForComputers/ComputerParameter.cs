using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ComputerParameter : CharacterParameter, IReticleFocused
{
    /// <summary>倒すべき敵の人数</summary>
    static byte _DefeatedEnemyQuota = 0;

    /// <summary>倒した敵の人数</summary>
    static byte _DefeatedEnemyCount = 0;

    /// <summary>照準時のブレの大きさの基本値</summary>
    static float _BaseAccuracyAim = 1f;

    [SerializeField, Tooltip("やられた動きをするラグドールのプレハブ")]
    GameObject _defeatedRagdollPref = null;

    [SerializeField, Tooltip("視界の距離")]
    float _range = 20f;

    [SerializeField, Tooltip("見失うまでの時間")]
    float _missingTime = 5f;

    /// <summary>見失いタイマー</summary>
    float _timer = 0f;

    /// <summary>襲うターゲット</summary>
    CharacterParameter _target = null;


    #region プロパティ
    /// <summary>倒すべき敵の人数</summary>
    public static byte DefeatedEnemyQuota => _DefeatedEnemyQuota;
    /// <summary>倒した敵の人数</summary>
    public static byte DefeatedEnemyCount => _DefeatedEnemyCount;
    /// <summary>照準時のブレの大きさの基本値</summary>
    public static float BaseAccuracyAim => _BaseAccuracyAim;
    /// <summary>襲うターゲット</summary>
    public CharacterParameter Target => _target;
    /// <summary>true : ターゲットへの射線が通っている</summary>
    public bool IsThroughLineOfSight => _missingTime <= _timer;

    #endregion


    /// <summary>このステージにおける敵情報の初期化</summary>
    /// <param name="quota">倒す敵の人数ノルマ</param>
    /// <param name="baseAim">敵の照準制度のベース値</param>
    public static void SetEnemyData(byte quota, float baseAim)
    {
        _DefeatedEnemyQuota = quota;
        _DefeatedEnemyCount = 0;
        _BaseAccuracyAim = baseAim;
    }


    protected override void Start()
    {
        base.Start();
        _Enemies.Add(this);
    }

    protected override void Update()
    {
        //ポーズ時は止める
        if (GameManager.IsPose)
        {
            return;
        }

        base.Update();
        SeekInSight();
    }

    private void OnDestroy()
    {
        _Enemies.Remove(this);
    }

    /// <summary>視界内のターゲット探索</summary>
    public void SeekInSight()
    {
        //まず、ターゲットを認識しているかで分岐
        Vector3 toTarget;
        RaycastHit hit;
        if (_target)
        {
            toTarget = Vector3.Normalize(_target.EyePoint.position - _eyePoint.position);

            //ターゲットから距離が離れている
            if (Vector3.SqrMagnitude(toTarget) > _range * _range)
            {
                //見失いカウントダウン
                _timer -= Time.deltaTime;
                if (_timer < 0f)
                {
                    //時間切れでターゲット解除
                    _target = null;
                }
            }
            //レイを飛ばし、間に障害物がないか確認
            else if (Physics.Raycast(_eyePoint.position, toTarget, out hit, _range, LayerManager.Instance.EnemyFocusable))
            {
                if (hit.collider.gameObject != _target.gameObject)
                {
                    //障害物なら見失いカウントダウン
                    _timer -= Time.deltaTime;
                    if (_timer < 0f)
                    {
                        //時間切れでターゲット解除
                        _target = null;
                    }
                }
                else
                {
                    _timer = _missingTime;
                }
            }
        }
        else
        {
            _timer = _missingTime;
            foreach (CharacterParameter parameter in _Allies)
            {
                toTarget = Vector3.Normalize(parameter.EyePoint.position - _eyePoint.position);

                //内積と距離比較でコーン内にいるか探索
                if (Vector3.Dot(toTarget, _lookDirection) > 0.5f
                    && Vector3.SqrMagnitude(toTarget) < _range * _range)
                {
                    //レイを飛ばし、間に障害物がないか確認
                    if (Physics.Raycast(_eyePoint.position, toTarget, out hit, _range, LayerManager.Instance.EnemyFocusable))
                    {
                        if (hit.collider.gameObject == parameter.gameObject)
                        {
                            //ターゲット登録
                            _target = parameter;
                            break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>照準を合わせた際にデータを開示</summary>
    public ReticleFocusedData GetData()
    {
        ReticleFocusedData data = new ReticleFocusedData();
        data._maxLife = _maxLife;
        data._currentLife = _currentLife;
        data._name = _character.ToString();

        return data;
    }

    public override void GaveDamage(short damage, float impact, Vector3 impactDirection)
    {
        base.GaveDamage(damage, impact, impactDirection);

        if (_state.Kind is MotionState.StateKind.Defeat)
        {
            GameObject ragdoll = Instantiate(_defeatedRagdollPref);
            ragdoll.transform.position = transform.position;
            ragdoll.transform.rotation = transform.rotation;
            ragdoll.GetComponent<DefeatedRagdoll>().BlowAway(impactDirection * impact);

            _DefeatedEnemyCount++;

            Destroy(gameObject, 0.1f);
        }
    }


}
