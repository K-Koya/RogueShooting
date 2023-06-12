using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerReticle : MonoBehaviour
{
    [SerializeField, Tooltip("�Ə��𓖂Ă���ő勗��")]
    float _maxDistance = 100f;

    /// <summary>MainCamera�̈ʒu���̏��</summary>
    Transform _mainCameraTransform = null;

    /// <summary>����</summary>
    float _distance = 0f;

    /// <summary>�Ə��������������W</summary>
    Vector3 _point = Vector3.zero;

    /// <summary>�Ə���f�[�^</summary>
    ReticleFocusedData _data = null;



    /// <summary>�Ə���f�[�^</summary>
    public ReticleFocusedData Data => _data;

    /// <summary>�Ə��������������W</summary>
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
    /// <summary>�Ə�����������C�L���X�g</summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        if(_mainCameraTransform) Gizmos.DrawLine(_mainCameraTransform.position, _point);
    }
#endif
}

/// <summary>�Ə������킹���Ƃ��Ƀf�[�^���擾�ł���C���^�[�t�F�[�X</summary>
public interface IReticleFocused
{
    /// <summary>�Ə���f�[�^�擾</summary>
    public ReticleFocusedData GetData();
}

/// <summary>�Ə������킹���Ƃ��Ɏ擾�ł���f�[�^</summary>
public class ReticleFocusedData
{
    /// <summary>�Ə��𓖂Ă����Ɏ��s���郁�\�b�h</summary>
    public Action<CharacterParameter, CharacterMove> _OnFocused = null;

    /// <summary>�C���^���N�g���Ɏ��s���郁�\�b�h</summary>
    public Action<CharacterParameter, CharacterMove> _OnInteraction = null;

    /// <summary>����</summary>
    public string _name = null;

    /// <summary>�ő�̗�</summary>
    public short _maxLife = 100;

    /// <summary>���ݑ̗�</summary>
    public short _currentLife = 100;


}
