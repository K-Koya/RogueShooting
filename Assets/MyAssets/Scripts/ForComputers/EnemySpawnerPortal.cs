using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnerPortal : MonoBehaviour
{
    /// <summary>同時に出現させる敵の数</summary>
    static byte _Capacity = 15;



    [SerializeField, Tooltip("出現間隔")]
    float _interval = 2f;

    /// <summary>一定周期で敵を出現させるコルーチン</summary>
    Coroutine _spawnCoroutine = null;

    /// <summary>出現を待つコルーチン用パラメータ</summary>
    WaitForSeconds _waitCoroutine = null;

    [SerializeField, Tooltip("敵出現半径")]
    float _spawnRadius = 5f;

    [SerializeField, Tooltip("敵退陣半径")]
    float _despawnRadius = 20f;

    /// <summary>一定周期で距離の離れた敵を消すコルーチン</summary>
    Coroutine _despawnCoroutine = null;



    [SerializeField, Tooltip("出現する敵集")]
    GameObject[] _enemyPrefs = null;


    /// <summary>敵情報所持</summary>
    GameObject[] _comObjects = null;

    /// <summary>プレイヤー情報</summary>
    PlayerParameter _player = null;


    /// <summary>同時に出現させる敵の数</summary>
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

    /// <summary>一定間隔で敵を出現させていくコルーチン</summary>
    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            //出現数が最大に達していなければ追加
            for(int i = 0; i < _comObjects.Length; i++)
            {
                if (_comObjects[i] is null)
                {
                    //出現させる地面を指定
                    float angle = Random.Range(0f, Mathf.PI);
                    Vector3 pos = _player.transform.position + new Vector3(Mathf.Cos(angle) * _spawnRadius, 20f, Mathf.Sin(angle) * _spawnRadius);
                    RaycastHit hit;
                    if (Physics.Raycast(pos, Vector3.down, out hit, 30f, LayerManager.Instance.Ground))
                    {
                        if (hit.collider.CompareTag(TagManager.Instance.Floor))
                        {
                            pos = hit.point;

                            yield return null;

                            //出現させて位置を調整
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

    /// <summary>一定間隔で離れた敵を削除するコルーチン</summary>
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
