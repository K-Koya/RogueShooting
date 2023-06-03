using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : Singleton<SEManager>
{
    [SerializeField, Tooltip("���ʉ� : �Ώ�E���s������")]
    AudioClip[] _footStampWalkOnRocks = null;

    /// <summary>�Ώ�E���s�������̐�</summary>
    byte _numberOfFootStampWalkOnRock = 1;

    [SerializeField, Tooltip("���ʉ� : �Ώ�E���s������")]
    AudioClip[] _footStampRunOnRocks = null;

    /// <summary>�Ώ�E���s�������̐�</summary>
    byte _numberOfFootStampRunOnRock = 1;

    [SerializeField, Tooltip("���ʉ� : �e����")]
    AudioClip _gunshotSmall = null;

    [SerializeField, Tooltip("���ʉ� : �e����")]
    AudioClip _gunshotLarge = null;


    /// <summary>���ʉ� : �Ώ�E���s������</summary>
    AudioClip FootStampWalkOnRock { get => _footStampWalkOnRocks[(int)Random.Range(0f, _numberOfFootStampWalkOnRock)]; }
    /// <summary>���ʉ� : �Ώ�E���s������</summary>
    AudioClip FootStampRunOnRock { get => _footStampRunOnRocks[(int)Random.Range(0f, _numberOfFootStampRunOnRock)]; }
    /// <summary>���ʉ� : �e����</summary>
    AudioClip GunShotSmall { get => _gunshotSmall; }
    /// <summary>���ʉ� : �e����</summary>
    AudioClip GunshotLarge { get => _gunshotLarge; }


    // Start is called before the first frame update
    void Start()
    {
        _numberOfFootStampWalkOnRock = (byte)_footStampWalkOnRocks.Length;
        _numberOfFootStampRunOnRock = (byte)_footStampRunOnRocks.Length;
    }
}
