using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPortal : MonoBehaviour
{
    /// <summary>�����ɏo��������G�̐�</summary>
    static byte _Capacity = 15;


    /// <summary>�Ώۃ}�b�v���</summary>
    IGetPlantMapData _mapData = null;

    [SerializeField, Tooltip("�o���Ԋu")]
    float _interval = 2f;

    [SerializeField, Tooltip("�G�L�������a")]
    float _activeRadius = 20f;

    [SerializeField, Tooltip("�G���������a")]
    float _disableRadius = 25f;



    /// <summary>�������œG���o��������R���[�`��</summary>
    Coroutine _spawnCoroutine = null;

    /// <summary>�������œG���ʒu�֌W�ŗL���E��������������R���[�`��</summary>
    Coroutine _activateSelectCoroutine = null;

    /// <summary>�o����҂R���[�`���p�҂�����</summary>
    WaitForSeconds _waitSpawnCoroutine = null;

    /// <summary>�G�̗L���E��������������R���[�`���p�҂�����</summary>
    WaitForSeconds _waitActivateSelectCoroutine = null;



    [SerializeField, Tooltip("�o������G�W")]
    GameObject[] _enemyPrefs = null;

    /// <summary>�v���C���[���</summary>
    PlayerParameter _player = null;


    /// <summary>�����ɏo��������G�̐�</summary>
    public static byte Capacity { get => _Capacity; set => _Capacity = value; }


    // Start is called before the first frame update
    void Start()
    {
        _waitSpawnCoroutine = new WaitForSeconds(_interval);
        _waitActivateSelectCoroutine = new WaitForSeconds(_interval * 2f);

        _player = FindObjectOfType<PlayerParameter>();
        _mapData = FindObjectOfType<MapRandomizer_Plant>();

        _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        //TODO
        //_activateSelectCoroutine = StartCoroutine(ActivateSelectCoroutine());
    }

    void Update()
    {
        if (GameManager.IsPose)
        {
            return;
        }
    }

    /// <summary>���Ԋu�œG���o�������Ă����R���[�`��</summary>
    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            if (GameManager.IsPose)
            {
                yield return null;
                continue;
            }

            //�o�������ő�ɒB���Ă��Ȃ���Βǉ�
            if(CharacterParameter.Enemies.Count < _Capacity + 1)
            {
                //�o��������n�ʂ��w��A�������A�v���C���[�t�߂͉��
                Vector3 pos = new Vector3(Random.Range(_mapData.GameAreaMin.x, _mapData.GameAreaMax.x), 0f, Random.Range(_mapData.GameAreaMin.y, _mapData.GameAreaMax.y));
                if(Vector3.SqrMagnitude(_player.transform.position - pos) > _activeRadius * _activeRadius)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(pos + Vector3.up * 20f, Vector3.down, out hit, 30f, LayerManager.Instance.Ground))
                    {
                        if (hit.collider.CompareTag(TagManager.Instance.Floor))
                        {
                            pos = hit.point;

                            yield return null;

                            //�o�������Ĉʒu�𒲐�
                            GameObject ins = Instantiate(_enemyPrefs[Random.Range(0, _enemyPrefs.Length)]);
                            ins.transform.position = pos;
                        }

                        yield return _waitSpawnCoroutine;
                    }
                }
            }

            yield return null;
        }
    }

    /// <summary>�������œG���ʒu�֌W�ŗL���E��������������R���[�`��</summary>
    IEnumerator ActivateSelectCoroutine()
    {
        while (true)
        {
            if (GameManager.IsPose)
            {
                yield return null;
                continue;
            }

            //�G�̗L�����E������
            foreach (CharacterParameter param in CharacterParameter.Enemies)
            {
                float playerSqrDistance = Vector3.SqrMagnitude(_player.transform.position - param.transform.position);
                if (playerSqrDistance < _activeRadius * _activeRadius)
                {
                    param.gameObject.SetActive(true);
                }
                else if (playerSqrDistance > _disableRadius * _disableRadius)
                {
                    param.gameObject.SetActive(false);
                }
            }

            yield return _waitSpawnCoroutine;
        }
    }
}
