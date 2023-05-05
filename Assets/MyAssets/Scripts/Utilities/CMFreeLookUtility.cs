using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CinemachineFreeLook))]
public class CMFreeLookUtility : MonoBehaviour
{
    /// <summary>当該のCinemachineFreeLook</summary>
    CinemachineFreeLook _CM = default;

    // Start is called before the first frame update
    void Start()
    {
        _CM = GetComponent<CinemachineFreeLook>();
        SeekPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        _CM.enabled = true;

        if (!_CM.Follow || !_CM.LookAt)
        {
            SeekPlayer();
        }
    }

    /// <summary>操作キャラクターの位置情報をCinemachineに反映</summary>
    void SeekPlayer()
    {
        PlayerParameter player = FindObjectOfType<PlayerParameter>();

        if(player)
        {
            _CM.Follow = player.transform;
            _CM.LookAt = player.EyePoint ? player.EyePoint.transform : player.transform;
        }
    }
}
