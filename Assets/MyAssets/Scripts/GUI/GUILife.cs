using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUILife : MonoBehaviour
{
    /// <summary>体力表示用画像</summary>
    Image _currentLife = null;

    /// <summary>プレイヤーキャラクターのステータス値</summary>
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
        //ポーズ時は止める
        if (GameManager.IsPose)
        {
            return;
        }

        _currentLife.fillAmount = _param.LifeRatio;
    }
}
