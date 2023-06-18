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
        //�|�[�Y���͎~�߂�
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
    /// <summary>�Ə�����������C�L���X�g</summary>
    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position, point);
    }
#endif
}
