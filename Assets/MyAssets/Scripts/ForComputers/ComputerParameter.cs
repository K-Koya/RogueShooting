using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class ComputerParameter : CharacterParameter, IReticleFocused
{
    /// <summary>�|���ׂ��G�̐l��</summary>
    static byte _DefeatedEnemyQuota = 0;

    /// <summary>�|�����G�̐l��</summary>
    static byte _DefeatedEnemyCount = 0;

    /// <summary>�Ə����̃u���̑傫���̊�{�l</summary>
    static float _BaseAccuracyAim = 1f;

    [SerializeField, Tooltip("���ꂽ���������郉�O�h�[���̃v���n�u")]
    GameObject _defeatedRagdollPref = null;

    [SerializeField, Tooltip("���E�̋���")]
    float _range = 20f;

    [SerializeField, Tooltip("�������܂ł̎���")]
    float _missingTime = 5f;

    /// <summary>�������^�C�}�[</summary>
    float _timer = 0f;

    /// <summary>�P���^�[�Q�b�g</summary>
    CharacterParameter _target = null;


    #region �v���p�e�B
    /// <summary>�|���ׂ��G�̐l��</summary>
    public static byte DefeatedEnemyQuota => _DefeatedEnemyQuota;
    /// <summary>�|�����G�̐l��</summary>
    public static byte DefeatedEnemyCount => _DefeatedEnemyCount;
    /// <summary>�Ə����̃u���̑傫���̊�{�l</summary>
    public static float BaseAccuracyAim => _BaseAccuracyAim;
    /// <summary>�P���^�[�Q�b�g</summary>
    public CharacterParameter Target => _target;
    /// <summary>true : �^�[�Q�b�g�ւ̎ː����ʂ��Ă���</summary>
    public bool IsThroughLineOfSight => _missingTime <= _timer;

    #endregion


    /// <summary>���̃X�e�[�W�ɂ�����G���̏�����</summary>
    /// <param name="quota">�|���G�̐l���m���}</param>
    /// <param name="baseAim">�G�̏Ə����x�̃x�[�X�l</param>
    public static void SetEnemyData(byte quota, float baseAim)
    {
        _DefeatedEnemyQuota = quota;
        _DefeatedEnemyCount = 0;
        _BaseAccuracyAim = baseAim;
    }


    protected override void Start()
    {
        base.Start();
        _Enemies.Add(this);
    }

    protected override void Update()
    {
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        base.Update();
        SeekInSight();
    }

    private void OnDestroy()
    {
        _Enemies.Remove(this);
    }

    /// <summary>���E���̃^�[�Q�b�g�T��</summary>
    public void SeekInSight()
    {
        //�܂��A�^�[�Q�b�g��F�����Ă��邩�ŕ���
        Vector3 toTarget;
        RaycastHit hit;
        if (_target)
        {
            toTarget = Vector3.Normalize(_target.EyePoint.position - _eyePoint.position);

            //�^�[�Q�b�g���狗��������Ă���
            if (Vector3.SqrMagnitude(toTarget) > _range * _range)
            {
                //�������J�E���g�_�E��
                _timer -= Time.deltaTime;
                if (_timer < 0f)
                {
                    //���Ԑ؂�Ń^�[�Q�b�g����
                    _target = null;
                }
            }
            //���C���΂��A�Ԃɏ�Q�����Ȃ����m�F
            else if (Physics.Raycast(_eyePoint.position, toTarget, out hit, _range, LayerManager.Instance.EnemyFocusable))
            {
                if (hit.collider.gameObject != _target.gameObject)
                {
                    //��Q���Ȃ猩�����J�E���g�_�E��
                    _timer -= Time.deltaTime;
                    if (_timer < 0f)
                    {
                        //���Ԑ؂�Ń^�[�Q�b�g����
                        _target = null;
                    }
                }
                else
                {
                    _timer = _missingTime;
                }
            }
        }
        else
        {
            _timer = _missingTime;
            foreach (CharacterParameter parameter in _Allies)
            {
                toTarget = Vector3.Normalize(parameter.EyePoint.position - _eyePoint.position);

                //���ςƋ�����r�ŃR�[�����ɂ��邩�T��
                if (Vector3.Dot(toTarget, _lookDirection) > 0.5f
                    && Vector3.SqrMagnitude(toTarget) < _range * _range)
                {
                    //���C���΂��A�Ԃɏ�Q�����Ȃ����m�F
                    if (Physics.Raycast(_eyePoint.position, toTarget, out hit, _range, LayerManager.Instance.EnemyFocusable))
                    {
                        if (hit.collider.gameObject == parameter.gameObject)
                        {
                            //�^�[�Q�b�g�o�^
                            _target = parameter;
                            break;
                        }
                    }
                }
            }
        }
    }

    /// <summary>�Ə������킹���ۂɃf�[�^���J��</summary>
    public ReticleFocusedData GetData()
    {
        ReticleFocusedData data = new ReticleFocusedData();
        data._maxLife = _maxLife;
        data._currentLife = _currentLife;
        data._name = _character.ToString();

        return data;
    }

    public override void GaveDamage(short damage, float impact, Vector3 impactDirection)
    {
        base.GaveDamage(damage, impact, impactDirection);

        if (_state.Kind is MotionState.StateKind.Defeat)
        {
            GameObject ragdoll = Instantiate(_defeatedRagdollPref);
            ragdoll.transform.position = transform.position;
            ragdoll.transform.rotation = transform.rotation;
            ragdoll.GetComponent<DefeatedRagdoll>().BlowAway(impactDirection * impact);

            _DefeatedEnemyCount++;

            Destroy(gameObject, 0.1f);
        }
    }


}
