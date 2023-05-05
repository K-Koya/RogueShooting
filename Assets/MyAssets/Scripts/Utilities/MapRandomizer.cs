using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapRandomizer : MonoBehaviour
{
    [SerializeField, Tooltip("マスの数(一辺5マス以上)")]
    Vector2Int _MapSize = new Vector2Int(5, 5);

    [SerializeField, Tooltip("マス目1辺の長さ")]
    float _Unit = 10f;

    [SerializeField, Tooltip("true : 乱数シードを指定する")]
    bool _UseSeed = false;

    [SerializeField, Tooltip("乱数シード")]
    int _Seed = 10;



    [Header("地面プレハブ")]
    [SerializeField, Tooltip("普通地面区画")]
    GameObject _CommonFloor = null;

    [SerializeField, Tooltip("直進道路区画")]
    GameObject _StraightRoad = null;

    [SerializeField, Tooltip("急な登坂区画")]
    GameObject _SteepSlopeRoad = null;

    [SerializeField, Tooltip("緩い登坂区画")]
    GameObject _GentleSlopeRoad = null;

    [SerializeField, Tooltip("曲線道路区画")]
    GameObject _CurveRoad = null;

    [Header("壁プレハブ")]
    [SerializeField, Tooltip("高低差埋めの壁 普通")]
    GameObject _DifferentHeightWall = null;

    [SerializeField, Tooltip("高低差埋めの壁 両面あり")]
    GameObject _BothSideWall = null;

    [SerializeField, Tooltip("壁の角を埋める柱")]
    GameObject _WallCornerPost = null;

    [Header("建物プレハブ")]
    [SerializeField, Tooltip("4×3の土地を使う建物")]
    GameObject[] _Buldings4x3 = null;

    [SerializeField, Tooltip("3×3の土地を使う建物")]
    GameObject[] _Buldings3x3 = null;

    [SerializeField, Tooltip("3×2の土地を使う建物")]
    GameObject[] _Buldings3x2 = null;

    [SerializeField, Tooltip("2×2の土地を使う建物")]
    GameObject[] _Buldings2x2 = null;

    [SerializeField, Tooltip("2×1の土地を使う建物")]
    GameObject[] _Buldings2x1 = null;

    [SerializeField, Tooltip("1つの土地を使う建物")]
    GameObject[] _Buldings1x1 = null;




    /// <summary>マップテーブル</summary>
    MapCell[,] map = null;


    /// <summary>方角</summary>
    public enum Compass
    {
        North = 1,
        East = 2,
        South = 3,
        West = 4,
    }

    /// <summary>配置する区画タイプ</summary>
    public enum FloorType
    {
        CommonFloor = 0,
        StraightRoad = 1,
        SteepSlopeRoad = 2,
        GentleSlopeRoad = 3,
        CurveRoad = 4,
    }

    void Awake()
    {
        /*乱数指定*/
        if (!_UseSeed)
        {
            //シード値生成
            _Seed = Random.Range(int.MinValue, int.MaxValue);
        }
        Random.InitState(_Seed);


        /*テーブル初期化*/
        if (_MapSize.x < 5) _MapSize.x = 5;
        if (_MapSize.y < 5) _MapSize.y = 5;
        map = new MapCell[_MapSize.y, _MapSize.x];
        for (int y = 0; y < _MapSize.y; y++)
        {
            for (int x = 0; x < _MapSize.x; x++)
            {
                map[y, x] = new MapCell();
            }
        }

        /*リンク設定*/
        for (int y = 0; y < _MapSize.y; y++)
        {
            for (int x = 0; x < _MapSize.x; x++)
            {
                //真下取得
                if (y > 0)
                {
                    map[y, x].down = map[y - 1, x];

                    //左下取得
                    if (x > 0)
                    {
                        map[y, x].downLeft = map[y - 1, x - 1];
                    }
                    //右下取得
                    if (x < _MapSize.x - 1)
                    {
                        map[y, x].downRight = map[y - 1, x + 1];
                    }
                }
                //真左取得
                if (x > 0)
                {
                    map[y, x].left = map[y, x - 1];
                }
                //真右取得
                if (x < _MapSize.x - 1)
                {
                    map[y, x].right = map[y, x + 1];
                }
                //真上取得
                if (y < _MapSize.y - 1)
                {
                    map[y, x].up = map[y + 1, x];

                    //左上取得
                    if (x > 0)
                    {
                        map[y, x].upLeft = map[y + 1, x - 1];
                    }
                    //右上取得
                    if (x < _MapSize.x - 1)
                    {
                        map[y, x].upRight = map[y + 1, x + 1];
                    }
                }
            }
        }


        SetHeightDiff();
        SetRoad();
        SetParts();
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    /// <summary>高低差構成</summary>
    void SetHeightDiff()
    {
        //任意のマップ4頂点を選択
        Vector2Int processDepth = Vector2Int.zero;
        Vector2Int depthDirection = Vector2Int.zero;
        Vector2Int widthDirection = Vector2Int.zero;
        float rand = Random.value;
        if (rand < 0.25f)
        {
            processDepth = new Vector2Int(0, _MapSize.y - 1);
            
            if(rand < 0.125f)
            {
                depthDirection.x = 1;
                widthDirection.y = -1;
            }
            else
            {
                depthDirection.y = -1;
                widthDirection.x = 1;
            }
        }
        else if(rand < 0.5f)
        {
            processDepth = new Vector2Int(_MapSize.x - 1, 0);

            if (rand < 0.375f)
            {
                depthDirection.x = -1;
                widthDirection.y = 1;
            }
            else
            {
                depthDirection.y = 1;
                widthDirection.x = -1;
            }
        }
        else if(rand < 0.75f)
        {
            processDepth = new Vector2Int(_MapSize.x - 1, _MapSize.y - 1);

            if (rand < 0.625f)
            {
                depthDirection.x = -1;
                widthDirection.y = -1;
            }
            else
            {
                depthDirection.y = -1;
                widthDirection.x = -1;
            }
        }
        else
        {
            if (rand < 0.875f)
            {
                depthDirection.x = 1;
                widthDirection.y = 1;
            }
            else
            {
                depthDirection.y = 1;
                widthDirection.x = 1;
            }
        }

        //指定した四隅角から指定した方向へ床上げ処理
        int depth = 0;
        int width = 0;
        Vector2Int processWidth = Vector2Int.zero;
        while (-1 < processDepth.x && processDepth.x < _MapSize.x
                && -1 < processDepth.y && processDepth.y < _MapSize.y)
        {
            processWidth = processDepth;

            if (depth < 1)
            {
                depth = (int)Random.Range(2f, Mathf.Min(_MapSize.x, _MapSize.y) / 2f);
                width = (int)Random.Range(0f, Mathf.Min(_MapSize.x, _MapSize.y));                
            }

            for (int i = 0; i < width; i++)
            {
                map[processWidth.y, processWidth.x].isUpperFloor = true;
                processWidth += widthDirection;
            }

            processDepth += depthDirection;
            depth--;
        }
    }

    /// <summary>道路指定</summary>
    void SetRoad()
    {
        //外周のどこかをスタート位置に指定
        float rand = Random.value;
        MapCell current = null;
        Compass advanceDirection = Compass.North;
        int[] advanceLimit = new int[4];
        if (rand < 0.5f)
        {
            int startX = (int)Random.Range(_MapSize.x * 0.2f, _MapSize.x * 0.6f);
            //北方向へ
            if (rand < 0.25f)
            {
                map[0, startX].floorType = FloorType.StraightRoad;
                map[0, startX].enter = Compass.South;
                map[0, startX].exit = Compass.North;
                map[1, startX].floorType = FloorType.StraightRoad;
                map[1, startX].enter = Compass.South;
                map[1, startX].exit = Compass.North;
                current = map[1, startX];

                advanceDirection = Compass.North;
                advanceLimit[0] = _MapSize.y;
                advanceLimit[1] = _MapSize.x - startX - 2;
                advanceLimit[2] = 0;
                advanceLimit[3] = startX - 2;
            }
            //南方向へ
            else
            {
                map[_MapSize.y - 1, startX].floorType = FloorType.StraightRoad;
                map[_MapSize.y - 1, startX].enter = Compass.North;
                map[_MapSize.y - 1, startX].exit = Compass.South;
                map[_MapSize.y - 2, startX].floorType = FloorType.StraightRoad;
                map[_MapSize.y - 2, startX].enter = Compass.North;
                map[_MapSize.y - 2, startX].exit = Compass.South;
                current = map[_MapSize.y - 2, startX];

                advanceDirection = Compass.South;
                advanceLimit[0] = 0;
                advanceLimit[1] = _MapSize.x - startX - 2;
                advanceLimit[2] = _MapSize.y;
                advanceLimit[3] = startX - 2;
            }
        }
        else
        {
            int startY = (int)Random.Range(_MapSize.y * 0.2f, _MapSize.y * 0.6f);
            //東方向へ
            if (rand < 0.75f)
            {
                map[startY, 0].floorType = FloorType.StraightRoad;
                map[startY, 0].enter = Compass.West;
                map[startY, 0].exit = Compass.East;
                map[startY, 1].floorType = FloorType.StraightRoad;
                map[startY, 1].enter = Compass.West;
                map[startY, 1].exit = Compass.East;
                current = map[startY, 1];

                advanceDirection = Compass.East;
                advanceLimit[0] = _MapSize.y - startY - 2;
                advanceLimit[1] = _MapSize.x;
                advanceLimit[2] = startY - 2;
                advanceLimit[3] = 0;
            }
            //西方向へ
            else
            {
                map[startY, _MapSize.x - 1].floorType = FloorType.StraightRoad;
                map[startY, _MapSize.x - 1].enter = Compass.East;
                map[startY, _MapSize.x - 1].exit = Compass.West;
                map[startY, _MapSize.x - 2].floorType = FloorType.StraightRoad;
                map[startY, _MapSize.x - 2].enter = Compass.East;
                map[startY, _MapSize.x - 2].exit = Compass.West;
                current = map[startY, _MapSize.x - 2];

                advanceDirection = Compass.West;
                advanceLimit[0] = _MapSize.y - startY - 2;
                advanceLimit[1] = 0;
                advanceLimit[2] = startY - 2;
                advanceLimit[3] = _MapSize.x;
            }
        }

        //穴掘り
        int loopLimit = _MapSize.x * _MapSize.y;
        Compass beforeStep = advanceDirection;
        bool isCurve = false;
        for (int i = 0; i < loopLimit; i++)
        {
            bool isDetectNext = false;
            Compass goNext = advanceDirection;
            for (int k = 0; k < 40 && !isDetectNext; k++)
            {
                rand = Random.value;
                //右サイドに向けて進行
                if (rand > 0.9f)
                {
                    if ((int)advanceDirection > 2)
                    {
                        goNext = Compass.North;
                    }
                    else
                    {
                        goNext = advanceDirection + 1;
                    }
                }
                //左サイドに向けて進行
                else if (rand > 0.8f)
                {
                    if ((int)advanceDirection < 1)
                    {
                        goNext = Compass.West;
                    }
                    else
                    {
                        goNext = advanceDirection - 1;
                    }
                }

                isCurve = goNext != beforeStep;


                //次に向かう方向
                switch (goNext)
                {
                    //北方向へ
                    case Compass.North:
                        if (current.up.floorType == FloorType.CommonFloor && advanceLimit[0] > 0)
                        {
                            if (isCurve)
                            {
                                current.floorType = FloorType.CurveRoad;
                            }

                            //今の区画が上で、次の区画が下の場合
                            if (current.isUpperFloor && !current.up.isUpperFloor)
                            {
                                if (isCurve)
                                {
                                    current.up.isUpperFloor = true;
                                }
                                else
                                {
                                    current.isUpperFloor = false;
                                    current.floorType = FloorType.SteepSlopeRoad;
                                }
                            }
                            //今の区画が下で、次の区画が上の場合
                            else if (!current.isUpperFloor && current.up.isUpperFloor)
                            {
                                if (isCurve)
                                {
                                    current.up.isUpperFloor = false;
                                }
                                else
                                {
                                    current.floorType = FloorType.SteepSlopeRoad;
                                }
                            }

                            current.exit = Compass.North;

                            current = current.up;
                            current.floorType = FloorType.StraightRoad;
                            current.enter = Compass.South;
                            isDetectNext = true;

                            advanceLimit[0]--;
                        }
                        break;
                    //南方向へ
                    case Compass.South:
                        if (current.down.floorType == FloorType.CommonFloor && advanceLimit[2] > 0)
                        {
                            if (isCurve)
                            {
                                current.floorType = FloorType.CurveRoad;
                            }

                            //今の区画が上で、次の区画が下の場合
                            if (current.isUpperFloor && !current.down.isUpperFloor)
                            {
                                if (isCurve)
                                {
                                    current.down.isUpperFloor = true;
                                }
                                else
                                {
                                    current.isUpperFloor = false;
                                    current.floorType = FloorType.SteepSlopeRoad;
                                }
                            }
                            //今の区画が下で、次の区画が上の場合
                            else if (!current.isUpperFloor && current.down.isUpperFloor)
                            {
                                if (isCurve)
                                {
                                    current.down.isUpperFloor = false;
                                }
                                else
                                {
                                    current.floorType = FloorType.SteepSlopeRoad;
                                }
                            }

                            current.exit = Compass.South;

                            current = current.down;
                            current.floorType = FloorType.StraightRoad;
                            current.enter = Compass.North;
                            isDetectNext = true;

                            advanceLimit[2]--;
                        }
                        break;
                    //東方向へ
                    case Compass.East:
                        if (current.right.floorType == FloorType.CommonFloor && advanceLimit[1] > 0)
                        {
                            if (isCurve)
                            {
                                current.floorType = FloorType.CurveRoad;
                            }

                            //今の区画が上で、次の区画が下の場合
                            if (current.isUpperFloor && !current.right.isUpperFloor)
                            {
                                if (isCurve)
                                {
                                    current.right.isUpperFloor = true;
                                }
                                else
                                {
                                    current.isUpperFloor = false;
                                    current.floorType = FloorType.SteepSlopeRoad;
                                }
                            }
                            //今の区画が下で、次の区画が上の場合
                            else if (!current.isUpperFloor && current.right.isUpperFloor)
                            {
                                if (isCurve)
                                {
                                    current.right.isUpperFloor = false;
                                }
                                else
                                {
                                    current.floorType = FloorType.SteepSlopeRoad;
                                }
                            }

                            current.exit = Compass.East;

                            current = current.right;
                            current.floorType = FloorType.StraightRoad;
                            current.enter = Compass.West;
                            isDetectNext = true;

                            advanceLimit[1]--;
                        }
                        break;
                    //西方向
                    case Compass.West:
                        if (current.left.floorType == FloorType.CommonFloor && advanceLimit[3] > 0)
                        {
                            if (isCurve)
                            {
                                current.floorType = FloorType.CurveRoad;
                            }

                            //今の区画が上で、次の区画が下の場合
                            if (current.isUpperFloor && !current.left.isUpperFloor)
                            {
                                if (isCurve)
                                {
                                    current.left.isUpperFloor = true;
                                }
                                else
                                {
                                    current.isUpperFloor = false;
                                    current.floorType = FloorType.SteepSlopeRoad;
                                }
                            }
                            //今の区画が下で、次の区画が上の場合
                            else if (!current.isUpperFloor && current.left.isUpperFloor)
                            {
                                if (isCurve)
                                {
                                    current.left.isUpperFloor = false;
                                }
                                else
                                {
                                    current.floorType = FloorType.SteepSlopeRoad;
                                }
                            }

                            current.exit = Compass.West;

                            current = current.left;
                            current.floorType = FloorType.StraightRoad;
                            current.enter = Compass.East;
                            isDetectNext = true;

                            advanceLimit[3]--;
                        }
                        break;
                    default: break;
                }
            }

            beforeStep = goNext;

            //どこかマップの端に到達していれば離脱
            if (current.up == null || current.down == null
                || current.right == null || current.left == null)
            {
                //最後の道の出口を外側に指定
                switch (current.enter)
                {
                    case Compass.North:
                        current.exit = Compass.South;
                        break;
                    case Compass.East:
                        current.exit = Compass.West;
                        break;
                    case Compass.South:
                        current.exit = Compass.North;
                        break;
                    case Compass.West:
                        current.exit = Compass.East;
                        break;
                    default: break;
                }

                break;
            }
        }
    }

    /// <summary>パーツ配置</summary>
    void SetParts()
    {
        //構造物配置決めのためのリスト
        List<Vector2Int> buildCandidates = new List<Vector2Int>(_MapSize.x * _MapSize.y);

        /*地形配置*/
        GameObject pref = null;
        for (int y = 0; y < _MapSize.y; y++)
        {
            for (int x = 0; x < _MapSize.x; x++)
            {
                Vector3 forward = Vector3.forward;
                if (map[y, x].floorType != FloorType.CommonFloor)
                {
                    //入口と出口の位置関係から道路の種類と置き方を指定
                    switch (map[y, x].enter)
                    {
                        //北が入口
                        case Compass.North:
                            switch (map[y, x].exit)
                            {
                                case Compass.East:
                                    pref = _CurveRoad;
                                    break;
                                //直進
                                case Compass.South:
                                    pref = _StraightRoad;
                                    forward = Vector3.left;
                                    break;
                                case Compass.West:
                                    pref = _CurveRoad;
                                    forward = Vector3.left;
                                    break;
                                default: break;
                            }
                            break;
                        //東が入口
                        case Compass.East:
                            switch (map[y, x].exit)
                            {
                                case Compass.North:
                                    pref = _CurveRoad;
                                    break;
                                case Compass.South:
                                    pref = _CurveRoad;
                                    forward = Vector3.right;
                                    break;
                                //直進
                                case Compass.West:
                                    pref = _StraightRoad;
                                    break;
                                default: break;
                            }
                            break;
                        //南が入口
                        case Compass.South:
                            switch (map[y, x].exit)
                            {
                                //直進
                                case Compass.North:
                                    pref = _StraightRoad;
                                    forward = Vector3.left;
                                    break;
                                case Compass.East:
                                    pref = _CurveRoad;
                                    forward = Vector3.right;
                                    break;
                                case Compass.West:
                                    pref = _CurveRoad;
                                    forward = Vector3.back;
                                    break;
                                default: break;
                            }
                            break;
                        //西が入口
                        case Compass.West:
                            switch (map[y, x].exit)
                            {
                                case Compass.North:
                                    pref = _CurveRoad;
                                    forward = Vector3.left;
                                    break;
                                //直進
                                case Compass.East:
                                    pref = _StraightRoad;
                                    break;
                                case Compass.South:
                                    pref = _CurveRoad;
                                    forward = Vector3.back;
                                    break;
                                default: break;
                            }
                            break;
                        default: break;
                    }
                }
                else
                {
                    pref = _CommonFloor;
                }

                //南隣の区画をチェック
                if(map[y, x].down != null)
                {
                    //その区画の方が高い位置にある
                    if (map[y, x].down.isUpperFloor && !map[y, x].isUpperFloor)
                    {
                        //両区画は道かつ方角の一致する入口と出口がある
                        if ((map[y, x].down.floorType != FloorType.CommonFloor && map[y, x].floorType != FloorType.CommonFloor)
                            && (map[y - 1, x].enter == map[y, x].enter || map[y - 1, x].exit == map[y, x].exit))
                        {
                            forward = Vector3.left;
                            pref = _SteepSlopeRoad;

                            //高低差壁作成
                            if (map[y, x].right != null && !map[y, x].right.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_BothSideWall);
                                wall.transform.SetParent(transform);
                                wall.transform.position = new Vector3(x * _Unit + _Unit / 2f, 0f, y * _Unit);
                                wall.transform.forward = Vector3.right;
                            }
                            if (map[y, x].left != null && !map[y, x].left.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_BothSideWall);
                                wall.transform.SetParent(transform);
                                wall.transform.position = new Vector3(x * _Unit - _Unit / 2f, 0f, y * _Unit);
                                wall.transform.forward = Vector3.left;
                            }
                        }
                        else
                        {
                            //高低差壁作成
                            GameObject wall = Instantiate(_DifferentHeightWall);
                            wall.transform.SetParent(transform);
                            wall.transform.position = new Vector3(x * _Unit, 0f, y * _Unit - _Unit / 2f);
                            wall.transform.forward = Vector3.back;
                        }
                    }

                    //その区画の方が低い位置にある
                    if (!map[y, x].down.isUpperFloor && map[y, x].isUpperFloor)
                    {
                        //コーナーポストを置く判定
                        if (map[y, x].right != null && !map[y, x].right.isUpperFloor)
                        {
                            GameObject post = Instantiate(_WallCornerPost);
                            post.transform.SetParent(transform);
                            post.transform.position = new Vector3(x * _Unit + _Unit / 2f, 0f, y * _Unit - _Unit / 2f);
                        }
                        if (map[y, x].left != null && !map[y, x].left.isUpperFloor)
                        {
                            GameObject post = Instantiate(_WallCornerPost);
                            post.transform.SetParent(transform);
                            post.transform.position = new Vector3(x * _Unit - _Unit / 2f, 0f, y * _Unit - _Unit / 2f);
                        }
                    }
                }
                //北隣の区画をチェック
                if (map[y, x].up != null)
                {
                    //その区画の方が高い位置にある
                    if (map[y, x].up.isUpperFloor && !map[y, x].isUpperFloor)
                    {
                        //両区画は道かつ方角の一致する入口と出口がある
                        if ((map[y, x].up.floorType != FloorType.CommonFloor && map[y, x].floorType != FloorType.CommonFloor)
                            && (map[y, x].up.enter == map[y, x].enter || map[y, x].up.exit == map[y, x].exit))
                        {
                            forward = Vector3.right;
                            pref = _SteepSlopeRoad;

                            //高低差壁作成
                            if (map[y, x].right != null && !map[y, x].right.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_BothSideWall);
                                wall.transform.SetParent(transform);
                                wall.transform.position = new Vector3(x * _Unit + _Unit / 2f, 0f, y * _Unit);
                                wall.transform.forward = Vector3.right;
                            }
                            if (map[y, x].left != null && !map[y, x].left.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_BothSideWall);
                                wall.transform.SetParent(transform);
                                wall.transform.position = new Vector3(x * _Unit - _Unit / 2f, 0f, y * _Unit);
                                wall.transform.forward = Vector3.left;
                            }
                        }
                        else
                        {
                            //高低差壁作成
                            GameObject wall = Instantiate(_DifferentHeightWall);
                            wall.transform.SetParent(transform);
                            wall.transform.position = new Vector3(x * _Unit, 0f, y * _Unit + _Unit / 2f);
                        }
                    }

                    //その区画の方が低い位置にある
                    if (!map[y, x].up.isUpperFloor && map[y, x].isUpperFloor)
                    {
                        //コーナーポストを置く判定
                        if (map[y, x].right != null && !map[y, x].right.isUpperFloor)
                        {
                            GameObject post = Instantiate(_WallCornerPost);
                            post.transform.SetParent(transform);
                            post.transform.position = new Vector3(x * _Unit + _Unit / 2f, 0f, y * _Unit + _Unit / 2f);
                        }
                        if (map[y, x].left != null && !map[y, x].left.isUpperFloor)
                        {
                            GameObject post = Instantiate(_WallCornerPost);
                            post.transform.SetParent(transform);
                            post.transform.position = new Vector3(x * _Unit - _Unit / 2f, 0f, y * _Unit + _Unit / 2f);
                        }
                    }
                }
                //西隣の区画をチェック
                if(map[y, x].left != null)
                {
                    //その区画の方が高い位置にある
                    if (map[y, x].left.isUpperFloor && !map[y, x].isUpperFloor)
                    {
                        //両区画は道かつ方角の一致する入口と出口がある
                        if ((map[y, x].left.floorType != FloorType.CommonFloor && map[y, x].floorType != FloorType.CommonFloor)
                            && (map[y, x].left.enter == map[y, x].enter || map[y, x].left.exit == map[y, x].exit))
                        {
                            pref = _SteepSlopeRoad;

                            //高低差壁作成
                            if (map[y, x].up != null && !map[y, x].up.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_BothSideWall);
                                wall.transform.SetParent(transform);
                                wall.transform.position = new Vector3(x * _Unit, 0f, y * _Unit + _Unit / 2f);
                            }
                            if (map[y, x].down != null && !map[y, x].down.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_BothSideWall);
                                wall.transform.SetParent(transform);
                                wall.transform.position = new Vector3(x * _Unit, 0f, y * _Unit - _Unit / 2f);
                                wall.transform.forward = Vector3.back;
                            }
                        }
                        else
                        {
                            //高低差壁作成
                            GameObject wall = Instantiate(_DifferentHeightWall);
                            wall.transform.SetParent(transform);
                            wall.transform.position = new Vector3(x * _Unit - _Unit / 2f, 0f, y * _Unit);
                            wall.transform.forward = Vector3.left;
                        }
                    }
                }
                //東隣の区画をチェック
                if (map[y, x].right != null)
                {
                    //その区画の方が高い位置にある
                    if (map[y, x].right.isUpperFloor && !map[y, x].isUpperFloor)
                    {
                        //両区画は道かつ方角の一致する入口と出口がある
                        if ((map[y, x].right.floorType != FloorType.CommonFloor && map[y, x].floorType != FloorType.CommonFloor)
                            && (map[y, x].right.enter == map[y, x].enter || map[y, x].right.exit == map[y, x].exit))
                        {
                            forward = Vector3.back;
                            pref = _SteepSlopeRoad;

                            //高低差壁作成
                            if (map[y, x].up != null && !map[y, x].up.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_BothSideWall);
                                wall.transform.SetParent(transform);
                                wall.transform.position = new Vector3(x * _Unit, 0f, y * _Unit + _Unit / 2f);
                            }
                            if (map[y, x].down != null && !map[y, x].down.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_BothSideWall);
                                wall.transform.SetParent(transform);
                                wall.transform.position = new Vector3(x * _Unit, 0f, y * _Unit - _Unit / 2f);
                                wall.transform.forward = Vector3.back;
                            }
                        }
                        else
                        {
                            //高低差壁作成
                            GameObject wall = Instantiate(_DifferentHeightWall);
                            wall.transform.SetParent(transform);
                            wall.transform.position = new Vector3(x * _Unit + _Unit / 2f, 0f, y * _Unit);
                            wall.transform.forward = Vector3.right;
                        }
                    }
                }

                //地面の生成
                GameObject ins = Instantiate(pref);
                ins.transform.SetParent(transform);
                ins.transform.position = new Vector3(x, map[y, x].isUpperFloor ? 0.2f : 0f, y) * _Unit;
                ins.transform.forward = forward;

                //道路以外は建造物配置候補
                if (map[y, x].floorType == FloorType.CommonFloor)
                {
                    buildCandidates.Add(new Vector2Int(x, y));
                }
            }
        }

        /*建造物配置*/
        int numberOfCandidate = buildCandidates.Count / 2;
        for (int i = 0; i < numberOfCandidate; i++)
        {
            //ランダムで候補地を指定
            int targetIndex = (int)Random.Range(1f, buildCandidates.Count) - 1;
            Vector2Int candidate = buildCandidates[targetIndex];

            //建造物が建てられていない
            if(!map[candidate.y, candidate.x].isBuilt)
            {
                MapCell[] cells = map[candidate.y, candidate.x].GetField3x3();
                if (cells is not null)
                {
                    GameObject build = Instantiate(_Buldings3x3[(int)Random.Range(0f, _Buldings3x3.Length - 0.01f)]);
                    build.transform.SetParent(transform);
                    build.transform.position = new Vector3(candidate.x, map[candidate.y, candidate.x].isUpperFloor ? 0.2f : 0f, candidate.y) * _Unit;

                    foreach (MapCell cell in cells)
                    {
                        cell.isBuilt = true;
                    }
                }
                else
                {
                    GameObject build = Instantiate(_Buldings1x1[(int)Random.Range(0f, _Buldings1x1.Length - 0.01f)]);
                    build.transform.SetParent(transform);
                    build.transform.position = new Vector3(candidate.x, map[candidate.y, candidate.x].isUpperFloor ? 0.2f : 0f, candidate.y) * _Unit;
                    map[candidate.y, candidate.x].isBuilt = true;
                }
            }

            buildCandidates.Remove(candidate);
        }
    }


    /// <summary>マップ1マスの情報</summary>
    class MapCell
    {
        /// <summary>道の入口</summary>
        public Compass enter = Compass.North;

        /// <summary>道の出口</summary>
        public Compass exit = Compass.South;

        /// <summary>区画の種類</summary>
        public FloorType floorType = FloorType.CommonFloor;

        /// <summary>true : 一段高い床</summary>
        public bool isUpperFloor = false;

        /// <summary>true : 建物が占有されている</summary>
        public bool isBuilt = false;



        /// <summary>左上の区画の情報</summary>
        public MapCell upLeft = null;

        /// <summary>真上の区画の情報</summary>
        public MapCell up = null;

        /// <summary>右上の区画の情報</summary>
        public MapCell upRight = null;

        /// <summary>真左の区画の情報</summary>
        public MapCell left = null;

        /// <summary>真右の区画の情報</summary>
        public MapCell right = null;

        /// <summary>左下の区画の情報</summary>
        public MapCell downLeft = null;

        /// <summary>真下の区画の情報</summary>
        public MapCell down = null;

        /// <summary>右下の区画の情報</summary>
        public MapCell downRight = null;


        /// <summary>未占有の3×3の地形を探す</summary>
        /// <returns>あれば対象の全区画</returns>
        public MapCell[] GetField3x3()
        {
            MapCell[] cells = { this, upLeft, up, upRight, left, right, downLeft, down, downRight };

            if (cells.Contains(null)
                || cells.Where(c => c.isUpperFloor != isUpperFloor 
                                || c.floorType != FloorType.CommonFloor 
                                || c.isBuilt).Count() > 0)
            {
                return null;
            }

            return cells;
        }



        public MapCell(FloorType floorType = FloorType.CommonFloor)
        {
            this.floorType = floorType;
        }
    }
}