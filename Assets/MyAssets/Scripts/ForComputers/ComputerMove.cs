using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ComputerMove : CharacterMove
{
    /// <summary>�Y���L�����N�^�[��NavMeshAgent</summary>
    NavMeshAgent _nav = null;

    /// <summary>�w�肷��ړ�����W</summary>
    Vector3? _destination = null;

    /// <summary>�������Ă��Ă��ǂ蒅����ړ�����W</summary>
    Vector3? _resultDestination = null;

    /// <summary>�͂�������␳�l</summary>
    Vector3 _forceCorrection = Vector3.zero;

    /// <summary>�ړ�����߂�R���[�`��</summary>
    Coroutine _setDestinationCoroutine = null;


    #region �i�r���b�V���p�����o
    [SerializeField, Tooltip("�ړI�n�ɐڋ߂����Ƃ݂Ȃ�����")]
    float _closeDistance = 3f;

    [SerializeField, Tooltip("true : �ړ����NavMesh��Ɍ�����ꂽ")]
    bool _isFoundDestination = false;

    [SerializeField, Tooltip("true : �ړ�����W�ɐڋ߂���")]
    bool _isCloseDestination = true;


    #endregion

    #region �v���p�e�B
    /// <summary>�w�肷��ړ�����W</summary>
    public Vector3? Destination { set => _destination = value; }

    /// <summary>�������Ă��Ă��ǂ蒅����ړ�����W</summary>
    public Vector3? ResultDestination { get => _resultDestination; }

    /// <summary>�͂�������␳�l</summary>
    public Vector3 ForceCorrection { set => _forceCorrection = value; }

    /// <summary>�i�r���b�V����ɂ�����ړ�����W</summary>
    public Vector3 DestinationOnNavMesh { get => _nav.destination; }

    /// <summary>true : �ړ����NavMesh��Ɍ�����ꂽ</summary>
    public bool IsFoundDestination { get => _isFoundDestination; }

    /// <summary>true : �ړ�����W�ɐڋ߂���</summary>
    public bool IsCloseDestination { get => _isCloseDestination; }

    #endregion

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        _nav = GetComponent<NavMeshAgent>();
        _nav.enabled = false;
        _rb.useGravity = false;
    }

    // Update is called once per frame
    protected override void Update()
    {
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        base.Update();

        //�i�r���b�V���G�[�W�F���g���A�킸���Ɏ��Ԃ������Ă���L����
        if (!_nav.enabled)
        {
            _nav.enabled = true;
            _nav.isStopped = true;

            _destination = null;
            _resultDestination = null;
        }

        if (_param.State.Kind is MotionState.StateKind.Defeat)
        {
            _isCloseDestination = true;
            _setDestinationCoroutine = null;
            _nav.ResetPath();
            _destination = null;
            _resultDestination = null;

            return;
        }

        MoveByNavMesh();
        SetResultDestination();
    }

    /// <summary>�i�r���b�V���𗘗p�����ړ����\�b�h</summary>
    void MoveByNavMesh()
    {
        _isCloseDestination = false;
        if (_destination == null)
        {
            _setDestinationCoroutine = null;
        }
        //�ړI�n�w�肪����΃R���[�`�������s
        else
        {
            //�ړI�n�ɂ����Ȃ�R���[�`���͎~�߂�
            if (_resultDestination is not null && Vector3.SqrMagnitude((Vector3)_resultDestination - transform.position) < _closeDistance * _closeDistance)
            {
                _isCloseDestination = true;
                _setDestinationCoroutine = null;
                _nav.ResetPath();
                _destination = null;
                _resultDestination = null;
            }
            //�R���[�`�������s�ł���Ύ��s
            else if (_setDestinationCoroutine == null)
            {
                _setDestinationCoroutine = StartCoroutine(DestinationSetOnAgent());
            }
        }

        //�o�H�p�X�ꗗ���A�ɂ߂ċ߂����łȂ��A���߂̈ʒu���擾����
        Vector3 currentNextPassing = transform.position;
        foreach (Vector3 passing in _nav.path.corners)
        {
            if (Vector3.SqrMagnitude(passing - transform.position) > 0.01f)
            {
                currentNextPassing = passing;
                break;
            }
        }

        //�ړ�����W���w�肵�Ă���΁A���߂̒ʉ߃|�C���g�Ɍ����ė͂�������
        if ((_param.Can & MotionEnableFlag.Walk) == MotionEnableFlag.Walk && _destination == null)
        {
            _param.MoveDirection = Vector3.zero;
        }
        else
        {
            _param.MoveDirection = Vector3.Normalize(currentNextPassing - transform.position);
        }

        //�ړ��͕␳�����Z
        _param.MoveDirection += _forceCorrection;

        //���͂�����Έړ��͂̏���
        if (_param.MoveDirection.sqrMagnitude > 0f)
        {
            //�ړ����͂̑傫�����擾
            _moveInputRate = _param.MoveDirection.magnitude;
            //�ړ��������擾
            _param.MoveDirection *= 1f / _moveInputRate;
            //�ړ��͂��v�Z
            _movePower = 5f;
        }
        else
        {
            _moveInputRate = 0f;
            _param.MoveDirection = Vector3.zero;
        }

        //�d�͕����ȊO�ňړ��ʐ������������ꍇ�A�u���[�L�ʂ��v�Z����
        bool isMoving = Vector3.SqrMagnitude(VelocityOnPlane) > 0.01f;
        if (isMoving)
        {
            _ForceOfBrake = -VelocityOnPlane.normalized * 0.2f;
        }
    }

    /// <summary>NavMeshAgent��Destination�Ɉ��Ԋu�ŖړI�n���w������R���[�`��</summary>
    IEnumerator DestinationSetOnAgent()
    {
        while (true)
        {
            _isFoundDestination = false;

            if (_destination == null)
            {
                yield return null;
            }
            else
            {
                RaycastHit hit;
                if (Physics.Raycast((Vector3)_destination + Vector3.up * 0.2f, Vector3.down, out hit, 10f, LayerManager.Instance.AllGround))
                {
                    _nav.destination = hit.point;
                    _isFoundDestination = true;
                }

                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    /// <summary>���ǂ蒅����ړ�����W���w��</summary>
    void SetResultDestination()
    {
        if (_nav.path.corners.Length > 1)
        {
            _resultDestination = _nav.path.corners.Last();
        }
        else
        {
            _resultDestination = null;
        }
    }

#if UNITY_EDITOR
    /// <summary>�i�r���b�V���ɂ��ړ��o�H�������o��</summary>
    void OnDrawGizmos()
    {
        if (_nav && _nav.enabled)
        {
            Gizmos.color = Color.red;
            var prefPos = transform.position;

            foreach (var pos in _nav.path.corners)
            {
                Gizmos.DrawLine(prefPos, pos);
                prefPos = pos;
            }
        }
    }
#endif

}
