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

    [SerializeField, Tooltip("���ʉ� : �}�K�W�����O�����̉�")]
    AudioClip _magazineRemove = null;

    [SerializeField, Tooltip("���ʉ� : �}�K�W���𒅂��鎞�̉�")]
    AudioClip _magazineConnect = null;

    [SerializeField, Tooltip("���ʉ� : �v���o�b�N��")]
    AudioClip _pullBack = null;

    [SerializeField, Tooltip("���ʉ� : �e�e�����ɓ����������̉�")]
    AudioClip[] _bulletRicochets = null;

    /// <summary>�e�e�����ɓ����������̉��̐�</summary>
    byte _numberOfBulletRicochets = 1;



    /// <summary>���ʉ� : �Ώ�E���s������</summary>
    public AudioClip FootStampWalkOnRock { get => _footStampWalkOnRocks[Random.Range(0, _numberOfFootStampWalkOnRock)]; }
    /// <summary>���ʉ� : �Ώ�E���s������</summary>
    public AudioClip FootStampRunOnRock { get => _footStampRunOnRocks[Random.Range(0, _numberOfFootStampRunOnRock)]; }
    /// <summary>���ʉ� : �e����</summary>
    public AudioClip GunShotSmall { get => _gunshotSmall; }
    /// <summary>���ʉ� : �e����</summary>
    public AudioClip GunShotLarge { get => _gunshotLarge; }
    /// <summary>���ʉ� : �}�K�W�����O�����̉�</summary>
    public AudioClip MagazineRemove { get => _magazineRemove; }
    /// <summary>���ʉ� : �}�K�W���𒅂��鎞�̉�</summary>
    public AudioClip MagazineConnect { get => _magazineConnect; }
    /// <summary>���ʉ� : �v���o�b�N��</summary>
    public AudioClip PullBack { get => _pullBack; }
    /// <summary>���ʉ� : �e�e�����ɓ����������̉�</summary>
    public AudioClip BulletRicochets { get => _bulletRicochets[Random.Range(0, _numberOfBulletRicochets)]; }



    // Start is called before the first frame update
    void Start()
    {
        _numberOfFootStampWalkOnRock = (byte)_footStampWalkOnRocks.Length;
        _numberOfFootStampRunOnRock = (byte)_footStampRunOnRocks.Length;
        _numberOfBulletRicochets = (byte)_bulletRicochets.Length;
    }
}
