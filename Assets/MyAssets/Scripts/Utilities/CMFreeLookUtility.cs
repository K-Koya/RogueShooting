using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CMFreeLookUtility : MonoBehaviour
{
    /// <summary>���Y��CinemachineFreeLook</summary>
    CinemachineFreeLook _cm = default;

    // Start is called before the first frame update
    void Start()
    {
        _cm = GetComponent<CinemachineFreeLook>();
        SeekPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        _cm.enabled = true;

        if (!_cm.Follow || !_cm.LookAt)
        {
            SeekPlayer();
        }
    }

    /// <summary>����L�����N�^�[�̈ʒu����Cinemachine�ɔ��f</summary>
    void SeekPlayer()
    {
        PlayerParameter player = FindObjectOfType<PlayerParameter>();

        if(player)
        {
            _cm.Follow = player.transform;
            _cm.LookAt = player.EyePoint ? player.EyePoint.transform : player.transform;
        }
    }
}
