using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>画面に描画する照準器を操作する</summary>
public class AimDrawer : MonoBehaviour
{
    /// <summary>照準器の描画位置を計算するコンポーネント</summary>
    AimMovement _AimMovement = null;

    [Header("UIコンポーネント")]

    [SerializeField, Tooltip("照準画像表示用コンポーネント")]
    Image _AimImage = null;

    [SerializeField, Tooltip("距離表示用テキストコンポーネント")]
    TextMeshProUGUI _DistanceText = null;

    [Header("照準画像用スプライト")]

    [SerializeField, Tooltip("射程外の時の照準画像")]
    Sprite _AimSpriteOutOfRange = null;

    [SerializeField, Tooltip("近接攻撃範囲外の時の照準画像")]
    Sprite _AimSpriteOutOfProximity = null;

    [SerializeField, Tooltip("近接攻撃範囲内の時の照準画像")]
    Sprite _AimSpriteWithinProximity = null;

    [Header("各射程距離におけるシンボルカラー")]

    [SerializeField, Tooltip("射程外の時に使用するシンボルカラー")]
    Color _ColorOutOfRange = default;

    [SerializeField, Tooltip("近接攻撃範囲外の時に使用するシンボルカラー")]
    Color _ColorOutOfProximity = default;

    [SerializeField, Tooltip("近接攻撃範囲内の時に使用するシンボルカラー")]
    Color _ColorWithinProximity = default;


    // Start is called before the first frame update
    void Start()
    {
        _AimMovement = GetComponentInParent<AimMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        //距離実数値を表示
        _DistanceText.text = _AimMovement.Distance.ToString("F2") + "m";

        //距離の識別値に応じて、距離実数値のテキストカラーの設定および照準スプライトと色を指定
        switch (_AimMovement.DistType)
        {
            case DistanceType.OutOfRange:
                {
                    _DistanceText.color = _ColorOutOfRange;
                    _AimImage.sprite = _AimSpriteOutOfRange;
                    _AimImage.color = _ColorOutOfRange;

                    break;
                }
            case DistanceType.OutOfProximity:
                {
                    _DistanceText.color = _ColorOutOfProximity;
                    _AimImage.sprite = _AimSpriteOutOfProximity;
                    _AimImage.color = _ColorOutOfProximity;

                    break;
                }
            case DistanceType.WithinProximity:
                {
                    _DistanceText.color = _ColorWithinProximity;
                    _AimImage.sprite = _AimSpriteWithinProximity;
                    _AimImage.color = _ColorWithinProximity;

                    break;
                }
        }
    }
}
