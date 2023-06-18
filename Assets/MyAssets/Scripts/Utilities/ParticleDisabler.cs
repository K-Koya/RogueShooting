using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDisabler : MonoBehaviour
{
    [SerializeField, Tooltip("パーティクルエフェクトの大本の親オブジェクト")]
    GameObject _particleParent = null;

    private void OnParticleSystemStopped()
    {
        _particleParent?.gameObject.SetActive(false);
    }
}