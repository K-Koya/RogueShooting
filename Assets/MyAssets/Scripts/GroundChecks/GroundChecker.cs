using UnityEngine;

[RequireComponent(typeof(CapsuleCollider))]
public class GroundChecker : MonoBehaviour
{
    #region メンバ
    /// <summary>当該オブジェクトのカプセルコライダー</summary>
    CapsuleCollider _collider = null;

    [SerializeField, Tooltip("True : 着地している")]
    bool _isGround = false;

    [SerializeField, Tooltip("地面と壁の境界角度")]
    float _slopeLimit = 45f;

    /// <summary>キャラクターの重力向き</summary>
    Vector3 _gravityDirection = Vector3.down;

    /// <summary>SphereCastおよびCapsuleCastする時の基準点となる座標1</summary>
    Vector3 _castBasePosition = Vector3.zero;

    /// <summary>登れる坂とみなすための中心点からの距離</summary>
    float _slopeAngleThreshold = 1f;
    #endregion

    #region プロパティ
    /// <summary>True : 着地している</summary>
    public bool IsGround { get => _isGround; }

    /// <summary>キャラクターの重力向き</summary>
    public Vector3 GravityDirection { get => _gravityDirection; set => _gravityDirection = value; }
    #endregion

    void Start()
    {
        _collider = GetComponent<CapsuleCollider>();
        _castBasePosition = _collider.center + Vector3.down * ((_collider.height - _collider.radius * 2f) / 2f);

        //円弧半径から弧長を求める公式
        _slopeAngleThreshold = 2f * _collider.radius * Mathf.Sin(Mathf.Deg2Rad * _slopeLimit / 2f);
    }

    void Update()
    {
        _isGround = false;
        RaycastHit hit;
        if (Physics.SphereCast(_castBasePosition + transform.position, _collider.radius * 0.99f, _gravityDirection, out hit, _collider.radius, LayerManager.Instance.AllGround))
        {
            if (Vector3.SqrMagnitude(transform.position - hit.point) < _slopeAngleThreshold * _slopeAngleThreshold)
            {
                _isGround = true;
            }
        }
    }
}
