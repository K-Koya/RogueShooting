using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ComputerParameter))]
[RequireComponent(typeof(ComputerMove))]
public class AIEnemyBase : MonoBehaviour
{
    [SerializeField, Tooltip("������͈̔�")]
    float _wanderDistance = 20f;

    /// <summary>�Y���L�����N�^�[�̃p�����[�^</summary>
    ComputerParameter _param = null;

    /// <summary>�Y���L�����N�^�[�̈ړ��w���R���|�[�l���g</summary>
    ComputerMove _move = null;

    /// <summary>�����̎�����̊�ɂȂ�ʒu</summary>
    Vector3 _basePosition = default;

    // Start is called before the first frame update
    void Start()
    {
        _param = GetComponent<ComputerParameter>();
        _move = GetComponent<ComputerMove>();

        _basePosition = transform.position;

        WanderingFromCurrentPos();
    }

    // Update is called once per frame
    void Update()
    {
        if (_move.IsCloseDestination || !_move.IsFoundDestination)
        {
            _param.State.Kind = MotionState.StateKind.Walk;
            WanderingFromBasePos();
        }
    }

    /// <summary>���ʂ������̏�őҋ@</summary>
    void IdleAndLookForward()
    {
        _move.Destination = null;
    }

    /// <summary>���͂������̏�őҋ@</summary>
    void IdleAndLookAround()
    {
        IdleAndLookForward();
    }

    /// <summary>�x�[�X�|�W�V�����t�߂����������</summary>
    void WanderingFromBasePos()
    {
        float radius = Random.Range(0f, _wanderDistance);
        float ratio = Random.value;

        _move.Destination = new Vector3(radius * ratio, 0f, radius * (1f - ratio)) + _basePosition;
    }

    /// <summary>���ݒn�t�߂����������</summary>
    void WanderingFromCurrentPos()
    {
        float radius = Random.Range(0f, _wanderDistance);
        float ratio = Random.value;

        _move.Destination = new Vector3(radius * ratio, 0f, radius * (1f - ratio)) + transform.position;
    }
}
