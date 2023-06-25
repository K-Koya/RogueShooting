using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIScore : MonoBehaviour
{
    [SerializeField, Tooltip("�|�����G�̐���\������e�L�X�g")]
    TMP_Text _textDefeatedEnemy = null;

    // Update is called once per frame
    void Update()
    {
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        _textDefeatedEnemy.text = $"{ComputerParameter.DefeatedEnemyCount} / {ComputerParameter.DefeatedEnemyQuota}";
    }
}
