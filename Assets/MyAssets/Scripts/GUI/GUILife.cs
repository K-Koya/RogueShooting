using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUILife : MonoBehaviour
{
    /// <summary>�̗͕\���p�摜</summary>
    Image _currentLife = null;

    /// <summary>�v���C���[�L�����N�^�[�̃X�e�[�^�X�l</summary>
    IGetStatus _param = null;

    // Start is called before the first frame update
    void Start()
    {
        _currentLife = GetComponent<Image>();
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

        _currentLife.fillAmount = _param.LifeRatio;
    }
}
