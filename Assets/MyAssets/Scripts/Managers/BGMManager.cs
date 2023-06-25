using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : Singleton<BGMManager>
{
    /// <summary>���C����BGM</summary>
    AudioSource _mainSpeaker = null;

    /// <summary>�T�u��BGM</summary>
    AudioSource _subSpeaker = null;

    [SerializeField, Tooltip("��n���̗����������Ƃ���ŗ���BGM")]
    AudioClip _baseSpace = null;

    [SerializeField, Tooltip("�Q�[����������������Ԃ̂Ƃ��ɗ���BGM")]
    AudioClip _fieldOnCalm = null;

    [SerializeField, Tooltip("�Q�[�����ٔ�������Ԃ̂Ƃ��ɗ���BGM")]
    AudioClip _fieldOnCation = null;

    [SerializeField, Tooltip("�Q�[�����N���A�����Ƃ��ɗ���BGM")]
    AudioClip _resultOnClear = null;

    [SerializeField, Tooltip("BGM��؂�ւ��鑬�x")]
    float _bgmSwitchSpeed = 0.2f;

    /// <summary>���݂�BGM��؂�ւ��鑬�x</summary>
    float _currentBgmSwitchSpeed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();
        _mainSpeaker = audios[0];
        _subSpeaker = audios[1];
        _currentBgmSwitchSpeed = 0f;
    }

    void Update()
    {
        //BGM�؂�ւ�����
        if (_currentBgmSwitchSpeed != 0f)
        {
            _mainSpeaker.volume -= _currentBgmSwitchSpeed * Time.deltaTime;
            _subSpeaker.volume += _currentBgmSwitchSpeed * Time.deltaTime;

            if (_mainSpeaker.volume >= 1f)
            {
                _mainSpeaker.volume = 1f;
                _subSpeaker.volume = 0f;
                _currentBgmSwitchSpeed = 0f;
                _subSpeaker.Stop();

            }
            else if (_mainSpeaker.volume <= 0f)
            {
                _mainSpeaker.volume = 0f;
                _subSpeaker.volume = 1f;
                _currentBgmSwitchSpeed = 0f;
                _mainSpeaker.Stop();
            }
        }
    }

    /// <summary>��n���̗����������Ƃ���ŗ���BGM</summary>
    public void BGMCallBaseSpace()
    {
        BGMInitialize();
        _mainSpeaker.Stop();
        _mainSpeaker.clip = _baseSpace;
        _mainSpeaker.Play();
    }

    /// <summary>�Q�[�����ɗ���BGM</summary>
    public void BGMCallField()
    {
        _mainSpeaker.Stop();
        _mainSpeaker.clip = _fieldOnCalm;
        _mainSpeaker.Play();
    }

    /// <summary>�Q�[�����ɗ����ٔ�����BGM</summary>
    /// <param name="isCation">true : �ٔ�����BGM�ɐ؂�ւ���</param>
    public void SwitchCallCation(bool isCation)
    {
        if (isCation)
        {
            if(_subSpeaker.clip != _fieldOnCation)
            {
                _subSpeaker.Stop();
                _subSpeaker.clip = _fieldOnCation;
            }

            _subSpeaker.Play();
            _currentBgmSwitchSpeed = _bgmSwitchSpeed;
        }
        else
        {
            _mainSpeaker.Play();
            _currentBgmSwitchSpeed = -_bgmSwitchSpeed;
        }
    }

    /// <summary>BGM����������</summary>
    public void BGMOff()
    {
        _mainSpeaker.Stop();
        _subSpeaker.Stop();
    }

    /// <summary>�Q�[�����N���A�����Ƃ��ɗ���BGM</summary>
    public void BGMCallResultOnClear()
    {
        BGMInitialize();
        _mainSpeaker.Stop();
        _mainSpeaker.clip = _resultOnClear;
        _mainSpeaker.Play();
    }

    /// <summary>BGM��������</summary>
    void BGMInitialize()
    {
        _currentBgmSwitchSpeed = 0f;
        _subSpeaker.Stop();
        _mainSpeaker.volume = 1f;
    }
}
