using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefeatedRagdoll : MonoBehaviour
{
    /// <summary>力をかける倍率</summary>
    const float _ADD_FORCE_RATIO = 10.0f;

    [SerializeField, Tooltip("体の中心近くのリジッドボディ")]
    Rigidbody _spineRb = null;

    [SerializeField, Tooltip("モデルを消すまでの待機時間")]
    short _destroyDelay = 7;

    /// <summary>モデルを消すまでの時間カウンター</summary>
    float _timer = 0;


    /// <summary>ラグドールの吹き飛ばし処理</summary>
    /// <param name="force">体の中心から力をかける方向</param>
    public void BlowAway(Vector3 force)
    {
        _spineRb.AddForce(force * _ADD_FORCE_RATIO, ForceMode.VelocityChange);
    }
        
    void Start()
    {
        _timer = _destroyDelay;
    }

    // Update is called once per frame
    void Update()
    {
        //ポーズ時は止める
        if (GameManager.IsPose)
        {
            return;
        }

        _timer -= Time.deltaTime;
        if(_timer < 0f)
        {
            Destroy(gameObject);
        }
    }
}
