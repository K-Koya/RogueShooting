using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class NavMeshSurfaceUtility : MonoBehaviour
{
    /// <summary>�i�r���b�V���𓮓I�ɍ����</summary>
    NavMeshSurface _nav = null;

    /// <summary>true : LastUpdate�i�r���b�V�����\��</summary>
    bool _doCreateNavMesh = false;

    void Awake()
    {
        _nav = GetComponent<NavMeshSurface>();
        _doCreateNavMesh = true;

        if (_doCreateNavMesh)
        {
            _nav.BuildNavMesh();
            _doCreateNavMesh = false;
        }
    }
}
