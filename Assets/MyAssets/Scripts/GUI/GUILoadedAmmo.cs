using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUILoadedAmmo : MonoBehaviour
{
    [SerializeField, Tooltip("�c�e����\������e�L�X�g")]
    TMP_Text _textAmmo = null;

    /// <summary>�v���C���[�L�����N�^�[�̃X�e�[�^�X�l</summary>
    IGetStatus _param = null;

    // Start is called before the first frame update
    void Start()
    {
        _param = FindObjectOfType<PlayerParameter>();
    }

    // Update is called once per frame
    void Update()
    {
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        _textAmmo.text = $"{_param.UsingGun.CurrentLoadAmmo} / {_param.UsingGun.MaxLoadAmmo}";
    }
}
