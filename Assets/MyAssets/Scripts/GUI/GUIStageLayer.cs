using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUIStageLayer : MonoBehaviour
{
    [SerializeField, Tooltip("現在のステージ番号を表示するテキスト")]
    TMP_Text _textStageNumber = null;
        
    // Update is called once per frame
    void Update()
    {
        //ポーズ時は止める
        if (GameManager.IsPose)
        {
            return;
        }

        _textStageNumber.text = $"{StageManager.NumberOfCurrentStage} / {StageManager.NumberOfAllStage}";
    }
}
