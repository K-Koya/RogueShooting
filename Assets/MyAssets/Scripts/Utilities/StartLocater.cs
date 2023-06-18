using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLocater : MonoBehaviour
{
    /// <summary>�}�b�v���iIStartLocation���p���j</summary>
    IStartLocation _map = null;

    [System.Serializable]
    struct LocationObject
    {
        [SerializeField, Tooltip("�z�u����ΏۃI�u�W�F�N�g")]
        public GameObject _object;

        [SerializeField, Tooltip("�z�u���鑊�΍��W")]
        public Vector3 _relativePosition;
    }
    [SerializeField, Tooltip("�z�u����I�u�W�F�N�g���")]
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
    /// <summary>����W�擾</summary>
    public Vector3 StartFloorBasePosition { get; }

    /// <summary>���ɐ����ȕ������擾</summary>
    public Vector3 StartFloorRoadCross { get; }
}
