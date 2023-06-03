using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTreeNode;

[System.Serializable]
public class WalkWander : IExecutionMethod
{
    [SerializeField, Tooltip("������͈̔�")]
    float _wanderDistance = 20f;

    [SerializeField, Tooltip("�����̎�����̊�ɂȂ�ʒu")]
    Vector3 _basePosition = default;

    [SerializeField, Tooltip("�ړ�����W���������Ȃ����̓���L�����Z���ҋ@����")]
    float _timeOut = 1f;

    /// <summary>���Ԍv��</summary>
    float _timer = 0f;

    /// <summary>true : �����������ς�</summary>
    bool _isInitialized = false;


    public Status Method(ComputerParameter param, ComputerMove move)
    {
        if (!_isInitialized)
        {
            float radius = Random.Range(0f, _wanderDistance);
            float ratio = Random.value;

            move.Destination = new Vector3(radius * ratio, 0f, radius * (1f - ratio)) + _basePosition;
            _timer = _timeOut;
        }

        if (move.IsFoundDestination)
        {
            _isInitialized = true;
        }
        else
        {
            _timer -= Time.deltaTime;
            if(_timer < 0f)
            {
                move.Destination = null;
                return Status.Failure;
            }
        }

        if (move.IsCloseDestination)
        {
            move.Destination = null;
            _isInitialized = false;
            return Status.Success;
        }

        return Status.Running;
    }
}
