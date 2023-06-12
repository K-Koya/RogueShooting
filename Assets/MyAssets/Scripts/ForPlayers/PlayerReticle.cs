using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReticle : MonoBehaviour
{
    [SerializeField, Tooltip("照準を当てられる最大距離")]
    float _maxDistance = 100f;

    /// <summary>MainCameraの位置等の情報</summary>
    Transform _mainCameraTransform = null;

    /// <summary>距離</summary>
    float _distance = 0f;

    /// <summary>照準が当たった座標</summary>
    Vector3 _point = Vector3.zero;

    /// <summary>照準先データ</summary>
    ReticleFocusedData _data = null;



    /// <summary>照準先データ</summary>
    public ReticleFocusedData Data => _data;

    /// <summary>照準が当たった座標</summary>
    public Vector3 Point => _point;


    // Start is called before the first frame update
    void Start()
    {
        _mainCameraTransform = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if(Physics.Raycast(_mainCameraTransform.position, _mainCameraTransform.forward, out hit, _maxDistance, LayerManager.Instance.OnTheReticle, QueryTriggerInteraction.Collide))
        {
            IReticleFocused irf = hit.transform.gameObject.GetComponent<IReticleFocused>();
            if(irf is not null) _data = irf.GetData();
            _distance = hit.distance;
            _point = hit.point;
        }
        else
        {
            _data = null;
            _distance = _maxDistance;
            _point = _mainCameraTransform.position + _mainCameraTransform.forward * _maxDistance;
        }
    }

#if UNITY_EDITOR
    /// <summary>照準先を示すレイキャスト</summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if(_mainCameraTransform) Gizmos.DrawLine(_mainCameraTransform.position, _point);
    }
#endif
}

/// <summary>照準を合わせたときにデータを取得できるインターフェース</summary>
public interface IReticleFocused
{
    /// <summary>照準先データ取得</summary>
    public ReticleFocusedData GetData();
}

/// <summary>照準を合わせたときに取得できるデータ</summary>
public class ReticleFocusedData
{
    /// <summary>照準を当てた時に実行するメソッド</summary>
    public Action<CharacterParameter, CharacterMove> _OnFocused = null;

    /// <summary>インタラクト時に実行するメソッド</summary>
    public Action<CharacterParameter, CharacterMove> _OnInteraction = null;

    /// <summary>名称</summary>
    public string _name = null;

    /// <summary>最大体力</summary>
    public short _maxLife = 100;

    /// <summary>現在体力</summary>
    public short _currentLife = 100;


}
