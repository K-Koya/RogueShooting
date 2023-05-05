using UnityEngine;
using System;
using System.Linq;
using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>オブジェクトプール</summary>
/// <typeparam name="T">プールする値の種類</typeparam>
[System.Serializable]
public class ObjectPool<T>
{
    [SerializeField, Tooltip("最大アイテム数")]
    protected uint _Length = 10;

    /// <summary>アイテム配列</summary>
    protected T[] _Values = null;

    /// <summary>アイテム配列</summary>
    public T[] Values { get => _Values; }

    /// <summary>オブジェクトプール</summary>
    /// <param name="length">アイテム数</param>
    public ObjectPool(uint? length = null)
    {
        if (length != null) _Length = (uint)length;
        _Values = new T[_Length];
    }

    /// <summary>現在のプールにアイテム生成の余裕があれば生成</summary>
    /// <param name="instant">生成したいアイテム</param>
    /// <returns>生成したアイテム　生成できなかった場合は型Tのdefault値</returns>
    public T Create(T instant)
    {
        T t = default;
        for(int i = 0; i < _Values.Length; i++)
        {
            if (_Values[i] == null)
            {
                _Values[i] = instant;
                t = _Values[i];
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
        T t = _Values[index];
        _Values[index] = default;
        return t;
    } 
}

/// <summary>UnityのGameObjectのプール</summary>
public class GameObjectPool : ObjectPool<GameObject>
{
    /// <summary>UnityのGameObjectのプール</summary>
    /// <param name="pref">プールするオブジェクトのプレハブ</param>
    /// <param name="length">オブジェクト数</param>
    public GameObjectPool(GameObject pref, uint? length = null)
    {
        if (length != null) _Length = (uint)length;
        _Values = new GameObject[_Length];

        for (int i = 0; i < _Values.Length; i++)
        {
            _Values[i] = UnityEngine.Object.Instantiate(pref);
            _Values[i].SetActive(false);
        }
    }

    /// <summary>UnityのGameObjectのプール</summary>
    /// <param name="pref">プールするオブジェクトのプレハブパス</param>
    /// <param name="length">オブジェクト数</param>
    public GameObjectPool(string prefPath, uint? length = null)
    {
        if (length != null) _Length = (uint)length;
        _Values = new GameObject[_Length];

        for(int i = 0; i < _Values.Length; i++)
        {
            _Values[i] = UnityEngine.Object.Instantiate((GameObject)Resources.Load(prefPath));
            _Values[i].SetActive(false);
        }
    }

    /// <summary>オブジェクトプールしたGameObjectを全てDestroyして解放する</summary>
    public void PoolDelete()
    {
        for (int i = 0; i < _Values.Length; i++)
        {
            UnityEngine.Object.Destroy(_Values[i]);
            _Values[i] = null;
        }
    }

    /// <summary>プールからオブジェクトをアクティブにする</summary>
    /// <returns>アクティブにできた場合はそのオブジェクトを返す</returns>
    public GameObject Instansiate()
    {
        GameObject obj = null;
        foreach(GameObject val in _Values)
        {
            if (val is not null && !val.activeSelf) 
            {
                val.SetActive(true);
                obj = val;
                break;
            }
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
