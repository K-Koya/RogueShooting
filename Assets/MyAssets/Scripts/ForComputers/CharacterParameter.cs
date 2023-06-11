using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParameter : MonoBehaviour
{
    /// <summary>�ő及���e�퐔</summary>
    protected const byte INVENTORY_SIZE = 5;


    [SerializeField, Tooltip("�ő僉�C�t")]
    protected short _maxLife = 100;

    [SerializeField, Tooltip("���݃��C�t")]
    protected short _currentLife = 100;



    /// <summary>�ړ��ۃr�b�g�t���O</summary>
    protected MotionEnableFlag _can = (MotionEnableFlag)byte.MaxValue;

    /// <summary>�s�����</summary>
    protected MotionState _state = null;

    /// <summary>����̎����萧��</summary>
    protected SetWeaponRig _weponRig = null;




    [SerializeField, Tooltip("�L�����N�^�[�̖ڐ��ʒu")]
    protected Transform _eyePoint = null;

    /// <summary>�Ə���̈ʒu</summary>
    protected Vector3 _reticlePoint = Vector3.zero;

    [SerializeField, Tooltip("�L�����N�^�[�̎��")]
    protected CharacterKind _character = CharacterKind.Player;

    [SerializeField, Tooltip("�L�����N�^�[�̌������")]
    protected Vector3 _characterDirection = Vector3.zero;

    [SerializeField, Tooltip("�L�����N�^�[�̈ړ��������")]
    protected Vector3 _moveDirection = Vector3.zero;

    [SerializeField, Tooltip("�L�����N�^�[�̒����������")]
    protected Vector3 _lookDirection = Vector3.forward;

    [SerializeField, Tooltip("�����e����")]
    protected GunInfo[] _inventory = null;

    /// <summary>�����e��̂����A��Ɏ����Ă�����̂̔ԍ�</summary>
    protected byte _inventoryNumber = 0;

    /// <summary>���ݎ���</summary>
    protected float _hurtTime = 0f;

    /// <summary>true : ���񂾂��A�j���[�^�[�ɏ��𑗂���</summary>
    bool _isHurtTriggered = false;


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

    /// <summary>�L�����N�^�[�̒����������</summary>
    public Vector3 LookDirection { get => _lookDirection; set => _lookDirection = value; }

    /// <summary>�����e��̂����A��Ɏ����Ă�����̂̔ԍ�</summary>
    public byte InventoryNumber { get => _inventoryNumber; }

    /// <summary>�����e��̂����A��Ɏ����Ă������</summary>
    public GunInfo UsingGun { get => _inventory[_inventoryNumber]; }

    public bool IsHurt 
    { 
        get
        {
            bool info = _isHurtTriggered;
            _isHurtTriggered = false;
            return info;
        }
    }
    #endregion

    // Start is called before the first frame update
    protected virtual void Start()
    {
        _hurtTime = 0f;

        _can = 0;
        _state = new MotionState();
        _state.Kind = MotionState.StateKind.Stay;
        _state.Process = MotionState.ProcessKind.Playing;

        _weponRig = GetComponentInChildren<SetWeaponRig>();

        if (!_eyePoint)
        {
            _eyePoint = transform;
        }

        //�����̏��������o�^
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
        _weponRig.DoSet(_inventory[_inventoryNumber].gameObject);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if(_state.Kind is MotionState.StateKind.Defeat)
        {
            return;
        }

        SetMotionEnableFlag();
    }

    /// <summary>��Ɏ����Ă���e�����ւ��鏈��</summary>
    /// <param name="index">�Ώ۔ԍ�</param>
    public void SwitchGun(byte index = 0)
    {
        _weponRig.DoSet(_inventory[index].gameObject);

        UsingGun.DoSwitch();
        _inventoryNumber = index;
    }

    /// <summary>�������Ă���e���S�ė��Ƃ�</summary>
    public void ReleaseGunAll()
    {
        _weponRig.DoRelease();
        foreach (GunInfo gun in _inventory)
        {
            gun.DoRelease();
            gun.transform.parent = null;
        }
    }

    /// <summary>��_���[�W����</summary>
    /// <param name="damage">�_���[�W��</param>
    /// <param name="impact">�Ռ�</param>
    /// <param name="impactDirection">�Ռ��̕���</param>
    public virtual void GaveDamage(short damage, float impact, Vector3 impactDirection)
    {
        _currentLife -= damage;
        if (_currentLife < 1)
        {
            _state.Kind = MotionState.StateKind.Defeat;
            //ReleaseGunAll();
        }
        //���C�t���Ȃ��Ȃ莟��|���ꂽ��Ԃ�
        else
        {
            _state.Kind = MotionState.StateKind.Hurt;
            _isHurtTriggered = true;
        }
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

/// <summary>�L�����N�^�[�̎��</summary>
public enum CharacterKind : byte
{
    Player = 0,
    BodyGuard1 = 1,
    BodyGuard2 = 2,
    BodyGuard3 = 3,
    Terrorist = 11,
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
