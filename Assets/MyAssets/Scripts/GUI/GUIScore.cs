using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIScore : MonoBehaviour
{
    [SerializeField, Tooltip("�|�����G�̐���\������e�L�X�g")]
    TMP_Text _textDefeatedEnemy = null;

    [SerializeField, Tooltip("���o�����Ă���G�̐���\������e�L�X�g")]
    TMP_Text _textAppearEnemy = null;

    // Update is called once per frame
    void Update()
    {
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        _textDefeatedEnemy.text = $"{ComputerParameter.DefeatedEnemyCount} / {ComputerParameter.DefeatedEnemyQuota}";
        _textAppearEnemy.text = $"Appear : {CharacterParameter.Enemies.Count}";
    }
}
