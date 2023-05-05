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
    string _ButtonNameStart = "Start";

    [SerializeField, Tooltip("InputAction�ɂ�����A�|�[�Y���j���[�{�^����")]
    string _ButtonNamePause = "Pause";

    [SerializeField, Tooltip("InputAction�ɂ�����A���X�g���j���[�{�^����")]
    string _ButtonNameOption = "Option";

    [SerializeField, Tooltip("InputAction�ɂ�����A�ړ������̓��͖�")]
    string _StickNameMoveDirection = "MoveDirection";

    [SerializeField, Tooltip("InputAction�ɂ�����A�J�����ړ����͖�")]
    string _StickNameCameraMove = "CameraMove";

    [SerializeField, Tooltip("InputAction�ɂ�����A�W�����v���͖�")]
    string _ButtonNameJump = "Jump";

    [SerializeField, Tooltip("InputAction�ɂ�����A����R�}���h���͖�")]
    string _ButtonNameAimCommand = "AimCommand";

    [SerializeField, Tooltip("InputAction�ɂ�����A�U�����͖�")]
    string _ButtonNameAttack = "Attack";

    [SerializeField, Tooltip("InputAction�ɂ�����A�K�[�h���͖�")]
    string _ButtonNameGuard = "Guard";

    [SerializeField, Tooltip("InputAction�ɂ�����A�����͖�")]
    string _ButtonNameDodge = "Dodge";

    [SerializeField, Tooltip("InputAction�ɂ�����A����{�^�����͖�")]
    string _ButtonNameDecide = "Decide";

    [SerializeField, Tooltip("InputAction�ɂ�����A�A�N�e�B�u�X�L���R�}���h�̓��͖�")]
    string _ButtonNameSkillCommand = "SkillCommand";
    #endregion
    /*
    #region �R���g���[���[�U���p�����o
    /// <summary> �R���g���[���[ </summary>
    //static Gamepad _Gamepad = default;

    [Header("�R���g���[���[�U���p�ɗp����p�����[�^�[")]
    [SerializeField, Range(0, 1), Tooltip("�R���g���[���[�̉E���̐U���̋���")]
    float _RightShakePower = 0.5f;

    /// <summary>DOTween�ۊǗp : �R���g���[���[�̉E���̐U���̋���</summary>
    float _TweenRightShakePower = 0f;

    [SerializeField, Range(0, 1), Tooltip("�R���g���[���[�̍����̐U���̋���")]
    float _LeftShakePower = 0.5f;

    /// <summary>DOTween�ۊǗp : �R���g���[���[�̍����̐U���̋���</summary>
    float _TweenLeftShakePower = 0f;

    [SerializeField, Tooltip("�R���g���[���[�̐U�������鎞��")]
    float _ShakeInterval = 0.75f;

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
    static InputAction _StartAction = default;

    /// <summary> �|�[�Y���j���[�N���̓��͏� </summary>
    static InputAction _PauseAction = default;

    /// <summary> ���X�g���j���[�N���̓��͏� </summary>
    static InputAction _OptionAction = default;

    /// <summary> �ړ�����̓��͏� </summary>
    static InputAction _MoveDirectionAction = default;

    /// <summary> �J�����ړ�����̓��͏� </summary>
    static InputAction _CameraMoveAction = default;

    /// <summary> �W�����v�̓��͏� </summary>
    static InputAction _JumpAction = default;

    /// <summary> ����R�}���h�{�^���̓��͏� </summary>
    static InputAction _AimCommandAction = default;

    /// <summary> �U���{�^���̓��͏� </summary>
    static InputAction _AttackAction = default;

    /// <summary> �K�[�h�{�^���̓��͏� </summary>
    static InputAction _GuardAction = default;

    /// <summary> ����{�^���̓��͏� </summary>
    static InputAction _DodgeAction = default;

    /// <summary> ����{�^���̓��͏� </summary>
    static InputAction _DecideAction = default;

    /// <summary> �A�N�e�B�u�X�L���R�}���h4�̓��͏� </summary>
    static InputAction[] _SkillCommandActions = default;
    #endregion

    #region �v���p�e�B
    /// <summary> �X�^�[�g�{�^���������� </summary>
    static public bool GetDownStart { get => _StartAction.triggered; }
    /// <summary> �|�[�Y�{�^���������� </summary>
    static public bool GetDownPause { get => _PauseAction.triggered; }
    /// <summary> ���X�g���j���[�{�^���������� </summary>
    static public bool GetDownOption { get => _OptionAction.triggered; }
    /// <summary> �ړ����쒼�� </summary>
    static public bool GetMoveDown { get => _MoveDirectionAction.triggered; }
    /// <summary> �ړ�����̕����擾 </summary>
    static public Vector2 GetMoveDirection { get => _MoveDirectionAction.ReadValue<Vector2>(); }
    /// <summary> �ړ�����I�� </summary>
    static public bool GetMoveUp { get => _MoveDirectionAction.WasReleasedThisFrame(); }
    /// <summary> �J�������쒼�� </summary>
    static public bool GetCameraMoveDown { get => _CameraMoveAction.triggered; }
    /// <summary> �J��������̕����擾 </summary>
    static public Vector2 GetCameraMoveDirection { get => _CameraMoveAction.ReadValue<Vector2>(); }
    /// <summary> �J��������I�� </summary>
    static public bool GetCameraMoveUp { get => _CameraMoveAction.WasReleasedThisFrame(); }
    /// <summary> �W�����v�{�^���������� </summary>
    static public bool GetDownJump { get => _JumpAction.triggered; }
    /// <summary> �W�����v�{�^�������� </summary>
    static public bool GetJump { get => _JumpAction.IsPressed(); }
    /// <summary> ����R�}���h�{�^���������� </summary>
    static public bool GetDownAimCommand { get => _AimCommandAction.triggered; }
    /// <summary> ����R�}���h�{�^�������� </summary>
    static public bool GetAimCommand { get => _AimCommandAction.IsPressed(); }
    /// <summary> �U���{�^���������� </summary>
    static public bool GetDownAttack { get => _AttackAction.triggered; }
    /// <summary> �U���{�^�������� </summary>
    static public bool GetAttack { get => _AttackAction.IsPressed(); }
    /// <summary> �K�[�h�{�^���������� </summary>
    static public bool GetDownGuard { get => _GuardAction.triggered; }
    /// <summary> �K�[�h�{�^�������� </summary>
    static public bool GetGuard { get => _GuardAction.IsPressed(); }
    /// <summary> ����{�^���������� </summary>
    static public bool GetDownDodge { get => _DodgeAction.triggered; }
    /// <summary> ����{�^�������� </summary>
    static public bool GetDodge { get => _DodgeAction.IsPressed(); }
    /// <summary> ����{�^���������� </summary>
    static public bool GetDownDecide { get => _DecideAction.triggered; }
    /// <summary> 1�Ԗڂ̃A�N�e�B�u�X�L���R�}���h�{�^���������� </summary>
    static public bool GetSkillCommand1 { get => _SkillCommandActions[0].IsPressed(); }
    /// <summary> 1�Ԗڂ̃A�N�e�B�u�X�L���R�}���h�{�^�������� </summary>
    static public bool GetDownSkillCommand1 { get => _SkillCommandActions[0].triggered; }
    /// <summary> 2�Ԗڂ̃A�N�e�B�u�X�L���R�}���h�{�^���������� </summary>
    static public bool GetSkillCommand2 { get => _SkillCommandActions[1].IsPressed(); }
    /// <summary> 2�Ԗڂ̃A�N�e�B�u�X�L���R�}���h�{�^�������� </summary>
    static public bool GetDownSkillCommand2 { get => _SkillCommandActions[1].triggered; }
    /// <summary> 3�Ԗڂ̃A�N�e�B�u�X�L���R�}���h�{�^���������� </summary>
    static public bool GetSkillCommand3 { get => _SkillCommandActions[2].IsPressed(); }
    /// <summary> 3�Ԗڂ̃A�N�e�B�u�X�L���R�}���h�{�^�������� </summary>
    static public bool GetDownSkillCommand3 { get => _SkillCommandActions[2].triggered; }
    /// <summary> 4�Ԗڂ̃A�N�e�B�u�X�L���R�}���h�{�^���������� </summary>
    static public bool GetSkillCommand4 { get => _SkillCommandActions[3].IsPressed(); }
    /// <summary> 4�Ԗڂ̃A�N�e�B�u�X�L���R�}���h�{�^�������� </summary>
    static public bool GetDownSkillCommand4 { get => _SkillCommandActions[3].triggered; }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //���͂��֘A�t��
        PlayerInput input = GetComponent<PlayerInput>();
        InputActionMap actionMap = input.currentActionMap;
        _StartAction = actionMap[_ButtonNameStart];
        _PauseAction = actionMap[_ButtonNamePause];
        _OptionAction = actionMap[_ButtonNameOption];
        _MoveDirectionAction = actionMap[_StickNameMoveDirection];
        _CameraMoveAction = actionMap[_StickNameCameraMove];
        _JumpAction = actionMap[_ButtonNameJump];
        _AimCommandAction = actionMap[_ButtonNameAimCommand];
        _AttackAction = actionMap[_ButtonNameAttack];
        _GuardAction = actionMap[_ButtonNameGuard];
        _DodgeAction = actionMap[_ButtonNameDodge];
        _DecideAction = actionMap[_ButtonNameDecide];

        _SkillCommandActions = new InputAction[4];
        for (int i = 0; i < _SkillCommandActions.Length; i++)
        {
            _SkillCommandActions[i] = actionMap[$"{_ButtonNameSkillCommand}{i + 1}"];
        }
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
                _TweenRightShakePower = _RightShakePower;

                yield return new WaitForSeconds(_ShakeInterval);

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
                DOTween.To(() => _TweenLeftShakePower, f => _TweenLeftShakePower = f, 0, _ShakeInterval).SetEase(_TweenShakeModeLeft);
                DOTween.To(() => _TweenRightShakePower, f => _TweenRightShakePower = f, 0, _ShakeInterval).SetEase(_TweenShakeModeRight);

                yield return new WaitForSeconds(_ShakeInterval);

                _TweenLeftShakePower = 0f;
                _TweenRightShakePower = 0f;

                yield return new WaitForSeconds(_UnShakeInterval);

                _TweenLeftShakePower = _LeftShakePower;
                _TweenRightShakePower = _RightShakePower;
            }
        }

        /// <summary>�R���g���[���[�̐U�������Ԋu��</summary>
        IEnumerator PalusTestShake()
        {
            while (enabled)
            {
                Sequence seqLeft = DOTween.Sequence();
                Sequence seqRight = DOTween.Sequence();
                seqLeft.Append(DOTween.To(() => _TweenLeftShakePower, f => _TweenLeftShakePower = f, 0, _ShakeInterval).SetEase(Ease.InCubic));
                seqRight.Append(DOTween.To(() => _TweenRightShakePower, f => _TweenRightShakePower = f, _RightShakePower, _ShakeInterval / 2f).SetEase(Ease.InCubic));
                seqRight.Append(DOTween.To(() => _TweenRightShakePower, f => _TweenRightShakePower = f, 0, _ShakeInterval / 2f).SetEase(Ease.OutCubic));
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
