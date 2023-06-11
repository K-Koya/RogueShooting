using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLocater : MonoBehaviour
{
    /// <summary>マップ情報（IStartLocationを継承）</summary>
    IStartLocation _map = null;

    [System.Serializable]
    struct LocationObject
    {
        [SerializeField, Tooltip("配置する対象オブジェクト")]
        public GameObject _object;

        [SerializeField, Tooltip("配置する相対座標")]
        public Vector3 _relativePosition;
    }
    [SerializeField, Tooltip("配置するオブジェクト情報")]
    LocationObject[] _locationObjects = null;


    void Awake()
    {
        _map = GetComponent<IStartLocation>();

        if (_map is not null)
        {
            Vector3 basePos = _map.StartFloorBasePosition;
            foreach(LocationObject obj in _locationObjects)
            {
                obj._object.transform.position = basePos + obj._relativePosition;
            }
        }
    }
}

public interface IStartLocation
{
    /// <summary>基準座標取得</summary>
    public Vector3 StartFloorBasePosition { get; }
}
