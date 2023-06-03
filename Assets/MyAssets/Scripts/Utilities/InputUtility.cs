using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// InputManager�Ŏg���Ă���{�^�����̕�������Ǘ�
/// </summary>
[RequireComponent(typeof(PlayerInput))]
public class InputUtility : Singleton<InputUtility>
{
    #region InputAction��Actions�L�[
    [Header("�ȉ��AInputAction��Actions�ɓo�^�������O��o�^")]
    [SerializeField, Tooltip("InputAction�ɂ�����A�X�^�[�g�{�^����")]
    string _buttonNameStart = "Start";

    [SerializeField, Tooltip("InputAction�ɂ�����A�|�[�Y���j���[�{�^����")]
    string _buttonNamePause = "Pause";

    [SerializeField, Tooltip("InputAction�ɂ�����A���X�g���j���[�{�^����")]
    string _buttonNameOption = "Option";

    [SerializeField, Tooltip("InputAction�ɂ�����A�ړ������̓��͖�")]
    string _stickNameMoveDirection = "MoveDirection";

    [SerializeField, Tooltip("InputAction�ɂ�����A�J�����ړ����͖�")]
    string _stickNameCameraMove = "CameraMove";

    [SerializeField, Tooltip("InputAction�ɂ�����A�W�����v���͖�")]
    string _buttonNameJump = "Jump";

    [SerializeField, Tooltip("InputAction�ɂ�����A�C���^���N�g���͖�")]
    string _buttonNameInteract = "Interact";

    [SerializeField, Tooltip("InputAction�ɂ�����A�ˌ����͖�")]
    string _buttonNameFire = "Fire";

    [SerializeField, Tooltip("InputAction�ɂ�����A�����[�h���͖�")]
    string _buttonNameReload = "Reload";

    [SerializeField, Tooltip("InputAction�ɂ�����A�����͖�")]
    string _buttonNameRun = "Run";

    [SerializeField, Tooltip("InputAction�ɂ�����A����{�^�����͖�")]
    string _buttonNameDecide = "Decide";

    #endregion
    /*
    #region �R���g���[���[�U���p�����o
    /// <summary> �R���g���[���[ </summary>
    //static Gamepad _Gamepad = default;

    [Header("�R���g���[���[�U���p�ɗp����p�����[�^�[")]
    [SerializeField, Range(0, 1), Tooltip("�R���g���[���[�̉E���̐U���̋���")]
    float _rightShakePower = 0.5f;

    /// <summary>DOTween�ۊǗp : �R���g���[���[�̉E���̐U���̋���</summary>
    float _TweenRightShakePower = 0f;

    [SerializeField, Range(0, 1), Tooltip("�R���g���[���[�̍����̐U���̋���")]
    float _LeftShakePower = 0.5f;

    /// <summary>DOTween�ۊǗp : �R���g���[���[�̍����̐U���̋���</summary>
    float _TweenLeftShakePower = 0f;

    [SerializeField, Tooltip("�R���g���[���[�̐U�������鎞��")]
    float _shakeInterval = 0.75f;

    [SerializeField, Tooltip("�R���g���[���[�̐U�����Ă��Ȃ�����")]
    float _UnShakeInterval = 0.75f;

    [SerializeField, Tooltip("DOTween���g���čׂ����U����������ꍇ�̎g��Easing�^�C�v")]
    Ease _TweenShakeModeRight = Ease.Linear;

    [SerializeField, Tooltip("DOTween���g���đ傫�ȐU����������ꍇ�̎g��Easing�^�C�v")]
    Ease _TweenShakeModeLeft = Ease.Linear;
    #endregion
    */

    #region InputAction
    /// <summary> �X�^�[�g�{�^���̓��͏� </summary>
    static InputAction _startAction = default;

    /// <summary> �|�[�Y���j���[�N���̓��͏� </summary>
    static InputAction _pauseAction = default;

    /// <summary> ���X�g���j���[�N���̓��͏� </summary>
    static InputAction _optionAction = default;

    /// <summary> �ړ�����̓��͏� </summary>
    static InputAction _moveDirectionAction = default;

    /// <summary> �J�����ړ�����̓��͏� </summary>
    static InputAction _cameraMoveAction = default;

    /// <summary> �W�����v�̓��͏� </summary>
    static InputAction _jumpAction = default;

