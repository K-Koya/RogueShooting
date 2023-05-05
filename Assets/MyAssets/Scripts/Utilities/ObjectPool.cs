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
    protected uint _Length = 10;

    /// <summary>�A�C�e���z��</summary>
    protected T[] _Values = null;

    /// <summary>�A�C�e���z��</summary>
    public T[] Values { get => _Values; }

    /// <summary>�I�u�W�F�N�g�v�[��</summary>
    /// <param name="length">�A�C�e����</param>
    public ObjectPool(uint? length = null)
    {
        if (length != null) _Length = (uint)length;
        _Values = new T[_Length];
    }

    /// <summary>���݂̃v�[���ɃA�C�e�������̗]�T������ΐ���</summary>
    /// <param name="instant">�����������A�C�e��</param>
    /// <returns>���������A�C�e���@�����ł��Ȃ������ꍇ�͌^T��default�l</returns>
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

    /// <summary>�w��Index�̃A�C�e����r��</summary>
    /// <param name="index">�폜�������A�C�e����Index</param>
    /// <returns>�v�[������O�����A�C�e��</returns>
    public T Remove(uint index)
    {
        T t = _Values[index];
        _Values[index] = default;
        return t;
    } 
}

/// <summary>Unity��GameObject�̃v�[��</summary>
public class GameObjectPool : ObjectPool<GameObject>
{
    /// <summary>Unity��GameObject�̃v�[��</summary>
    /// <param name="pref">�v�[������I�u�W�F�N�g�̃v���n�u</param>
    /// <param name="length">�I�u�W�F�N�g��</param>
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

    /// <summary>Unity��GameObject�̃v�[��</summary>
    /// <param name="pref">�v�[������I�u�W�F�N�g�̃v���n�u�p�X</param>
    /// <param name="length">�I�u�W�F�N�g��</param>
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

    /// <summary>�I�u�W�F�N�g�v�[������GameObject��S��Destroy���ĉ������</summary>
    public void PoolDelete()
    {
        for (int i = 0; i < _Values.Length; i++)
        {
            UnityEngine.Object.Destroy(_Values[i]);
            _Values[i] = null;
        }
    }

    /// <summary>�v�[������I�u�W�F�N�g���A�N�e�B�u�ɂ���</summary>
    /// <returns>�A�N�e�B�u�ɂł����ꍇ�͂��̃I�u�W�F�N�g��Ԃ�</returns>
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

    /// <summary>�I�u�W�F�N�g���A�N�e�B�u�ɂ���</summary>
    /// <param name="obj">��A�N�e�B�u�ɂ������I�u�W�F�N�g</param>
    public void Destroy(GameObject obj)
    {
        obj.SetActive(false);
    }
}
