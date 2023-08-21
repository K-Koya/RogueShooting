using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Audio;

public class GUIConfig : MonoBehaviour
{
    const string _MASTER_VOLUME = "Master";

    [SerializeField, Tooltip("BGMの音量ミキサー")]
    AudioMixer _bGMMixer = null;

    [SerializeField, Tooltip("SEの音量ミキサー")]
    AudioMixer _sEMixer = null;

    [SerializeField, Tooltip("BGMの音量調整バー")]
    Slider _bGMVolume = null;

    [SerializeField, Tooltip("SEの音量調整バー")]
    Slider _sEVolume = null;

    [SerializeField, Tooltip("カメラ感度水平軸調整バー")]
    Slider _cameraSenseX = null;

    [SerializeField, Tooltip("カメラ感度鉛直軸調整バー")]
    Slider _cameraSenseY = null;

    void OnEnable()
    {
        float value = 0f;
        if (_bGMMixer && _bGMMixer.GetFloat(_MASTER_VOLUME, out value))
        {
            _bGMVolume.value = value;
        }
        if (_sEMixer && _sEMixer.GetFloat(_MASTER_VOLUME, out value))
        {
            _sEVolume.value = value;
        }
        if (_cameraSenseX) _cameraSenseX.value = CinemachineInputProviderTunable.SenseRateX;
        if (_cameraSenseY) _cameraSenseY.value = CinemachineInputProviderTunable.SenseRateY;

        Debug.Log($"BGM : {_bGMVolume.value}\nSE  : {_sEVolume.value}\nCam : {_cameraSenseX.value}, {_cameraSenseY.value}");
    }

    public void SetBGMVolume()
    {
        _bGMMixer?.SetFloat(_MASTER_VOLUME, _bGMVolume.value);
    }

    public void SetSEVolume()
    {
        _sEMixer?.SetFloat(_MASTER_VOLUME, _sEVolume.value);
    }

    public void SetCameraSenseX()
    {
        if (_cameraSenseX) CinemachineInputProviderTunable.SenseRateX = _cameraSenseX.value;
    }

    public void SetCameraSenseY()
    {
        if (_cameraSenseY) CinemachineInputProviderTunable.SenseRateY = _cameraSenseY.value;
    }
}
