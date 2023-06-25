using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartLocater : MonoBehaviour
{
    /// <summary>�}�b�v���iIStartLocation���p���j</summary>
    IGetPlantMapData _map = null;

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
        _map = GetComponent<IGetPlantMapData>();
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
