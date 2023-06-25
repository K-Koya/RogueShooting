using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : Singleton<BGMManager>
{
    /// <summary>メインのBGM</summary>
    AudioSource _mainSpeaker = null;

    /// <summary>サブのBGM</summary>
    AudioSource _subSpeaker = null;

    [SerializeField, Tooltip("基地内の落ち着いたところで流すBGM")]
    AudioClip _baseSpace = null;

    [SerializeField, Tooltip("ゲーム中落ち着いた状態のときに流すBGM")]
    AudioClip _fieldOnCalm = null;

    [SerializeField, Tooltip("ゲーム中緊迫した状態のときに流すBGM")]
    AudioClip _fieldOnCation = null;

    [SerializeField, Tooltip("ゲームをクリアしたときに流すBGM")]
    AudioClip _resultOnClear = null;

    [SerializeField, Tooltip("BGMを切り替える速度")]
    float _bgmSwitchSpeed = 0.2f;

    /// <summary>現在のBGMを切り替える速度</summary>
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
        //BGM切り替え処理
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

    /// <summary>基地内の落ち着いたところで流すBGM</summary>
    public void BGMCallBaseSpace()
    {
        BGMInitialize();
        _mainSpeaker.Stop();
        _mainSpeaker.clip = _baseSpace;
        _mainSpeaker.Play();
    }

    /// <summary>ゲーム中に流すBGM</summary>
    public void BGMCallField()
    {
        _mainSpeaker.Stop();
        _mainSpeaker.clip = _fieldOnCalm;
        _mainSpeaker.Play();
    }

    /// <summary>ゲーム中に流す緊迫したBGM</summary>
    /// <param name="isCation">true : 緊迫したBGMに切り替える</param>
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

    /// <summary>BGMを消す処理</summary>
    public void BGMOff()
    {
        _mainSpeaker.Stop();
        _subSpeaker.Stop();
    }

    /// <summary>ゲームをクリアしたときに流すBGM</summary>
    public void BGMCallResultOnClear()
    {
        BGMInitialize();
        _mainSpeaker.Stop();
        _mainSpeaker.clip = _resultOnClear;
        _mainSpeaker.Play();
    }

    /// <summary>BGM初期処理</summary>
    void BGMInitialize()
    {
        _currentBgmSwitchSpeed = 0f;
        _subSpeaker.Stop();
        _mainSpeaker.volume = 1f;
    }
}
