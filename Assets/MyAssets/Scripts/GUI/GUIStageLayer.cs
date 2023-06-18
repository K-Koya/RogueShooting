using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIStageLayer : MonoBehaviour
{
    [SerializeField, Tooltip("���݂̃X�e�[�W�ԍ���\������e�L�X�g")]
    TMP_Text _textStageNumber = null;
        
    // Update is called once per frame
    void Update()
    {
        //�|�[�Y���͎~�߂�
        if (GameManager.IsPose)
        {
            return;
        }

        _textStageNumber.text = $"{StageManager.NumberOfCurrentStage} / {StageManager.NumberOfAllStage}";
    }
}
