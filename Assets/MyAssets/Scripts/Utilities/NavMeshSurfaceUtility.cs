using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
[DefaultExecutionOrder(-101)]
public class NavMeshSurfaceUtility : MonoBehaviour
{
    /// <summary>ナビメッシュを動的に作るやつ</summary>
    NavMeshSurface _nav = null;

    /// <summary>true : LastUpdateナビメッシュを構成</summary>
    bool _doCreateNavMesh = false;

    // Start is called before the first frame update
    void Start()
    {
        _nav = GetComponent<NavMeshSurface>();
        _doCreateNavMesh = true;

        if (_doCreateNavMesh)
        {
            _nav.BuildNavMesh();
            _doCreateNavMesh = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
