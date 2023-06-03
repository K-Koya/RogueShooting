using UnityEngine;
using System;
using System.Linq;
using static UnityEditor.Experimental.GraphView.GraphView;

/// <summary>�I�u�W�F�N�g�v�[��</summary>
/// <typeparam name="T">�v�[������l�̎��</typeparam>
[System.Serializable]
public class ObjectPool<T>
{
    [SerializeField, Tooltip("�ő�A�C�e����")]
    protected uint _length = 10;

    /// <summary>�A�C�e���z��</summary>
    protected T[] _values = null;

    /// <summary>�A�C�e���z��</summary>
    public T[] Values { get => _values; }

    /// <summary>�I�u�W�F�N�g�v�[��</summary>
    /// <param name="length">�A�C�e����</param>
    public ObjectPool(uint? length = null)
    {
        if (length != null) _length = (uint)length;
        _values = new T[_length];
    }

    /// <summary>���݂̃v�[���ɃA�C�e�������̗]�T������ΐ���</summary>
    /// <param name="instant">�����������A�C�e��</param>
    /// <returns>���������A�C�e���@�����ł��Ȃ������ꍇ�͌^T��default�l</returns>
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

    /// <summary>�w��Index�̃A�C�e����r��</summary>
    /// <param name="index">�폜�������A�C�e����Index</param>
    /// <returns>�v�[������O�����A�C�e��</returns>
    public T Remove(uint index)
    {
        T t = _values[index];
        _values[index] = default;
        return t;
    } 
}

/// <summary>Unity��GameObject�̃v�[��</summary>
public class GameObjectPool : ObjectPool<GameObject>
{
    /// <summary>Unity��GameObject�̃v�[��</summary>
    /// <param name="pref">�v�[������I�u�W�F�N�g�̃v���n�u</param>
    /// <param name="parent">�e�I�u�W�F�g</param>
    /// <param name="length">�I�u�W�F�N�g��</param>
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

    /// <summary>Unity��GameObject�̃v�[��</summary>
    /// <param name="pref">�v�[������I�u�W�F�N�g�̃v���n�u�p�X</param>
    /// <param name="parent">�e�I�u�W�F�g</param>
    /// <param name="length">�I�u�W�F�N�g��</param>
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

    /// <summary>�I�u�W�F�N�g�v�[������GameObject��S��Destroy���ĉ������</summary>
    public void PoolDelete()
    {
        for (int i = 0; i < _values.Length; i++)
        {
            UnityEngine.Object.Destroy(_values[i]);
            _values[i] = null;
        }
    }

    /// <summary>�v�[������I�u�W�F�N�g���A�N�e�B�u�ɂ���</summary>
    /// <returns>�A�N�e�B�u�ɂł����ꍇ�͂��̃I�u�W�F�N�g��Ԃ�</returns>
    public GameObject Instansiate()
    {
        GameObject obj = null;
        foreach(GameObject val in _values)
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

    /// <summary>�I�u�W�F�N�g���A�N�e�B�u�ɂ���</summary>
    /// <param name="obj">��A�N�e�B�u�ɂ������I�u�W�F�N�g</param>
    public void Destroy(GameObject obj)
    {
        obj.SetActive(false);
    }
}
