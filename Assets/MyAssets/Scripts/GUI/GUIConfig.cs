using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Audio;

public class GUIConfig : MonoBehaviour
{
    const string _MASTER_VOLUME = "Master";

    [SerializeField, Tooltip("BGM�̉��ʃ~�L�T�[")]
    AudioMixer _bGMMixer = null;

    [SerializeField, Tooltip("SE�̉��ʃ~�L�T�[")]
    AudioMixer _sEMixer = null;

    [SerializeField, Tooltip("BGM�̉��ʒ����o�[")]
    Slider _bGMVolume = null;

    [SerializeField, Tooltip("SE�̉��ʒ����o�[")]
    Slider _sEVolume = null;

    [SerializeField, Tooltip("�J�������x�����������o�[")]
    Slider _cameraSenseX = null;

    [SerializeField, Tooltip("�J�������x�����������o�[")]
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
