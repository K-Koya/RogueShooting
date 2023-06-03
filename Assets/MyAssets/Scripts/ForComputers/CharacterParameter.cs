using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameter : MonoBehaviour
{
    /// <summary>�ő及���e�퐔</summary>
    protected const byte INVENTORY_SIZE = 5;

    /// <summary>�ړ��ۃr�b�g�t���O</summary>
    MotionEnableFlag _can = (MotionEnableFlag)byte.MaxValue;

    /// <summary>�s�����</summary>
    MotionState _state = null;





    [SerializeField, Tooltip("�L�����N�^�[�̖ڐ��ʒu")]
    protected Transform _eyePoint = null;

    /// <summary>�Ə���̈ʒu</summary>
    protected Vector3 _reticlePoint = Vector3.zero;

    [SerializeField, Tooltip("�L�����N�^�[�̌������")]
    protected Vector3 _characterDirection = Vector3.zero;

    [SerializeField, Tooltip("�L�����N�^�[�̈ړ��������")]
    protected Vector3 _moveDirection = Vector3.zero;

    /// <summary>�����e����</summary>
    protected GunInfo[] _inventory = null;

    /// <summary>�����e��̂����A��Ɏ����Ă�����̂̔ԍ�</summary>
    protected byte _inventoryNumber = 0;


    #region �v���p�e�B
    /// <summary>�ړ��ۃr�b�g�t���O</summary>
    public MotionEnableFlag Can { get => _can; }

    /// <summary>�s�����</summary>
    public MotionState State { get => _state; }

    /// <summary>�L�����N�^�[�̖ڐ��ʒu</summary>
    public Transform EyePoint { get => _eyePoint; }

    /// <summary>�L�����N�^�[�̌������</summary>
    public Vector3 CharDirection { get => _characterDirection; set => _characterDirection = value; }

    /// <summary>�L�����N�^�[�̈ړ��������</summary>
    public Vector3 MoveDirection { get => _moveDirection; set => _moveDirection = value; }

    /// <summary>�����e��̂����A��Ɏ����Ă�����̂̔ԍ�</summary>
    public byte InventoryNumber { get => _inventoryNumber; }

    /// <summary>�����e��̂����A��Ɏ����Ă������</summary>
    public GunInfo UsingGun { get => _inventory[_inventoryNumber]; }
    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _can = 0;
        _state = new MotionState();
        _state.Kind = MotionState.StateKind.Stay;
        _state.Process = MotionState.ProcessKind.Playing;

        if (!_eyePoint)
        {
            _eyePoint = transform;
        }


        _inventory = new GunInfo[INVENTORY_SIZE];
        GunInfo[] infos = GetComponentsInChildren<GunInfo>();
        for(int i = 0; i < INVENTORY_SIZE; i++)
        {
            if(i < infos.Length)
            {
                _inventory[i] = infos[i];
            }
        }
        _inventoryNumber = 0;
    }

    // Update is called once per frame
    protected virtual void Update()
    {


        SetMotionEnableFlag();
    }

    /// <summary>��Ɏ����Ă���e�����ւ��鏈��</summary>
    /// <param name="index">�Ώ۔ԍ�</param>
    public void SwitchGun(byte index = 0)
    {
        UsingGun.DoSwitch();
        _inventoryNumber = index;
    }

    /// <summary>�X�e�[�g�ɉ����ē��싖�t���O���w��</summary>
    void SetMotionEnableFlag()
    {
        switch (_state.Kind)
        {
            case MotionState.StateKind.Stay:
            case MotionState.StateKind.Walk:
                _can = MotionEnableFlag.Walk
                        | MotionEnableFlag.Run
                        | MotionEnableFlag.Jump
                        | MotionEnableFlag.FireHipUse;

                break;
            case MotionState.StateKind.Run:
                _can = MotionEnableFlag.Walk
                        | MotionEnableFlag.Run
                        | MotionEnableFlag.Jump
                        | MotionEnableFlag.FireHipUse;

                break;

            case MotionState.StateKind.JumpNoraml:
                _can = MotionEnableFlag.FireHipUse;

                break;
            case MotionState.StateKind.FallNoraml:
                _can = MotionEnableFlag.FireHipUse;

                break;
            case MotionState.StateKind.FireHipUse:
                _can = MotionEnableFlag.Walk
                        | MotionEnableFlag.Run
                        | MotionEnableFlag.Jump
                        | MotionEnableFlag.FireHipUse
                        | MotionEnableFlag.FireLookInto;

                break;
            case MotionState.StateKind.FireLookInto:
                _can = MotionEnableFlag.Walk
                        | MotionEnableFlag.FireHipUse;

                break;
            case MotionState.StateKind.Hurt:
                _can = 0;

                break;
            case MotionState.StateKind.Defeat:
                _can = 0;

                break;
            default: break;
        }
    }
}

/// <summary>�ړ��ۃr�b�g�t���O</summary>
public enum MotionEnableFlag : byte
{
    Walk = 1,
    Run = 2,
    Jump = 4,
    FireHipUse = 8,
    FireLookInto = 16,

}
