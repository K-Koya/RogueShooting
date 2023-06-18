using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnerPortal : MonoBehaviour
{
    /// <summary>�����ɏo��������G�̐�</summary>
    static byte _Capacity = 15;



    [SerializeField, Tooltip("�o���Ԋu")]
    float _interval = 2f;

    /// <summary>�������œG���o��������R���[�`��</summary>
    Coroutine _spawnCoroutine = null;

    /// <summary>�o����҂R���[�`���p�p�����[�^</summary>
    WaitForSeconds _waitCoroutine = null;

    [SerializeField, Tooltip("�G�o�����a")]
    float _spawnRadius = 5f;

    [SerializeField, Tooltip("�G�ސw���a")]
    float _despawnRadius = 20f;

    /// <summary>�������ŋ����̗��ꂽ�G�������R���[�`��</summary>
    Coroutine _despawnCoroutine = null;



    [SerializeField, Tooltip("�o������G�W")]
    GameObject[] _enemyPrefs = null;


    /// <summary>�G��񏊎�</summary>
    GameObject[] _comObjects = null;

    /// <summary>�v���C���[���</summary>
    PlayerParameter _player = null;


    /// <summary>�����ɏo��������G�̐�</summary>
    public static byte Capacity { get => _Capacity; set => _Capacity = value; }


    // Start is called before the first frame update
    void Start()
    {
        _waitCoroutine = new WaitForSeconds(_interval);
        _comObjects = new GameObject[_Capacity];
        _player = FindObjectOfType<PlayerParameter>();

        _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        _despawnCoroutine = StartCoroutine(DespawnCoroutine());
    }

    /// <summary>���Ԋu�œG���o�������Ă����R���[�`��</summary>
    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            //�o�������ő�ɒB���Ă��Ȃ���Βǉ�
            for(int i = 0; i < _comObjects.Length; i++)
            {
                if (_comObjects[i] is null)
                {
                    //�o��������n�ʂ��w��
                    float angle = Random.Range(0f, Mathf.PI);
                    Vector3 pos = _player.transform.position + new Vector3(Mathf.Cos(angle) * _spawnRadius, 20f, Mathf.Sin(angle) * _spawnRadius);
                    RaycastHit hit;
                    if (Physics.Raycast(pos, Vector3.down, out hit, 30f, LayerManager.Instance.Ground))
                    {
                        if (hit.collider.CompareTag(TagManager.Instance.Floor))
                        {
                            pos = hit.point;

                            yield return null;

                            //�o�������Ĉʒu�𒲐�
                            GameObject ins = Instantiate(_enemyPrefs[Random.Range(0, _enemyPrefs.Length)]);
                            ins.transform.position = pos;
                            _comObjects[i] = ins;
                        }
                    }

                    break;
                }
            }

            yield return _waitCoroutine;
        }
    }

    /// <summary>���Ԋu�ŗ��ꂽ�G���폜����R���[�`��</summary>
    IEnumerator DespawnCoroutine()
    {
        for (int i = _comObjects.Length - 1; ; i--)
        {
            if (i < 1)
            {
                i = _comObjects.Length - 1;
            }

            try
            {
                if (_comObjects[i] is not null)
                {
                    if (Vector3.SqrMagnitude(_player.transform.position - _comObjects[i].transform.position) > _despawnRadius * _despawnRadius)
                    {
                        Destroy(_comObjects[i]);
                        _comObjects[i] = null;
                    }
                }
            }
            catch (MissingReferenceException)
            {
                _comObjects[i] = null;
            }

            yield return null;
        }
    }
}
