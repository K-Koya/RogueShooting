using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GUICameraConfigScroll : MonoBehaviour
{
    [SerializeField, Tooltip("�����ŏ��l")]
    float _minSence = 0.01f;

    [SerializeField, Tooltip("�����ő�l")]
    float _maxSence = 0.1f;

    [SerializeField, Tooltip("�����}�E�X���x����")]
    Scrollbar _cameraHorizontal = null;

    [SerializeField, Tooltip("�����}�E�X���x����")]
    Scrollbar _cameraVertical = null;

    [SerializeField, Tooltip("InputAction�ɂ�����A�J�����ړ����͖�")]
    string _stickNameCameraMove = "CameraMove";

    /// <summary> �J�����ړ�����̓��͐��� </summary>
    InputAction _cameraMoveAction = default;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        InputActionMap actionMap = input.currentActionMap;
        _cameraMoveAction = actionMap[_stickNameCameraMove];
    }
}
