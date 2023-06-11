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
    /// <summary>����W�擾</summary>
    public Vector3 StartFloorBasePosition { get; }
}
