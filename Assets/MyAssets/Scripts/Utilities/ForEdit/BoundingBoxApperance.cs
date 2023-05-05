#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBoxApperance : MonoBehaviour
{
    static readonly Color BoundsColor = new Color(1, 0, 0, 0.5f);

    /// <summary>対象レンダーラー</summary>
    Renderer _Renderer = null;

    [Header("バウンディングボックス")]
    [SerializeField, Tooltip("中心座標")]
    Vector3 _Center = Vector3.zero;

    [SerializeField, Tooltip("大きさ")]
    Vector3 _Size = Vector3.zero;

    void OnDrawGizmos()
    {
        if (_Renderer is null)
        {
            _Renderer = GetComponent<Renderer>();

            _Center = _Renderer.bounds.center;
            _Size = _Renderer.bounds.size;
        }
        else
        {
            Gizmos.color = BoundsColor;
            Gizmos.DrawCube(_Renderer.bounds.center, _Renderer.bounds.size);
        }
    }
}

#endif
