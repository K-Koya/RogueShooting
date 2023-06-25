using UnityEngine;

/// <summary>オブジェクトプール</summary>
/// <typeparam name="T">プールする値の種類</typeparam>
[System.Serializable]
public class ObjectPool<T>
{
    [SerializeField, Tooltip("最大アイテム数")]
    protected uint _length = 10;

    /// <summary>アイテム配列</summary>
    protected T[] _values = null;

    /// <summary>アイテム配列</summary>
    public T[] Values { get => _values; }

    /// <summary>オブジェクトプール</summary>
    /// <param name="length">アイテム数</param>
    public ObjectPool(uint? length = null)
    {
        if (length != null) _length = (uint)length;
        _values = new T[_length];
    }

    /// <summary>現在のプールにアイテム生成の余裕があれば生成</summary>
    /// <param name="instant">生成したいアイテム</param>
    /// <returns>生成したアイテム　生成できなかった場合は型Tのdefault値</returns>
    public T Create(T instant)
    {
        T t = default;
        for(int i = 0; i < _values.Length; i++)
        {
            if (_values[i] == null)
            {
                _values[i] = instant;
                t = _values[i];
                break;
            }
        }
        return t;
    }

    /// <summary>指定Indexのアイテムを排除</summary>
    /// <param name="index">削除したいアイテムのIndex</param>
    /// <returns>プールから外したアイテム</returns>
    public T Remove(uint index)
    {
        T t = _values[index];
        _values[index] = default;
        return t;
    } 
}

/// <summary>UnityのGameObjectのプール</summary>
public class GameObjectPool : ObjectPool<GameObject>
{
    /// <summary>アクティブ化するオブジェクトのindex</summary>
    uint _revolveCount = 0;

    /// <summary>UnityのGameObjectのプール</summary>
    /// <param name="pref">プールするオブジェクトのプレハブ</param>
    /// <param name="parent">親オブジェト</param>
    /// <param name="length">オブジェクト数</param>
    public GameObjectPool(GameObject pref, Transform parent = null, uint? length = null)
    {
        if (length != null) _length = (uint)length;
        _values = new GameObject[_length];

        for (int i = 0; i < _values.Length; i++)
        {
            _values[i] = UnityEngine.Object.Instantiate(pref);
            _values[i].transform.parent = parent;
            _values[i].SetActive(false);
        }
    }

    /// <summary>UnityのGameObjectのプール</summary>
    /// <param name="pref">プールするオブジェクトのプレハブパス</param>
    /// <param name="parent">親オブジェト</param>
    /// <param name="length">オブジェクト数</param>
    public GameObjectPool(string prefPath, Transform parent = null, uint? length = null)
    {
        if (length != null) _length = (uint)length;
        _values = new GameObject[_length];

        for(int i = 0; i < _values.Length; i++)
        {
            _values[i] = UnityEngine.Object.Instantiate((GameObject)Resources.Load(prefPath));
            _values[i].transform.parent = parent;
            _values[i].SetActive(false);
        }
    }

    /// <summary>オブジェクトプールしたGameObjectを全てDestroyして解放する</summary>
    public void PoolDelete()
    {
        for (int i = 0; i < _values.Length; i++)
        {
            UnityEngine.Object.Destroy(_values[i]);
            _values[i] = null;
        }
    }

    /// <summary>プールからオブジェクトをアクティブにする</summary>
    /// <returns>アクティブにできた場合はそのオブジェクトを返す</returns>
    public GameObject Instansiate()
    {
        GameObject obj = null;

        uint retry = 0;
        while(retry < _length)
        {
            _revolveCount++;
            if (_revolveCount > _length - 1) _revolveCount = 0;

            if (_values[_revolveCount] is not null && !_values[_revolveCount].activeSelf)
            {
                _values[_revolveCount].SetActive(true);
                obj = _values[_revolveCount];
                break;
            }

            retry++;
        }

        return obj;
    }

    /// <summary>オブジェクトを非アクティブにする</summary>
    /// <param name="obj">非アクティブにしたいオブジェクト</param>
    public void Destroy(GameObject obj)
    {
        obj.SetActive(false);
    }
}
