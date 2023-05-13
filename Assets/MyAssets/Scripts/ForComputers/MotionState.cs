using UnityEngine;

/// <summary>�{�L�����N�^�[���ǂ̂悤�ȓ�������Ă��邩���܂Ƃ߂�N���X</summary>
[System.Serializable]
public class MotionState
{
    [SerializeField, Tooltip("�s���̎��")]
    StateKind _kind = StateKind.Stay;

    [SerializeField, Tooltip("���̍s���̒i�K")]
    ProcessKind _process = ProcessKind.NotPlaying;

    /// <summary>�s���̎��</summary>
    public StateKind Kind { get => _kind; set => _kind = value; }
    /// <summary>���̍s���̒i�K</summary>
    public ProcessKind Process { get => _process; set => _process = value; }

    /// <summary>�s���̎�ނ������񋓑�</summary>
    public enum StateKind : byte
    {
        /// <summary>�ҋ@���̏��</summary>
        Stay = 0,
        /// <summary>���s���̏��</summary>
        Walk,
        /// <summary>���s���̏��</summary>
        Run,
        /// <summary>�ʏ�W�����v���̏��</summary>
        JumpNoraml,
        /// <summary>�ʏ헎�����̏��</summary>
        FallNoraml,
        /// <summary>�ˌ� : ������</summary>
        FireHipUse,
        /// <summary>�ˌ� : �X�R�[�v�`������</summary>
        FireLookInto,
        /// <summary>�_���[�W���󂯂��肵�ĂЂ��ł�����</summary>
        Hurt,
        /// <summary>�|���ꂽ���</summary>
        Defeat,
    }

    /// <summary>�e�s���ɂ��āA���̓���̂ǂ̋ǖʂ��������񋓑�</summary>
    public enum ProcessKind : byte
    {
        /// <summary>�����{</summary>
        NotPlaying = 0,
        /// <summary>�{����O�̗\�����쒆(���̑���͕s�\)</summary>
        Preparation,
        /// <summary>�{���쒆</summary>
        Playing,
        /// <summary>�{���썇�Ԃ̋󂫎���(���̑���͈ꕔ�󂯕t����)</summary>
        Interval,
        /// <summary>�{����I�����O</summary>
        EndSoon
    }
}
