using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    /// <summary>弾本体</summary>
    MeshRenderer _shotAmmo = null;

    /// <summary>弾道</summary>
    TrailRenderer _shotLine = null;

    [SerializeField, Tooltip("FixedUpdate毎の飛行距離")]
    float _fixedSpeed = 5f;

    [SerializeField, Tooltip("最大射程")]
    float _maxRange = 30f;

    [SerializeField, Tooltip("威力最大値")]
    short _maxPower = 20;

    [SerializeField, Tooltip("衝撃の大きさ")]
    float _impact = 1f;

    /// <summary>弾を消したあとに弾道を残す時間</summary>
    float _destroyDelayTime = 0f;

    /// <summary>発砲地点</summary>
    Vector3 _start = Vector3.zero;

    void Start()
    {
        _shotAmmo = GetComponentInChildren<MeshRenderer>();
        _shotLine = GetComponentInChildren<TrailRenderer>();
    }

    void OnEnable()
    {
        _start = transform.position;
        if(_shotAmmo) _shotAmmo.enabled = true;
    }

    void FixedUpdate()
    {
        //弾道を残しつつ消す処理
        if (!_shotAmmo.enabled)
        {
            //消す
            if (_destroyDelayTime < 0f)
            {
                _shotLine.Clear();
                gameObject.SetActive(false);
            }
            else
            {
                _destroyDelayTime -= Time.fixedDeltaTime;
            }
        }
        else
        {
            //_fixedSpeedだけ先（すなわちFixedUpdateの今のフレームで通過するセグメント）の状況を見る
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, _fixedSpeed, LayerManager.Instance.BulletHit))
            {
                //被ダメージコンポーネントがあればダメージ処理呼び出し
                Damager damager = hit.collider.GetComponent<Damager>();
                if (damager)
                {
                    damager.GetDamage(_maxPower, _impact);
                }

                //弾を消す予約
                _destroyDelayTime = _shotLine.time;
                _shotAmmo.enabled = false;
            }

            //正面方向へ前進
            transform.position += transform.forward * _fixedSpeed;

            //最大射程まで飛んだら消す予約
            if (Vector3.SqrMagnitude(transform.position - _start) > _maxRange * _maxRange)
            {
                _destroyDelayTime = _shotLine.time;
                _shotAmmo.enabled = false;
            }
        }
    }
}
