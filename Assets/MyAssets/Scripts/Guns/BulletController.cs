using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    /// <summary>�e�{��</summary>
    MeshRenderer _shotAmmo = null;

    /// <summary>�e��</summary>
    TrailRenderer _shotLine = null;

    /// <summary>�Ώۂ̃R���C�_�[</summary>
    Collider _collider = null;

    [SerializeField, Tooltip("FixedUpdate���̔�s����")]
    float _fixedSpeed = 5f;

    [SerializeField, Tooltip("�ő�˒�")]
    float _maxRange = 30f;

    [SerializeField, Tooltip("�З͍ő�l")]
    short _maxPower = 20;

    [SerializeField, Tooltip("�Ռ��̑傫��")]
    float _impact = 1f;

    /// <summary>�e�����������Ƃɒe�����c������</summary>
    float _destroyDelayTime = 0f;

    /// <summary>���C�n�_</summary>
    Vector3? _start = null;


    void Start()
    {
        _shotAmmo = GetComponentInChildren<MeshRenderer>();
        _shotLine = GetComponentInChildren<TrailRenderer>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    void OnEnable()
    {
        _destroyDelayTime = 1;
        _start = null;
        if (_collider) _collider.enabled = true;
        if (_shotAmmo) _shotAmmo.enabled = true;
    }

    void FixedUpdate()
    {
        //�e�����c����������
        if (!_shotAmmo.enabled)
        {
            //����
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
            if(_start is null)
            {
                _start = transform.position;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, transform.forward, out hit, _fixedSpeed, LayerManager.Instance.BulletHit))
                {
                    Damager dmg = null;
                    GameObject ins = null;
                    if (hit.collider.TryGetComponent(out dmg))
                    {
                        dmg.GetDamage(_maxPower, _impact, transform.forward);
                        ins = EffectManager.Instance.BulletHitCharacterEffects.Instansiate();
                    }
                    else
                    {
                        ins = EffectManager.Instance.BulletHitGroundEffects.Instansiate();
                    }

                    if (ins)
                    {
                        ins.transform.position = transform.position;
                        ins.transform.forward = -transform.forward;
                    }

                    _destroyDelayTime = _shotLine.time;
                    _shotAmmo.enabled = false;
                    _collider.enabled = false;
                }

                //���ʕ����֑O�i
                transform.position += transform.forward * _fixedSpeed;

                //�ő�˒��܂Ŕ�񂾂�����\��
                if (Vector3.SqrMagnitude(transform.position - _start.Value) > _maxRange * _maxRange)
                {
                    _destroyDelayTime = _shotLine.time;
                    _shotAmmo.enabled = false;
                    _collider.enabled = false;
                }
            }
        }
    }
}
