using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEventTelop : MonoBehaviour
{
    [SerializeField, Tooltip("�Ăяo�������e���b�v���A�T�C��")]
    GameObject _callTelop = null;


    private void OnTriggerEnter(Collider other)
    {
        PlayerParameter param;
        if (other.TryGetComponent(out param))
        {
            GameManager.Instance.PauseMode(true);
            GameManager.Instance.CursorMode(true);
            _callTelop.SetActive(true);
        }
    }
}
