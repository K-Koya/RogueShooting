using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastCheck : MonoBehaviour
{
    [SerializeField]
    LayerMask mask = default;

    [SerializeField]
    float length = 100f;

    Vector3 point = Vector3.forward;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ポーズ時は止める
        if (GameManager.IsPose)
        {
            return;
        }

        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, length, mask))
        {
            point = hit.point;
        }
        else
        {
            point = transform.position + transform.forward * length;
        }
    }

#if UNITY_EDITOR
    /// <summary>照準先を示すレイキャスト</summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, point);
    }
#endif
}
