using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputManagerで使われているボタン名の文字列を管理
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputUtility : Singleton<InputUtility>
{
    #region InputActionのActionsキー
    [Header("以下、InputActionのActionsに登録した名前を登録")]
    [SerializeField, Tooltip("InputActionにおける、スタートボタン名")]
    string _buttonNameStart = "Start";

    [SerializeField, Tooltip("InputActionにおける、ポーズメニューボタン名")]
    string _buttonNamePause = "Pause";

    [SerializeField, Tooltip("InputActionにおける、リストメニューボタン名")]
    string _buttonNameOption = "Option";

    [SerializeField, Tooltip("InputActionにおける、移動方向の入力名")]
    string _stickNameMoveDirection = "MoveDirection";

    [SerializeField, Tooltip("InputActionにおける、カメラ移動入力名")]
    string _stickNameCameraMove = "CameraMove";

    [SerializeField, Tooltip("InputActionにおける、ジャンプ入力名")]
    string _buttonNameJump = "Jump";

    [SerializeField, Tooltip("InputActionにおける、インタラクト入力名")]
    string _buttonNameInteract = "Interact";

    [SerializeField, Tooltip("InputActionにおける、射撃入力名")]
    string _buttonNameFire = "Fire";

    [SerializeField, Tooltip("InputActionにおける、リロード入力名")]
    string _buttonNameReload = "Reload";

    [SerializeField, Tooltip("InputActionにおける、回避入力名")]
    string _buttonNameRun = "Run";

    [SerializeField, Tooltip("InputActionにおける、決定ボタン入力名")]
    string _buttonNameDecide = "Decide";

    #endregion
    /*
    #region コントローラー振動用メンバ
    /// <summary> コントローラー </summary>
    //static Gamepad _Gamepad = default;

    [Header("コントローラー振動用に用いるパラメーター")]
    [SerializeField, Range(0, 1), Tooltip("コントローラーの右側の振動の強さ")]
    float _rightShakePower = 0.5f;

    /// <summary>DOTween保管用 : コントローラーの右側の振動の強さ</summary>
    float _TweenRightShakePower = 0f;

    [SerializeField, Range(0, 1), Tooltip("コントローラーの左側の振動の強さ")]
    float _LeftShakePower = 0.5f;

    /// <summary>DOTween保管用 : コントローラーの左側の振動の強さ</summary>
    float _TweenLeftShakePower = 0f;

    [SerializeField, Tooltip("コントローラーの振動させる時間")]
    float _shakeInterval = 0.75f;

    [SerializeField, Tooltip("コントローラーの振動していない時間")]
    float _UnShakeInterval = 0.75f;

    [SerializeField, Tooltip("DOTweenを使って細かい振動をさせる場合の使うEasingタイプ")]
    Ease _TweenShakeModeRight = Ease.Linear;

    [SerializeField, Tooltip("DOTweenを使って大きな振動をさせる場合の使うEasingタイプ")]
    Ease _TweenShakeModeLeft = Ease.Linear;
    #endregion
    */

    #region InputAction
    /// <summary> スタートボタンの入力状況 </summary>
    static InputAction _startAction = default;

    /// <summary> ポーズメニュー起動の入力状況 </summary>
    static InputAction _pauseAction = default;

    /// <summary> リストメニュー起動の入力状況 </summary>
    static InputAction _optionAction = default;

    /// <summary> 移動操作の入力状況 </summary>
    static InputAction _moveDirectionAction = default;

    /// <summary> カメラ移動操作の入力状況 </summary>
    static InputAction _cameraMoveAction = default;

    /// <summary> ジャンプの入力状況 </summary>
    static InputAction _jumpAction = default;

    /// <summary> インタラクトボタンの入力状況 </summary>
    static InputAction _interactAction = default;

    /// <summary> 射撃ボタンの入力状況 </summary>
    static InputAction _fireAction = default;

    /// <summary> リロードボタンの入力状況 </summary>
    static InputAction _reloadAction = default;

    /// <summary> 回避ボタンの入力状況 </summary>
    static InputAction _runAction = default;

    /// <summary> 決定ボタンの入力状況 </summary>
    static InputAction _decideAction = default;
    #endregion

    #region プロパティ
    /// <summary> スタートボタン押下直後 </summary>
    static public bool GetDownStart { get => _startAction.triggered; }
    /// <summary> ポーズボタン押下直後 </summary>
    static public bool GetDownPause { get => _pauseAction.triggered; }
    /// <summary> リストメニューボタン押下直後 </summary>
    static public bool GetDownOption { get => _optionAction.triggered; }
    /// <summary> 移動操作直後 </summary>
    static public bool GetMoveDown { get => _moveDirectionAction.triggered; }
    /// <summary> 移動操作の方向取得 </summary>
    static public Vector2 GetMoveDirection { get => _moveDirectionAction.ReadValue<Vector2>(); }
    /// <summary> 移動操作終了 </summary>
    static public bool GetMoveUp { get => _moveDirectionAction.WasReleasedThisFrame(); }
    /// <summary> カメラ操作直後 </summary>
    static public bool GetCameraMoveDown { get => _cameraMoveAction.triggered; }
    /// <summary> カメラ操作の方向取得 </summary>
    static public Vector2 GetCameraMoveDirection { get => _cameraMoveAction.ReadValue<Vector2>(); }
    /// <summary> カメラ操作終了 </summary>
    static public bool GetCameraMoveUp { get => _cameraMoveAction.WasReleasedThisFrame(); }
    /// <summary> ジャンプボタン押下直後 </summary>
    static public bool GetDownJump { get => _jumpAction.triggered; }
    /// <summary> ジャンプボタン押下中 </summary>
    static public bool GetJump { get => _jumpAction.IsPressed(); }
    /// <summary> インタラクトボタン押下直後 </summary>
    static public bool GetDownInteract { get => _interactAction.triggered; }
    /// <summary> インタラクトボタン押下中 </summary>
    static public bool GetInteract { get => _interactAction.IsPressed(); }
    /// <summary> 射撃ボタン押下直後 </summary>
    static public bool GetDownFire { get => _fireAction.triggered; }
    /// <summary> 射撃ボタン押下中 </summary>
    static public bool GetFire { get => _fireAction.IsPressed(); }
    /// <summary> リロードボタン押下直後 </summary>
    static public bool GetDownReload { get => _reloadAction.triggered; }
    /// <summary> 回避ボタン押下直後 </summary>
    static public bool GetDownRun { get => _runAction.triggered; }
    /// <summary> 回避ボタン押下中 </summary>
    static public bool GetRun { get => _runAction.IsPressed(); }
    /// <summary> 決定ボタン押下直後 </summary>
    static public bool GetDownDecide { get => _decideAction.triggered; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //入力を関連付け
        PlayerInput input = GetComponent<PlayerInput>();
        InputActionMap actionMap = input.currentActionMap;
        _startAction = actionMap[_buttonNameStart];
        _pauseAction = actionMap[_buttonNamePause];
        _optionAction = actionMap[_buttonNameOption];
        _moveDirectionAction = actionMap[_stickNameMoveDirection];
        _cameraMoveAction = actionMap[_stickNameCameraMove];
        _jumpAction = actionMap[_buttonNameJump];
        _interactAction = actionMap[_buttonNameInteract];
        _fireAction = actionMap[_buttonNameFire];
        _reloadAction = actionMap[_buttonNameReload];
        _runAction = actionMap[_buttonNameRun];
        _decideAction = actionMap[_buttonNameDecide];

    }
    /*
        void OnDestroy()
        {
            StopShakeController();
        }

        /// <summary> コントローラーの振動を促す。ただし、数値は0～1の範囲で。範囲を超える場合はClampする。 </summary>
        /// <param name="leftPower">左側のモーター強度</param>
        /// <param name="rightPower">右側のモーター強度</param>
        static public void SimpleShakeController(float leftPower, float rightPower)
        {
            if (_Gamepad == null) _Gamepad = Gamepad.current;
            if (_Gamepad != null)
            {
                _Gamepad.SetMotorSpeeds(Mathf.Clamp01(leftPower), Mathf.Clamp01(rightPower));
            }
        }

        /// <summary>コントローラーの振動を止める</summary>
        static public void StopShakeController()
        {
            if (_Gamepad == null) _Gamepad = Gamepad.current;
            if (_Gamepad != null)
            {
                _Gamepad.SetMotorSpeeds(0f, 0f);
            }
        }

        /// <summary>コントローラーの振動を一定間隔で</summary>
        IEnumerator PalusShake()
        {
            while (enabled)
            {
                _TweenLeftShakePower = _LeftShakePower;
                _TweenRightShakePower = _rightShakePower;

                yield return new WaitForSeconds(_shakeInterval);

                _TweenLeftShakePower = 0f;
                _TweenRightShakePower = 0f;

                yield return new WaitForSeconds(_UnShakeInterval);
            }
        }

        /// <summary>コントローラーの振動を一定間隔で</summary>
        IEnumerator PalusTweenShake()
        {
            while (enabled)
            {
                DOTween.To(() => _TweenLeftShakePower, f => _TweenLeftShakePower = f, 0, _shakeInterval).SetEase(_TweenShakeModeLeft);
                DOTween.To(() => _TweenRightShakePower, f => _TweenRightShakePower = f, 0, _shakeInterval).SetEase(_TweenShakeModeRight);

                yield return new WaitForSeconds(_shakeInterval);

                _TweenLeftShakePower = 0f;
                _TweenRightShakePower = 0f;

                yield return new WaitForSeconds(_UnShakeInterval);

                _TweenLeftShakePower = _LeftShakePower;
                _TweenRightShakePower = _rightShakePower;
            }
        }

        /// <summary>コントローラーの振動を一定間隔で</summary>
        IEnumerator PalusTestShake()
        {
            while (enabled)
            {
                Sequence seqLeft = DOTween.Sequence();
                Sequence seqRight = DOTween.Sequence();
                seqLeft.Append(DOTween.To(() => _TweenLeftShakePower, f => _TweenLeftShakePower = f, 0, _shakeInterval).SetEase(Ease.InCubic));
                seqRight.Append(DOTween.To(() => _TweenRightShakePower, f => _TweenRightShakePower = f, _rightShakePower, _shakeInterval / 2f).SetEase(Ease.InCubic));
                seqRight.Append(DOTween.To(() => _TweenRightShakePower, f => _TweenRightShakePower = f, 0, _shakeInterval / 2f).SetEase(Ease.OutCubic));
                seqLeft.Play();
                seqRight.Play();

                yield return seqRight.WaitForCompletion();

                _TweenLeftShakePower = 0f;
                _TweenRightShakePower = 0f;

                yield return new WaitForSeconds(_UnShakeInterval);

                _TweenLeftShakePower = _LeftShakePower;
            }
        }
    */
}
