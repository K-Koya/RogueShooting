using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerPortal : MonoBehaviour
{
    /// <summary>同時に出現させる敵の数</summary>
    static byte _Capacity = 15;


    /// <summary>対象マップ情報</summary>
    IGetPlantMapData _mapData = null;

    [SerializeField, Tooltip("出現間隔")]
    float _interval = 2f;

    [SerializeField, Tooltip("敵有効化半径")]
    float _activeRadius = 20f;

    [SerializeField, Tooltip("敵無効化半径")]
    float _disableRadius = 25f;



    /// <summary>一定周期で敵を出現させるコルーチン</summary>
    Coroutine _spawnCoroutine = null;

    /// <summary>一定周期で敵を位置関係で有効・無効を検討するコルーチン</summary>
    Coroutine _activateSelectCoroutine = null;

    /// <summary>出現を待つコルーチン用待ち時間</summary>
    WaitForSeconds _waitSpawnCoroutine = null;

    /// <summary>敵の有効・無効を検討するコルーチン用待ち時間</summary>
    WaitForSeconds _waitActivateSelectCoroutine = null;



    [SerializeField, Tooltip("出現する敵集")]
    GameObject[] _enemyPrefs = null;

    /// <summary>プレイヤー情報</summary>
    PlayerParameter _player = null;


    /// <summary>同時に出現させる敵の数</summary>
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

    /// <summary>一定間隔で敵を出現させていくコルーチン</summary>
    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            if (GameManager.IsPose)
            {
                yield return null;
                continue;
            }

            //出現数が最大に達していなければ追加
            if(CharacterParameter.Enemies.Count < _Capacity + 1)
            {
                //出現させる地面を指定、ただし、プレイヤー付近は回避
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

                            //出現させて位置を調整
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

    /// <summary>一定周期で敵を位置関係で有効・無効を検討するコルーチン</summary>
    IEnumerator ActivateSelectCoroutine()
    {
        while (true)
        {
            if (GameManager.IsPose)
            {
                yield return null;
                continue;
            }

            //敵の有効化・無効化
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
