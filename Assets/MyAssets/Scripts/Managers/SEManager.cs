using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : Singleton<SEManager>
{
    [SerializeField, Tooltip("効果音 : 石畳・歩行時足音")]
    AudioClip[] _footStampWalkOnRocks = null;

    /// <summary>石畳・歩行時足音の数</summary>
    byte _numberOfFootStampWalkOnRock = 1;

    [SerializeField, Tooltip("効果音 : 石畳・走行時足音")]
    AudioClip[] _footStampRunOnRocks = null;

    /// <summary>石畳・走行時足音の数</summary>
    byte _numberOfFootStampRunOnRock = 1;

    [SerializeField, Tooltip("効果音 : 銃声小")]
    AudioClip _gunshotSmall = null;

    [SerializeField, Tooltip("効果音 : 銃声大")]
    AudioClip _gunshotLarge = null;


    /// <summary>効果音 : 石畳・歩行時足音</summary>
    AudioClip FootStampWalkOnRock { get => _footStampWalkOnRocks[(int)Random.Range(0f, _numberOfFootStampWalkOnRock)]; }
    /// <summary>効果音 : 石畳・走行時足音</summary>
    AudioClip FootStampRunOnRock { get => _footStampRunOnRocks[(int)Random.Range(0f, _numberOfFootStampRunOnRock)]; }
    /// <summary>効果音 : 銃声小</summary>
    AudioClip GunShotSmall { get => _gunshotSmall; }
    /// <summary>効果音 : 銃声大</summary>
    AudioClip GunshotLarge { get => _gunshotLarge; }


    // Start is called before the first frame update
    void Start()
    {
        _numberOfFootStampWalkOnRock = (byte)_footStampWalkOnRocks.Length;
        _numberOfFootStampRunOnRock = (byte)_footStampRunOnRocks.Length;
    }
}
