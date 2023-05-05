using UnityEngine;

/// <summary>
/// �V���O���g����������R���|�[�l���g�̊��N���X
/// </summary>
/// <typeparam name="T">MonoBehaviour���p������iInspector��ɏo�������j�R���|�[�l���g</typeparam>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    [Header("�V���O���g���I�u�W�F�N�g�ł���")]
    [SerializeField, Tooltip("ture : DontDestroyOnLoad�̑Ώۂɂ���")]
    bool _isDontDestroyOnLoad = false;

    [Space]

    /// <summary>
    /// Inspector��ɏo�Ă���V���O���g���̃R���|�[�l���g�C���X�^���X
    /// </summary>
    static T _Instance;


    /* �v���p�e�B */
    /// <summary>
    /// Inspector��ɏo�Ă���V���O���g���̃R���|�[�l���g�̃C���X�^���X
    /// </summary>
    public static T Instance
    {
        get
        {
            //�Ώۂ̃V���O���g���R���|�[�l���g���o�^����ĂȂ���΁A���݂̃V�[������E���Ă���
            if (!_Instance)
            {
                _Instance = (T)FindObjectOfType(typeof(T));
                if (!_Instance) Debug.LogError("�V���O���g���R���|�[�l���g�� " + typeof(T) + " ���A���݂̃V�[���ɑ��݂��܂���I");
            }
            return _Instance;
        }
    }

    /// <summary>
    /// ture : DontDestroyOnLoad�̑Ώۂɂ���
    /// </summary>
    public bool IsDontDestroyOnLoad { get => _isDontDestroyOnLoad; set => _isDontDestroyOnLoad = value; }


    protected virtual void Awake()
    {
        //DontDestroyOnLoad�ɓo�^���Ȃ��R���|�[�l���g�Ȃ痣�E
        if (!_isDontDestroyOnLoad) return;

        //�o�^����Ă���V���O���g���R���|�[�l���g�������̃C���X�^���X�Ɠ����Ȃ�ADontDestroyOnLoad�ɓo�^����
        //�قȂ�΁A������j������
        if (this != Instance) Destroy(this.gameObject);
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        //DontDestroyOnLoad�ɓo�^����R���|�[�l���g�Ȃ痣�E
        if (_isDontDestroyOnLoad) return;

        //���̃C���X�^���X��static���珜��
        _Instance = null;
    }
}
