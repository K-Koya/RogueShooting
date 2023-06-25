using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GUICameraConfigScroll : MonoBehaviour
{
    [SerializeField, Tooltip("調整最小値")]
    float _minSence = 0.01f;

    [SerializeField, Tooltip("調整最大値")]
    float _maxSence = 0.1f;

    [SerializeField, Tooltip("水平マウス感度調整")]
    Scrollbar _cameraHorizontal = null;

    [SerializeField, Tooltip("垂直マウス感度調整")]
    Scrollbar _cameraVertical = null;

    [SerializeField, Tooltip("InputActionにおける、カメラ移動入力名")]
    string _stickNameCameraMove = "CameraMove";

    /// <summary> カメラ移動操作の入力制御 </summary>
    InputAction _cameraMoveAction = default;

    // Start is called before the first frame update
    void Start()
    {
        PlayerInput input = GetComponent<PlayerInput>();
        InputActionMap actionMap = input.currentActionMap;
        _cameraMoveAction = actionMap[_stickNameCameraMove];
    }
}
