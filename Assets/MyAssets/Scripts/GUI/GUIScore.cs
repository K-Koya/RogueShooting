using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GUIScore : MonoBehaviour
{
    [SerializeField, Tooltip("倒した敵の数を表示するテキスト")]
    TMP_Text _textDefeatedEnemy = null;

    // Update is called once per frame
    void Update()
    {
        //ポーズ時は止める
        if (GameManager.IsPose)
        {
            return;
        }

        _textDefeatedEnemy.text = $"{ComputerParameter.DefeatedEnemyCount} / {ComputerParameter.DefeatedEnemyQuota}";
    }
}
