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
            GameObject baseObj = new GameObject();

            foreach (LocationObject obj in _locationObjects)
            {
                obj._object.transform.parent = baseObj.transform;
                obj._object.transform.localPosition = obj._relativePosition;
            }

            baseObj.transform.position = _map.StartFloorBasePosition;
            baseObj.transform.forward = _map.StartFloorRoadCross;

            foreach (LocationObject obj in _locationObjects)
            {
                obj._object.transform.parent = null;
            }

            Destroy(baseObj);
        }
    }
}

public interface IStartLocation
{
    /// <summary>基準座標取得</summary>
    public Vector3 StartFloorBasePosition { get; }

    /// <summary>道に垂直な方向を取得</summary>
    public Vector3 StartFloorRoadCross { get; }
}
