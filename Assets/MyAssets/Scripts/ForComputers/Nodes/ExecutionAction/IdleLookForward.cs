using BehaviorTreeNode;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IdleLookForward : IExecutionMethod
{
    [SerializeField, Tooltip("�ҋ@�Œ�����")]
    float _maxTime = 10f;

    [SerializeField, Tooltip("�ҋ@�ŒZ����")]
    float _minTime = 2f;

    /// <summary>���Ԍv��</summary>
    float _timer = 0f;

    /// <summary>true : �����������ς�</summary>
    bool _isInitialized = false;

    public Status Method(ComputerParameter param, ComputerMove move)
    {
        if (!_isInitialized)
        {
            move.Destination = null;
            if (_minTime > _maxTime) _minTime = _maxTime;
            _timer = Random.Range(_minTime, _maxTime);
            param.State.Kind = MotionState.StateKind.Stay;
            _isInitialized = true;
        }

        if(_timer < 0)
        {
            _isInitialized = false;
            return Status.Success;
        }
        else
        {
            _timer -= Time.deltaTime;
        }

        return Status.Running;
    }
}
