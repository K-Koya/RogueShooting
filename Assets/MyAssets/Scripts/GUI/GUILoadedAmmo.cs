using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUILoadedAmmo : MonoBehaviour
{
    [SerializeField, Tooltip("残弾数を表示するテキスト")]
    TMP_Text _textAmmo = null;

    /// <summary>プレイヤーキャラクターのステータス値</summary>
    IGetStatus _param = null;

    // Start is called before the first frame update
    void Start()
    {
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

        _textAmmo.text = $"{_param.UsingGun.CurrentLoadAmmo} / {_param.UsingGun.MaxLoadAmmo}";
    }
}
