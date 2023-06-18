using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEventTelop : MonoBehaviour
{
    [SerializeField, Tooltip("�Ăяo�������e���b�v���A�T�C��")]
    GameObject _callTelop = null;


    private void OnTriggerEnter(Collider other)
    {
        GameManager.PauseMode(true);
        GameManager.CursorMode(true);
        _callTelop.SetActive(true);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameManager.PauseMode(true);
        GameManager.CursorMode(true);
        _callTelop.SetActive(true);
    }
}
