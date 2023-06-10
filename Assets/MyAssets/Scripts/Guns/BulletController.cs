using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    /// <summary>�e�{��</summary>
    MeshRenderer _shotAmmo = null;

    /// <summary>�e��</summary>
    TrailRenderer _shotLine = null;

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
            //_fixedSpeed������i���Ȃ킿FixedUpdate�̍��̃t���[���Œʉ߂���Z�O�����g�j�̏󋵂�����
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, _fixedSpeed, LayerManager.Instance.BulletHit))
            {
                //��_���[�W�R���|�[�l���g������΃_���[�W�����Ăяo��
                Damager damager = hit.collider.GetComponent<Damager>();
                if (damager)
                {
                    damager.GetDamage(_maxPower, _impact);
                }

                //�e�������\��
                _destroyDelayTime = _shotLine.time;
                _shotAmmo.enabled = false;
            }

            //���ʕ����֑O�i
            transform.position += transform.forward * _fixedSpeed;

            //�ő�˒��܂Ŕ�񂾂�����\��
            if (Vector3.SqrMagnitude(transform.position - _start) > _maxRange * _maxRange)
            {
                _destroyDelayTime = _shotLine.time;
                _shotAmmo.enabled = false;
            }
        }
    }
}
