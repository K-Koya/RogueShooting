using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    /// <summary>’e–{‘Ì</summary>
    MeshRenderer _shotAmmo = null;

    /// <summary>’e“¹</summary>
    TrailRenderer _shotLine = null;

    [SerializeField, Tooltip("FixedUpdate–ˆ‚Ì”òs‹——£")]
    float _fixedSpeed = 5f;

    [SerializeField, Tooltip("Å‘åË’ö")]
    float _maxRange = 30f;

    [SerializeField, Tooltip("ˆĞ—ÍÅ‘å’l")]
    short _maxPower = 20;

    [SerializeField, Tooltip("ÕŒ‚‚Ì‘å‚«‚³")]
    float _impact = 1f;

    /// <summary>’e‚ğÁ‚µ‚½‚ ‚Æ‚É’e“¹‚ğc‚·ŠÔ</summary>
    float _destroyDelayTime = 0f;

    /// <summary>”­–C’n“_</summary>
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
        //’e“¹‚ğc‚µ‚Â‚ÂÁ‚·ˆ—
        if (!_shotAmmo.enabled)
        {
            //Á‚·
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
            //³–Ê•ûŒü‚Ö‘Oi
            transform.position += transform.forward * _fixedSpeed;

            //Å‘åË’ö‚Ü‚Å”ò‚ñ‚¾‚çÁ‚·—\–ñ
            if (Vector3.SqrMagnitude(transform.position - _start) > _maxRange * _maxRange)
            {
                _destroyDelayTime = _shotLine.time;
                _shotAmmo.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Damager dmg = null;
        if(other.TryGetComponent(out dmg))
        {
            dmg.GetDamage(_maxPower, _impact, transform.forward);
        }

        _destroyDelayTime = _shotLine.time;
        _shotAmmo.enabled = false;
    }
}