    /// <summary> �C���^���N�g�{�^���̓��͏� </summary>
    static InputAction _interactAction = default;

    /// <summary> �ˌ��{�^���̓��͏� </summary>
    static InputAction _fireAction = default;

    /// <summary> �����[�h�{�^���̓��͏� </summary>
    static InputAction _reloadAction = default;

    /// <summary> ����{�^���̓��͏� </summary>
    static InputAction _runAction = default;

    /// <summary> ����{�^���̓��͏� </summary>
    static InputAction _decideAction = default;
    #endregion

    #region �v���p�e�B
    /// <summary> �X�^�[�g�{�^���������� </summary>
    static public bool GetDownStart { get => _startAction.triggered; }
    /// <summary> �|�[�Y�{�^���������� </summary>
    static public bool GetDownPause { get => _pauseAction.triggered; }
    /// <summary> ���X�g���j���[�{�^���������� </summary>
    static public bool GetDownOption { get => _optionAction.triggered; }
    /// <summary> �ړ����쒼�� </summary>
    static public bool GetMoveDown { get => _moveDirectionAction.triggered; }
    /// <summary> �ړ�����̕����擾 </summary>
    static public Vector2 GetMoveDirection { get => _moveDirectionAction.ReadValue<Vector2>(); }
    /// <summary> �ړ�����I�� </summary>
    static public bool GetMoveUp { get => _moveDirectionAction.WasReleasedThisFrame(); }
    /// <summary> �J�������쒼�� </summary>
    static public bool GetCameraMoveDown { get => _cameraMoveAction.triggered; }
    /// <summary> �J��������̕����擾 </summary>
    static public Vector2 GetCameraMoveDirection { get => _cameraMoveAction.ReadValue<Vector2>(); }
    /// <summary> �J��������I�� </summary>
    static public bool GetCameraMoveUp { get => _cameraMoveAction.WasReleasedThisFrame(); }
    /// <summary> �W�����v�{�^���������� </summary>
    static public bool GetDownJump { get => _jumpAction.triggered; }
    /// <summary> �W�����v�{�^�������� </summary>
    static public bool GetJump { get => _jumpAction.IsPressed(); }
    /// <summary> �C���^���N�g�{�^���������� </summary>
    static public bool GetDownInteract { get => _interactAction.triggered; }
    /// <summary> �C���^���N�g�{�^�������� </summary>
    static public bool GetInteract { get => _interactAction.IsPressed(); }
    /// <summary> �ˌ��{�^���������� </summary>
    static public bool GetDownFire { get => _fireAction.triggered; }
    /// <summary> �ˌ��{�^�������� </summary>
    static public bool GetFire { get => _fireAction.IsPressed(); }
    /// <summary> �����[�h�{�^���������� </summary>
    static public bool GetDownReload { get => _reloadAction.triggered; }
    /// <summary> ����{�^���������� </summary>
    static public bool GetDownRun { get => _runAction.triggered; }
    /// <summary> ����{�^�������� </summary>
    static public bool GetRun { get => _runAction.IsPressed(); }
    /// <summary> ����{�^���������� </summary>
    static public bool GetDownDecide { get => _decideAction.triggered; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //���͂��֘A�t��
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

        /// <summary> �R���g���[���[�̐U���𑣂��B�������A���l��0�`1�͈̔͂ŁB�͈͂𒴂���ꍇ��Clamp����B </summary>
        /// <param name="leftPower">�����̃��[�^�[���x</param>
        /// <param name="rightPower">�E���̃��[�^�[���x</param>
        static public void SimpleShakeController(float leftPower, float rightPower)
        {
            if (_Gamepad == null) _Gamepad = Gamepad.current;
            if (_Gamepad != null)
            {
                _Gamepad.SetMotorSpeeds(Mathf.Clamp01(leftPower), Mathf.Clamp01(rightPower));
            }
        }

        /// <summary>�R���g���[���[�̐U�����~�߂�</summary>
        static public void StopShakeController()
        {
            if (_Gamepad == null) _Gamepad = Gamepad.current;
            if (_Gamepad != null)
            {
                _Gamepad.SetMotorSpeeds(0f, 0f);
            }
        }

        /// <summary>�R���g���[���[�̐U�������Ԋu��</summary>
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

        /// <summary>�R���g���[���[�̐U�������Ԋu��</summary>
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

        /// <summary>�R���g���[���[�̐U�������Ԋu��</summary>
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
