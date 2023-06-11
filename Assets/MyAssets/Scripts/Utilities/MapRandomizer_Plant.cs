using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider))]
public class MapRandomizer_Plant : MonoBehaviour, IStartLocation
{
    /// <summary>基本試行回数</summary>
    int NUMBER_OF_TRIAL = 40;

    /// <summary>床のマス目1辺の長さ</summary>
    float UNIT_FOR_FLOOR = 10f;

    /// <summary>空中回廊の1辺の長さ</summary>
    float UNIT_FOR_BRIDGE = 5f;

    /// <summary>ゲームするエリアとなる外周からの距離</summary>
    byte GAME_AREA_AROUND_DISTANCE = 7;


    /// <summary>スタート地点になるフロア座標</summary>
    Vector2Int _startFloor = new Vector2Int();

    /// <summary>スタート地点になるフロアの高さ</summary>
    float _startFloorHeight = 0f;


    /// <summary>スタート地点になるフロア座標</summary>
    public Vector3 StartFloorBasePosition => new Vector3(_startFloor.x * UNIT_FOR_FLOOR, _startFloorHeight, _startFloor.y * UNIT_FOR_FLOOR);


    [SerializeField, Tooltip("マスの数(一辺15マス以上)")]
    Vector2Int _mapSize = new Vector2Int(15, 15);

    [SerializeField, Tooltip("true : 乱数シードを指定する")]
    bool _useSeed = false;

    [SerializeField, Tooltip("乱数シード")]
    int _seed = 10;


    /// <summary>エリア外に行けないようにするコライダー</summary>
    BoxCollider[] _areaLimits = null;


    [Header("地面プレハブ")]
    [SerializeField, Tooltip("普通地面区画")]
    GameObject _commonFloor = null;

    [SerializeField, Tooltip("直進道路区画")]
    GameObject _straightRoad = null;

    [SerializeField, Tooltip("急な登坂区画")]
    GameObject _steepSlopeRoad = null;

    [SerializeField, Tooltip("緩い登坂区画")]
    GameObject _gentleSlopeRoad = null;

    [SerializeField, Tooltip("曲線道路区画")]
    GameObject _curveRoad = null;

    [SerializeField, Tooltip("階段が食い込んだ普通地面区画")]
    GameObject _commonFloorInnerStair = null;


    [Header("高低差壁プレハブ")]
    [SerializeField, Tooltip("高低差埋めの壁 普通")]
    GameObject _differentHeightWall = null;

    [SerializeField, Tooltip("高低差埋めの壁 両面あり")]
    GameObject _bothSideWall = null;

    [SerializeField, Tooltip("壁の角を埋める柱")]
    GameObject _wallCornerPost = null;

    [SerializeField, Tooltip("壁と埋め込まれた左階段")]
    GameObject _innerStairLeft = null;

    [SerializeField, Tooltip("壁と埋め込まれた右階段")]
    GameObject _innerStairRight = null;

    [SerializeField, Tooltip("壁と左側にせり出した階段")]
    GameObject _outerStairLeft = null;

    [SerializeField, Tooltip("壁と右側にせり出した階段")]
    GameObject _outerStairRight = null;


    [Header("フェンスプレハブ")]
    [SerializeField, Tooltip("コンクリートの壁")]
    GameObject _concreteFence = null;

    [SerializeField, Tooltip("コンクリート壁の角を埋める柱")]
    GameObject _concreteFenceCornerPost = null;

    [SerializeField, Tooltip("金網の壁")]
    GameObject _latticeFence = null;

    [SerializeField, Tooltip("エリア外となる道止め")]
    GameObject _roadBlockAreaLimit = null;


    [Header("連絡通路プレハブ")]
    [SerializeField, Tooltip("階段")]
    GameObject _bridgeStair = null;

    [SerializeField, Tooltip("直進通路")]
    GameObject _bridgeStraight = null;

    [SerializeField, Tooltip("支柱付き直進通路")]
    GameObject _bridgeStraightStrut = null;

    [SerializeField, Tooltip("曲進通路")]
    GameObject _bridgeCorner = null;


    [Header("配管プレハブ")]
    [SerializeField, Tooltip("二本並列配管")]
    GameObject _pipeTwoStraight = null;

    [SerializeField, Tooltip("二本並列上下配管")]
    GameObject _pipeTwoHigher = null;


    [Header("建物プレハブ")]
    [SerializeField, Tooltip("4×3の土地を使う建物")]
    GameObject[] _buldings4x3 = null;

    [SerializeField, Tooltip("3×3の土地を使う建物")]
    GameObject[] _buldings3x3 = null;

    [SerializeField, Tooltip("3×2の土地を使う建物")]
    GameObject[] _buldings3x2 = null;

    [SerializeField, Tooltip("2×2の土地を使う建物")]
    GameObject[] _buldings2x2 = null;

    [SerializeField, Tooltip("2×1の土地を使う建物")]
    GameObject[] _buldings2x1 = null;

    [SerializeField, Tooltip("1つの土地を使う建物")]
    GameObject[] _buldings1x1 = null;




    /// <summary>マップテーブル</summary>
    MapCell[,] map = null;

    /// <summary>橋のマップテーブル</summary>
    BridgeCell[,] bridges = null;


    /// <summary>方角</summary>
    public enum Compass
    {
        North = 1,
        East = 2,
        South = 3,
        West = 4,
    }

    /// <summary>配置する区画タイプ</summary>
    public enum FloorType : byte
    {
        CommonFloor = 0,
        StraightRoad = 1,
        SteepSlopeRoad = 2,
        GentleSlopeRoad = 3,
        CurveRoad = 4,
    }

    /// <summary>配置する建造物タイプ</summary>
    public enum BuildType : byte
    {
        None = 0,
        FloorStair,
        BuildSmall,
        BuildLarge,
    }

    /// <summary>配置する空中通路タイプ</summary>
    public enum BridgeType : byte
    {
        None = 0,
        LowerStair,
        LowerRoad,
        UpperStair,
        UpperRoad,
    }

    void Awake()
    {
        /*乱数指定*/
        if (!_useSeed)
        {
            //シード値生成
            _seed = Random.Range(int.MinValue, int.MaxValue);
        }
        Random.InitState(_seed);


        Initialize();

        SetHeightDiff();
        SetRoad();
        SetBridge();
        SetParts();

        //BoxCollider col = GetComponent<BoxCollider>();

        /*メモリ解放*/
        map = null;
        bridges = null;
    }

    /// <summary>テーブル初期化</summary>
    private void Initialize()
    {
        if (_mapSize.x < 15) _mapSize.x = 15;
        if (_mapSize.y < 15) _mapSize.y = 15;
        map = new MapCell[_mapSize.y, _mapSize.x];
        bridges = new BridgeCell[_mapSize.y * 2, _mapSize.x * 2];
        for (int y = 0; y < _mapSize.y; y++)
        {
            for (int x = 0; x < _mapSize.x; x++)
            {
                map[y, x] = new MapCell();
                bridges[y * 2, x * 2] = new BridgeCell(map[y, x]);
                bridges[y * 2 + 1, x * 2] = new BridgeCell(map[y, x]);
                bridges[y * 2, x * 2 + 1] = new BridgeCell(map[y, x]);
                bridges[y * 2 + 1, x * 2 + 1] = new BridgeCell(map[y, x]);

                if (GAME_AREA_AROUND_DISTANCE - 1 < y && y < _mapSize.y - GAME_AREA_AROUND_DISTANCE - 1
                    && GAME_AREA_AROUND_DISTANCE - 1 < x && x < _mapSize.x - GAME_AREA_AROUND_DISTANCE - 1)
                {
                    map[y, x].region = 1;
                }
                else
                {
                    map[y, x].region = 0;
                }
            }
        }

        /*リンク設定*/
        //地面
        for (int y = 0; y < _mapSize.y; y++)
        {
            for (int x = 0; x < _mapSize.x; x++)
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
                    if (x < _mapSize.x - 1)
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
                if (x < _mapSize.x - 1)
                {
                    map[y, x].right = map[y, x + 1];
                }
                //真上取得
                if (y < _mapSize.y - 1)
                {
                    map[y, x].up = map[y + 1, x];

                    //左上取得
                    if (x > 0)
                    {
                        map[y, x].upLeft = map[y + 1, x - 1];
                    }
                    //右上取得
                    if (x < _mapSize.x - 1)
                    {
                        map[y, x].upRight = map[y + 1, x + 1];
                    }
                }
            }
        }
        //連絡通路
        for (int y = 0; y < bridges.GetLength(0); y++)
        {
            for (int x = 0; x < bridges.GetLength(1); x++)
            {
                //真下取得
                if (y > 0)
                {
                    bridges[y, x].down = bridges[y - 1, x];

                    //左下取得
                    if (x > 0)
                    {
                        bridges[y, x].downLeft = bridges[y - 1, x - 1];
                    }
                    //右下取得
                    if (x < bridges.GetLength(1) - 1)
                    {
                        bridges[y, x].downRight = bridges[y - 1, x + 1];
                    }
                }
                //真左取得
                if (x > 0)
                {
                    bridges[y, x].left = bridges[y, x - 1];
                }
                //真右取得
                if (x < bridges.GetLength(1) - 1)
                {
                    bridges[y, x].right = bridges[y, x + 1];
                }
                //真上取得
                if (y < bridges.GetLength(0) - 1)
                {
                    bridges[y, x].up = bridges[y + 1, x];

                    //左上取得
                    if (x > 0)
                    {
                        bridges[y, x].upLeft = bridges[y + 1, x - 1];
                    }
                    //右上取得
                    if (x < bridges.GetLength(1) - 1)
                    {
                        bridges[y, x].upRight = bridges[y + 1, x + 1];
                    }
                }
            }
        }

        //エリア四方の壁張り
        _areaLimits = GetComponents<BoxCollider>();
        for(int i = 0; i < 4 && i < _areaLimits.Length; i++)
        {
            switch (i)
            {
                case 0:
                    _areaLimits[i].center = new Vector3((GAME_AREA_AROUND_DISTANCE - 0.5f) * UNIT_FOR_FLOOR, 5f, (_mapSize.y - 2f) * UNIT_FOR_FLOOR / 2f);
                    _areaLimits[i].size = new Vector3(0.5f, 10f, (_mapSize.y - GAME_AREA_AROUND_DISTANCE * 2f) * UNIT_FOR_FLOOR);
                    break;
                case 1:
                    _areaLimits[i].center = new Vector3((_mapSize.x - GAME_AREA_AROUND_DISTANCE - 1.5f) * UNIT_FOR_FLOOR, 5f, (_mapSize.y - 2f) * UNIT_FOR_FLOOR / 2f);
                    _areaLimits[i].size = new Vector3(0.5f, 10f, (_mapSize.y - GAME_AREA_AROUND_DISTANCE * 2f) * UNIT_FOR_FLOOR);
                    break;
                case 2:
                    _areaLimits[i].center = new Vector3((_mapSize.x - 2f) * UNIT_FOR_FLOOR / 2f, 5f, (GAME_AREA_AROUND_DISTANCE - 0.5f) * UNIT_FOR_FLOOR);
                    _areaLimits[i].size = new Vector3((_mapSize.x - GAME_AREA_AROUND_DISTANCE * 2f) * UNIT_FOR_FLOOR, 10f, 0.5f);
                    break;
                case 3:
                    _areaLimits[i].center = new Vector3((_mapSize.x - 2f) * UNIT_FOR_FLOOR / 2f, 5f, (_mapSize.y - GAME_AREA_AROUND_DISTANCE - 1.5f) * UNIT_FOR_FLOOR);
                    _areaLimits[i].size = new Vector3((_mapSize.x - GAME_AREA_AROUND_DISTANCE * 2f) * UNIT_FOR_FLOOR, 10f, 0.5f);
                    break;
                default: break;
            }
        }
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
            processDepth = new Vector2Int(0, _mapSize.y - 1);

            if (rand < 0.125f)
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
        else if (rand < 0.5f)
        {
            processDepth = new Vector2Int(_mapSize.x - 1, 0);

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
        else if (rand < 0.75f)
        {
            processDepth = new Vector2Int(_mapSize.x - 1, _mapSize.y - 1);

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
        while (-1 < processDepth.x && processDepth.x < _mapSize.x
                && -1 < processDepth.y && processDepth.y < _mapSize.y)
        {
            processWidth = processDepth;

            if (depth < 1)
            {
                depth = (int)Random.Range(2f, Mathf.Min(_mapSize.x, _mapSize.y) / 2f);
                width = (int)Random.Range(0f, Mathf.Min(_mapSize.x, _mapSize.y));
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
        //マップの中央付近のどこかをスタート位置に指定
        int startX = (int)Random.Range(_mapSize.x * 0.4f, _mapSize.x * 0.6f);
        int startY = (int)Random.Range(_mapSize.y * 0.4f, _mapSize.y * 0.6f);

        //東西南北穴掘りする方向を指定
        Compass[] fowards = new Compass[2];
        float rand = Random.value;
        //一方は北方向へ
        if(rand < 0.5f)
        {
            fowards[0] = Compass.North;
            //もう一方は南方向へ
            if (rand < 0.25f)
            {
                fowards[1] = Compass.South;
            }
            //もう一方は西方向へ
            else
            {
                fowards[1] = Compass.West;
            }
        }
        //一方は東方向へ
        else
        {
            fowards[0] = Compass.East;
            //もう一方は南方向へ
            if (rand < 0.75f)
            {
                fowards[1] = Compass.South;
            }
            //もう一方は西方向へ
            else
            {
                fowards[1] = Compass.West;
            }
        }

        //2方向に穴掘り
        map[startY, startX].enter = fowards[1];
        map[startY, startX].exit = fowards[0];
        foreach (Compass f in fowards)
        {
            MapCell current = null;
            Compass advanceDirection = Compass.North;
            int[] advanceLimit = new int[4];

            //掘る前に下処理
            switch (f)
            {
                case Compass.North:

                    map[startY, startX].floorType = FloorType.StraightRoad;
                    map[startY + 1, startX].floorType = FloorType.StraightRoad;
                    map[startY + 1, startX].enter = Compass.South;
                    map[startY + 1, startX].exit = Compass.North;
                    map[startY + 1, startX].isUpperFloor = map[startY, startX].isUpperFloor;
                    current = map[startY + 1, startX];

                    advanceDirection = Compass.North;
                    advanceLimit[0] = _mapSize.y;
                    advanceLimit[1] = _mapSize.x - startX - 2;
                    advanceLimit[2] = 0;
                    advanceLimit[3] = startX - 2;

                    break;

                case Compass.South:

                    map[_mapSize.y - startY, startX].floorType = FloorType.StraightRoad;
                    map[_mapSize.y - startY - 1, startX].floorType = FloorType.StraightRoad;
                    map[_mapSize.y - startY - 1, startX].enter = Compass.North;
                    map[_mapSize.y - startY - 1, startX].exit = Compass.South;
                    map[_mapSize.y - startY - 1, startX].isUpperFloor = map[startY, startX].isUpperFloor;
                    current = map[_mapSize.y - startY - 1, startX];

                    advanceDirection = Compass.South;
                    advanceLimit[0] = 0;
                    advanceLimit[1] = _mapSize.x - startX - 2;
                    advanceLimit[2] = _mapSize.y;
                    advanceLimit[3] = startX - 2;

                    break;

                case Compass.East:

                    map[startY, startX].floorType = FloorType.StraightRoad;
                    map[startY, startX + 1].floorType = FloorType.StraightRoad;
                    map[startY, startX + 1].enter = Compass.West;
                    map[startY, startX + 1].exit = Compass.East;
                    map[startY, startX + 1].isUpperFloor = map[startY, startX].isUpperFloor;
                    current = map[startY, startX + 1];

                    advanceDirection = Compass.East;
                    advanceLimit[0] = _mapSize.y - startY - 2;
                    advanceLimit[1] = _mapSize.x;
                    advanceLimit[2] = startY - 2;
                    advanceLimit[3] = 0;

                    break;

                case Compass.West:

                    map[startY, _mapSize.x - startX].floorType = FloorType.StraightRoad;
                    map[startY, _mapSize.x - startX - 1].floorType = FloorType.StraightRoad;
                    map[startY, _mapSize.x - startX - 1].enter = Compass.East;
                    map[startY, _mapSize.x - startX - 1].exit = Compass.West;
                    map[startY, _mapSize.x - startX - 1].isUpperFloor = map[startY, startX].isUpperFloor;
                    current = map[startY, _mapSize.x - startX - 1];

                    advanceDirection = Compass.West;
                    advanceLimit[0] = _mapSize.y - startY - 2;
                    advanceLimit[1] = 0;
                    advanceLimit[2] = startY - 2;
                    advanceLimit[3] = _mapSize.x;

                    break;
                default: break;
            }

            //穴掘り
            int loopLimit = _mapSize.x * _mapSize.y;
            Compass beforeStep = advanceDirection;
            bool isCurve = false;
            for (int i = 0; i < loopLimit; i++)
            {
                bool isDetectNext = false;
                Compass goNext = advanceDirection;
                for (int k = 0; k < NUMBER_OF_TRIAL && !isDetectNext; k++)
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

        //スタート位置登録
        _startFloor = new Vector2Int(startX, startY);
        _startFloorHeight = map[startY, startX].isUpperFloor ? 2f : 0f;
    }

    /// <summary>連絡通路指定</summary>
    void SetBridge()
    {
        //配置試行回数を決定
        int ratioBase = (_mapSize.x + _mapSize.y) / 2;
        int numberOfTrySetBridge = (int)Random.Range(ratioBase * 0.5f, ratioBase * 1.5f);

        //連絡通路の最大範囲を指定
        int lengthOfBridge = (_mapSize.x + _mapSize.y) / 4;

        //配置を試行 できなかった場合は中断して回数だけ消費
        for (int i = 0; i < numberOfTrySetBridge; i++)
        {
            //階段二つをセットとして同じ高さに配置
            BridgeCell enterStair = null;
            int trial = 0;
            Vector2Int enterPos = Vector2Int.zero;
            for (; trial < NUMBER_OF_TRIAL; trial++)
            {
                enterPos = new Vector2Int(Random.Range(1, bridges.GetLength(0)), Random.Range(1, bridges.GetLength(1)));
                enterStair = bridges[enterPos.y, enterPos.x];
                if (enterStair.FloorData.buildType == BuildType.None
                    && enterStair.FloorData.floorType == FloorType.CommonFloor)
                {
                    break;
                }
                enterStair = null;
            }
            if (enterStair is null)
            {
                continue;
            }
            BridgeCell exitStair = null;
            Vector2Int exitPos = Vector2Int.zero;
            for (; trial < NUMBER_OF_TRIAL; trial++)
            {
                exitPos.x = Random.Range(Mathf.Max(0, enterPos.x - lengthOfBridge), Mathf.Min(enterPos.x + lengthOfBridge, bridges.GetLength(0)));
                exitPos.y = Random.Range(Mathf.Max(0, enterPos.y - lengthOfBridge), Mathf.Min(enterPos.y + lengthOfBridge, bridges.GetLength(1)));
                exitStair = bridges[exitPos.y, exitPos.x];
                if (exitStair != enterStair
                    && exitStair.FloorData.isUpperFloor == enterStair.FloorData.isUpperFloor
                    && exitStair.FloorData.buildType == BuildType.None
                    && exitStair.FloorData.floorType == FloorType.CommonFloor)
                {
                    break;
                }
                exitStair = null;
            }
            if (exitStair is null)
            {
                continue;
            }

            //enterからexitに向けて1マスずつ進む
            int moveOnX = enterPos.x - exitPos.x < 0 ? 1 : -1;
            int moveOnY = enterPos.y - exitPos.y < 0 ? 1 : -1;
            List<Vector2Int> posCashe = new List<Vector2Int>(_mapSize.x + _mapSize.y);
            bool err = false;
            Compass nextEnter = Compass.North;
            while (enterPos != exitPos)
            {
                if (enterPos.x == exitPos.x)
                {
                    if (moveOnY < 0)
                    {
                        bridges[enterPos.y, enterPos.x].exit = Compass.South;
                        nextEnter = Compass.North;
                        enterPos.y += moveOnY;
                    }
                    else
                    {
                        bridges[enterPos.y, enterPos.x].exit = Compass.North;
                        nextEnter = Compass.South;
                        enterPos.y += moveOnY;
                    }
                }
                else if (enterPos.y == exitPos.y)
                {
                    if (moveOnX < 0)
                    {
                        bridges[enterPos.y, enterPos.x].exit = Compass.West;
                        nextEnter = Compass.East;
                        enterPos.x += moveOnX;
                    }
                    else
                    {
                        bridges[enterPos.y, enterPos.x].exit = Compass.East;
                        nextEnter = Compass.West;
                        enterPos.x += moveOnX;
                    }
                }
                else
                {
                    if (Random.value < 0.5f)
                    {
                        if (moveOnY < 0)
                        {
                            bridges[enterPos.y, enterPos.x].exit = Compass.South;
                            nextEnter = Compass.North;
                            enterPos.y += moveOnY;
                        }
                        else
                        {
                            bridges[enterPos.y, enterPos.x].exit = Compass.North;
                            nextEnter = Compass.South;
                            enterPos.y += moveOnY;
                        }
                    }
                    else
                    {
                        if (moveOnX < 0)
                        {
                            bridges[enterPos.y, enterPos.x].exit = Compass.West;
                            nextEnter = Compass.East;
                            enterPos.x += moveOnX;
                        }
                        else
                        {
                            bridges[enterPos.y, enterPos.x].exit = Compass.East;
                            nextEnter = Compass.West;
                            enterPos.x += moveOnX;
                        }
                    }
                }

                //すでに通路が定義されている、または、スタート位置よりも高い位置で地面が道の場合は中断
                if (bridges[enterPos.y, enterPos.x].bridgeType != BridgeType.None
                    || (!bridges[enterPos.y, enterPos.x].FloorData.isUpperFloor
                        && bridges[exitPos.y, exitPos.x].FloorData.isUpperFloor
                        && bridges[enterPos.y, enterPos.x].FloorData.floorType != FloorType.CommonFloor))
                {
                    err = true;
                    break;
                }

                bridges[enterPos.y, enterPos.x].enter = nextEnter;

                //対の階段に到達したら離脱
                if (enterPos == exitPos)
                {
                    break;
                }

                posCashe.Add(enterPos);
            }

            //すでに通路が定義されていれば中断
            if (err)
            {
                continue;
            }

            //連絡通路の情報登録
            enterStair.FloorData.buildType = BuildType.BuildLarge;
            exitStair.FloorData.buildType = BuildType.BuildLarge;
            BridgeType bridgeType = BridgeType.LowerRoad;
            if (enterStair.FloorData.isUpperFloor)
            {
                enterStair.bridgeType = BridgeType.UpperStair;
                exitStair.bridgeType = BridgeType.UpperStair;
                bridgeType = BridgeType.UpperRoad;
            }
            else
            {
                enterStair.bridgeType = BridgeType.LowerStair;
                exitStair.bridgeType = BridgeType.LowerStair;
            }
            switch (enterStair.exit)
            {
                case Compass.North:
                    enterStair.enter = Compass.South;
                    break;
                case Compass.South:
                    enterStair.enter = Compass.North;
                    break;
                case Compass.East:
                    enterStair.enter = Compass.West;
                    break;
                case Compass.West:
                    enterStair.enter = Compass.East;
                    break;
            }
            switch (exitStair.enter)
            {
                case Compass.North:
                    exitStair.enter = Compass.South;
                    exitStair.exit = Compass.North;
                    break;
                case Compass.South:
                    exitStair.enter = Compass.North;
                    exitStair.exit = Compass.South;
                    break;
                case Compass.East:
                    exitStair.enter = Compass.West;
                    exitStair.exit = Compass.East;
                    break;
                case Compass.West:
                    exitStair.enter = Compass.East;
                    exitStair.exit = Compass.West;
                    break;
            }

            foreach (Vector2Int pos in posCashe)
            {
                bridges[pos.y, pos.x].FloorData.buildType = BuildType.BuildLarge;
                bridges[pos.y, pos.x].bridgeType = bridgeType;
            }
        }
    }

    /// <summary>パーツ配置</summary>
    void SetParts()
    {
        //構造物配置決めのためのリスト
        List<Vector2Int> buildCandidates = new List<Vector2Int>(_mapSize.x * _mapSize.y);

        /*地形配置*/
        GameObject pref = null;
        for (int y = 0; y < _mapSize.y; y++)
        {
            for (int x = 0; x < _mapSize.x; x++)
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
                                    pref = _curveRoad;
                                    break;
                                //直進
                                case Compass.South:
                                    pref = _straightRoad;
                                    forward = Vector3.left;
                                    break;
                                case Compass.West:
                                    pref = _curveRoad;
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
                                    pref = _curveRoad;
                                    break;
                                case Compass.South:
                                    pref = _curveRoad;
                                    forward = Vector3.right;
                                    break;
                                //直進
                                case Compass.West:
                                    pref = _straightRoad;
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
                                    pref = _straightRoad;
                                    forward = Vector3.left;
                                    break;
                                case Compass.East:
                                    pref = _curveRoad;
                                    forward = Vector3.right;
                                    break;
                                case Compass.West:
                                    pref = _curveRoad;
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
                                    pref = _curveRoad;
                                    forward = Vector3.left;
                                    break;
                                //直進
                                case Compass.East:
                                    pref = _straightRoad;
                                    break;
                                case Compass.South:
                                    pref = _curveRoad;
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
                    pref = _commonFloor;
                }

                //高低差壁配置
                //南隣の区画をチェック
                if (map[y, x].down != null)
                {
                    //その区画の方が高い位置にある
                    if (map[y, x].down.isUpperFloor && !map[y, x].isUpperFloor)
                    {
                        //両区画は道かつ方角の一致する入口と出口がある
                        if ((map[y, x].down.floorType != FloorType.CommonFloor && map[y, x].floorType != FloorType.CommonFloor)
                            && (map[y - 1, x].enter == map[y, x].enter || map[y - 1, x].exit == map[y, x].exit))
                        {
                            forward = Vector3.left;
                            pref = _steepSlopeRoad;

                            //高低差壁作成
                            if (map[y, x].right != null && !map[y, x].right.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_bothSideWall, transform);
                                wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f, 0f, y * UNIT_FOR_FLOOR);
                                wall.transform.forward = Vector3.right;
                            }
                            if (map[y, x].left != null && !map[y, x].left.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_bothSideWall, transform);
                                wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f, 0f, y * UNIT_FOR_FLOOR);
                                wall.transform.forward = Vector3.left;
                            }
                        }
                        else
                        {
                            //高低差壁作成
                            GameObject wallPref = _differentHeightWall;
                            if (pref == _commonFloor)
                            {
                                float rand = Random.value;
                                if (rand > 0.8f)
                                {
                                    if (rand > 0.9f)
                                    {
                                        wallPref = _outerStairLeft;
                                        map[y, x].buildType = BuildType.FloorStair;
                                    }
                                    else
                                    {
                                        wallPref = _outerStairRight;
                                        map[y, x].buildType = BuildType.FloorStair;
                                    }
                                }
                            }
                            GameObject wall = Instantiate(wallPref, transform);
                            wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR, 0f, y * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f);
                            wall.transform.forward = Vector3.back;
                        }
                    }

                    //その区画の方が低い位置にある
                    if (!map[y, x].down.isUpperFloor && map[y, x].isUpperFloor)
                    {
                        //コーナーポストを置く判定
                        if (map[y, x].right != null && !map[y, x].right.isUpperFloor)
                        {
                            GameObject post = Instantiate(_wallCornerPost, transform);
                            post.transform.position = new Vector3(x * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f, 0f, y * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f);
                        }
                        if (map[y, x].left != null && !map[y, x].left.isUpperFloor)
                        {
                            GameObject post = Instantiate(_wallCornerPost, transform);
                            post.transform.position = new Vector3(x * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f, 0f, y * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f);
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
                            pref = _steepSlopeRoad;

                            //高低差壁作成
                            if (map[y, x].right != null && !map[y, x].right.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_bothSideWall, transform);
                                wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f, 0f, y * UNIT_FOR_FLOOR);
                                wall.transform.forward = Vector3.right;
                            }
                            if (map[y, x].left != null && !map[y, x].left.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_bothSideWall, transform);
                                wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f, 0f, y * UNIT_FOR_FLOOR);
                                wall.transform.forward = Vector3.left;
                            }
                        }
                        else
                        {
                            //高低差壁作成
                            GameObject wallPref = _differentHeightWall;
                            if (pref == _commonFloor)
                            {
                                float rand = Random.value;
                                if (rand > 0.8f)
                                {
                                    if (rand > 0.9f)
                                    {
                                        wallPref = _outerStairLeft;
                                    }
                                    else
                                    {
                                        wallPref = _outerStairRight;
                                    }
                                }
                            }
                            GameObject wall = Instantiate(wallPref, transform);
                            wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR, 0f, y * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f);
                        }
                    }

                    //その区画の方が低い位置にある
                    if (!map[y, x].up.isUpperFloor && map[y, x].isUpperFloor)
                    {
                        //コーナーポストを置く判定
                        if (map[y, x].right != null && !map[y, x].right.isUpperFloor)
                        {
                            GameObject post = Instantiate(_wallCornerPost, transform);
                            post.transform.position = new Vector3(x * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f, 0f, y * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f);
                        }
                        if (map[y, x].left != null && !map[y, x].left.isUpperFloor)
                        {
                            GameObject post = Instantiate(_wallCornerPost, transform);
                            post.transform.position = new Vector3(x * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f, 0f, y * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f);
                        }
                    }
                }
                //西隣の区画をチェック
                if (map[y, x].left != null)
                {
                    //その区画の方が高い位置にある
                    if (map[y, x].left.isUpperFloor && !map[y, x].isUpperFloor)
                    {
                        //両区画は道かつ方角の一致する入口と出口がある
                        if ((map[y, x].left.floorType != FloorType.CommonFloor && map[y, x].floorType != FloorType.CommonFloor)
                            && (map[y, x].left.enter == map[y, x].enter || map[y, x].left.exit == map[y, x].exit))
                        {
                            pref = _steepSlopeRoad;

                            //高低差壁作成
                            if (map[y, x].up != null && !map[y, x].up.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_bothSideWall, transform);
                                wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR, 0f, y * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f);
                            }
                            if (map[y, x].down != null && !map[y, x].down.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_bothSideWall, transform);
                                wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR, 0f, y * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f);
                                wall.transform.forward = Vector3.back;
                            }
                        }
                        else
                        {
                            //高低差壁作成
                            GameObject wallPref = _differentHeightWall;
                            if (pref == _commonFloor)
                            {
                                float rand = Random.value;
                                if (rand > 0.8f)
                                {
                                    if (rand > 0.9f)
                                    {
                                        wallPref = _outerStairLeft;
                                    }
                                    else
                                    {
                                        wallPref = _outerStairRight;
                                    }
                                }
                            }
                            GameObject wall = Instantiate(wallPref, transform);
                            wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f, 0f, y * UNIT_FOR_FLOOR);
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
                            pref = _steepSlopeRoad;

                            //高低差壁作成
                            if (map[y, x].up != null && !map[y, x].up.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_bothSideWall, transform);
                                wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR, 0f, y * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f);
                            }
                            if (map[y, x].down != null && !map[y, x].down.isUpperFloor)
                            {
                                GameObject wall = Instantiate(_bothSideWall, transform);
                                wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR, 0f, y * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f);
                                wall.transform.forward = Vector3.back;
                            }
                        }
                        else
                        {
                            //高低差壁作成
                            GameObject wallPref = _differentHeightWall;
                            if (pref == _commonFloor)
                            {
                                float rand = Random.value;
                                if (rand > 0.8f)
                                {
                                    if (rand > 0.9f)
                                    {
                                        wallPref = _outerStairLeft;
                                    }
                                    else
                                    {
                                        wallPref = _outerStairRight;
                                    }
                                }
                            }
                            GameObject wall = Instantiate(wallPref, transform);
                            wall.transform.position = new Vector3(x * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f, 0f, y * UNIT_FOR_FLOOR);
                            wall.transform.forward = Vector3.right;
                        }
                    }
                }

                //地面の生成
                GameObject ins = Instantiate(pref, transform);
                ins.transform.position = new Vector3(x, map[y, x].isUpperFloor ? 0.2f : 0f, y) * UNIT_FOR_FLOOR;
                ins.transform.forward = forward;

                //道路以外は建造物配置候補
                if (map[y, x].floorType == FloorType.CommonFloor)
                {
                    buildCandidates.Add(new Vector2Int(x, y));
                }
            }
        }

        /*連絡通路配置*/
        for (int y = 0; y < bridges.GetLength(0); y++)
        {
            for (int x = 0; x < bridges.GetLength(1); x++)
            {
                bool isContinue = false;

                //置くパーツを指定
                pref = _bridgeStraight;
                Vector3 setPos = Vector3.zero;
                Vector3 forward = Vector3.forward;
                switch (bridges[y, x].bridgeType)
                {
                    case BridgeType.LowerRoad:

                        pref = _bridgeStraight;
                        setPos = new Vector3((x - 0.5f) * UNIT_FOR_BRIDGE, 3.22f, (y - 0.5f) * UNIT_FOR_BRIDGE);
                        break;
                    case BridgeType.UpperRoad:

                        pref = _bridgeStraight;
                        setPos = new Vector3((x - 0.5f) * UNIT_FOR_BRIDGE, 5.22f, (y - 0.5f) * UNIT_FOR_BRIDGE);
                        break;
                    case BridgeType.LowerStair:

                        pref = _bridgeStair;
                        setPos = new Vector3(x - 0.5f, 0f, y - 0.5f) * UNIT_FOR_BRIDGE;
                        break;
                    case BridgeType.UpperStair:

                        pref = _bridgeStair;
                        setPos = new Vector3(x - 0.5f, 0.4f, y - 0.5f) * UNIT_FOR_BRIDGE;
                        break;
                    default:
                        isContinue = true;
                        break;
                }

                if (isContinue) continue;

                switch (bridges[y, x].enter)
                {
                    case Compass.North:
                        switch (bridges[y, x].exit)
                        {
                            case Compass.East:
                                pref = _bridgeCorner;
                                break;
                            //直進
                            case Compass.South:
                                forward = Vector3.back;
                                break;
                            case Compass.West:
                                pref = _bridgeCorner;
                                forward = Vector3.left;
                                break;
                        }
                        break;
                    case Compass.East:
                        switch (bridges[y, x].exit)
                        {
                            case Compass.North:
                                pref = _bridgeCorner;
                                break;
                            case Compass.South:
                                pref = _bridgeCorner;
                                forward = Vector3.right;
                                break;
                            //直進
                            case Compass.West:
                                forward = Vector3.left;
                                break;
                        }
                        break;
                    case Compass.South:
                        switch (bridges[y, x].exit)
                        {
                            //直進
                            case Compass.North:
                                break;
                            case Compass.East:
                                pref = _bridgeCorner;
                                forward = Vector3.right;
                                break;
                            case Compass.West:
                                pref = _bridgeCorner;
                                forward = Vector3.back;
                                break;
                        }
                        break;
                    case Compass.West:
                        switch (bridges[y, x].exit)
                        {
                            case Compass.North:
                                pref = _bridgeCorner;
                                forward = Vector3.left;
                                break;
                            //直進
                            case Compass.East:
                                forward = Vector3.right;
                                break;
                            case Compass.South:
                                pref = _bridgeCorner;
                                forward = Vector3.back;
                                break;
                        }
                        break;
                }

                //確率で連絡通路の柱をつける
                if (bridges[y, x].FloorData.floorType == FloorType.CommonFloor && pref == _bridgeStraight)
                {
                    pref = Random.value < 0.5f ? pref : _bridgeStraightStrut;
                }

                GameObject ins = Instantiate(pref, transform);
                ins.transform.position = setPos;
                ins.transform.forward = forward;
            }
        }

        /*壁配置*/
        //右と上に対して壁の配置を検討
        for (int y = 0; y < _mapSize.y - 1; y++)
        {
            for (int x = 0; x < _mapSize.x - 1; x++)
            {
                //配置する高さ
                float height = 0f;
                //true : 道つながりである
                bool isConectRoad = false;
                //true : 自分が高い位置である
                bool isUpperThisFloor = false;
                //true : 相手が高い位置である
                bool isUpperThatFloor = false;

                //上側を検討
                if (map[y, x].up is not null)
                {
                    pref = null;

                    isConectRoad = (map[y, x].floorType != FloorType.CommonFloor && map[y, x].up.floorType != FloorType.CommonFloor);
                    isUpperThisFloor = map[y, x].isUpperFloor && !map[y, x].up.isUpperFloor;
                    isUpperThatFloor = !map[y, x].isUpperFloor && map[y, x].up.isUpperFloor;

                    //自分がゲーム範囲外で上側が範囲内の時
                    if (map[y, x].region == 0 && map[y, x].up.region > 0)
                    {
                        GameObject ins;

                        pref = _concreteFence;
                        if (isConectRoad)
                        {
                            pref = _roadBlockAreaLimit;
                        }
                        if (map[y, x].isUpperFloor || map[y, x].up.isUpperFloor)
                        {
                            height = 2f;
                        }
                        else if (map[y, x].left.isUpperFloor || map[y, x].left.up.isUpperFloor)
                        {
                            ins = Instantiate(_concreteFenceCornerPost, transform);
                            ins.transform.position = new Vector3(x * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f, 2f, y * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f);
                        }

                        ins = Instantiate(_concreteFenceCornerPost, transform);
                        ins.transform.position = new Vector3(x * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f, height, y * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f);
                    }
                    //自分がゲーム範囲内で上側が範囲外の時
                    else if (map[y, x].region > 0 && map[y, x].up.region == 0)
                    {
                        GameObject ins;

                        pref = _concreteFence;
                        if (isConectRoad)
                        {
                            pref = _roadBlockAreaLimit;
                        }
                        if (map[y, x].isUpperFloor || map[y, x].up.isUpperFloor)
                        {
                            height = 2f;
                        }
                        else if (map[y, x].left.isUpperFloor || map[y, x].left.up.isUpperFloor)
                        {
                            ins = Instantiate(_concreteFenceCornerPost, transform);
                            ins.transform.position = new Vector3(x * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f, 2f, y * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f);
                        }

                        ins = Instantiate(_concreteFenceCornerPost, transform);
                        ins.transform.position = new Vector3(x * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f, height, y * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f);
                    }

                    if (pref)
                    {
                        GameObject ins = Instantiate(pref, transform);
                        ins.transform.position = new Vector3(x * UNIT_FOR_FLOOR, height, y * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f);
                        ins.transform.forward = Vector3.back;
                    }
                }
                //右側を検討
                if (map[y, x].right is not null)
                {
                    pref = null;

                    isConectRoad = (map[y, x].floorType != FloorType.CommonFloor && map[y, x].right.floorType != FloorType.CommonFloor);
                    //&& ((map[y, x].enter == Compass.East && map[y, x].right.exit == Compass.West)
                    //    || (map[y, x].enter == Compass.West && map[y, x].right.exit == Compass.East));
                    isUpperThisFloor = map[y, x].isUpperFloor && !map[y, x].right.isUpperFloor;
                    isUpperThatFloor = !map[y, x].isUpperFloor && map[y, x].right.isUpperFloor;

                    //自分がゲーム範囲外で右側が範囲内の時
                    if (map[y, x].region == 0 && map[y, x].right.region > 0)
                    {
                        GameObject ins;

                        pref = _concreteFence;
                        if (isConectRoad)
                        {
                            pref = _roadBlockAreaLimit;
                        }

                        if (map[y, x].isUpperFloor || map[y, x].right.isUpperFloor)
                        {
                            height = 2f;
                        }
                        else if (map[y, x].down.isUpperFloor || map[y, x].down.right.isUpperFloor)
                        {
                            ins = Instantiate(_concreteFenceCornerPost, transform);
                            ins.transform.position = new Vector3(x * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f, 2f, y * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f);
                        }

                        ins = Instantiate(_concreteFenceCornerPost, transform);
                        ins.transform.position = new Vector3(x * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f, height, y * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f);
                    }
                    //自分がゲーム範囲内で右側が範囲外の時
                    else if (map[y, x].region > 0 && map[y, x].right.region == 0)
                    {
                        GameObject ins;

                        pref = _concreteFence;
                        if (isConectRoad)
                        {
                            pref = _roadBlockAreaLimit;
                        }

                        if (map[y, x].isUpperFloor || map[y, x].right.isUpperFloor)
                        {
                            height = 2f;
                        }
                        else if (map[y, x].down.isUpperFloor || map[y, x].down.right.isUpperFloor)
                        {
                            ins = Instantiate(_concreteFenceCornerPost, transform);
                            ins.transform.position = new Vector3(x * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f, 2f, y * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f);
                        }

                        ins = Instantiate(_concreteFenceCornerPost, transform);
                        ins.transform.position = new Vector3(x * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f, height, y * UNIT_FOR_FLOOR - UNIT_FOR_FLOOR / 2f);
                    }

                    if (pref)
                    {
                        GameObject ins = Instantiate(pref, transform);
                        ins.transform.position = new Vector3(x * UNIT_FOR_FLOOR + UNIT_FOR_FLOOR / 2f, height, y * UNIT_FOR_FLOOR);
                        ins.transform.forward = Vector3.right;
                    }
                }
            }
        }


        /*建造物配置*/
        int numberOfCandidate = (int)(buildCandidates.Count * 0.75f);
        for (int i = 0; i < numberOfCandidate; i++)
        {
            //ランダムで候補地を指定
            int targetIndex = (int)Random.Range(1f, buildCandidates.Count) - 1;
            Vector2Int candidate = buildCandidates[targetIndex];

            //建造物が建てられていない、かつ、道ではない
            if (map[candidate.y, candidate.x].buildType == BuildType.None
                && map[candidate.y, candidate.x].floorType == FloorType.CommonFloor)
            {
                map[candidate.y, candidate.x].CheckAroundBuildFlag();
                for (int k = 0; k < 5; k++)
                {
                    MapCell[] cells = null;
                    Vector3 forward = Vector3.forward;
                    Vector3 centerPos = new Vector3(candidate.x, 0f, candidate.y);
                    float rand = Random.value;
                    if (rand > 0.75f) { forward = Vector3.right; }
                    else if (rand > 0.5f) { forward = Vector3.back; }
                    else if (rand > 0.25f) { forward = Vector3.left; }
                    (MapCell[], Vector3) data;
                    switch (k)
                    {
                        //3x3の探索
                        case 0:
                            cells = map[candidate.y, candidate.x].GetField3x3();
                            pref = _buldings3x3[(int)Random.Range(0f, _buldings3x3.Length - 0.01f)];
                            break;
                        //3x2の探索
                        case 1:
                            data = map[candidate.y, candidate.x].GetField3x2();
                            cells = data.Item1;
                            forward = data.Item2;
                            centerPos += forward * 0.5f;
                            pref = _buldings3x2[(int)Random.Range(0f, _buldings3x2.Length - 0.01f)];
                            break;
                        //2x2の探索
                        case 2:
                            data = map[candidate.y, candidate.x].GetField2x2();
                            cells = data.Item1;
                            centerPos += data.Item2 * 0.5f;
                            pref = _buldings2x1[(int)Random.Range(0f, _buldings2x1.Length - 0.01f)];
                            break;
                        //2x1の探索
                        case 3:
                            data = map[candidate.y, candidate.x].GetField2x1();
                            cells = data.Item1;
                            forward = data.Item2;
                            centerPos += forward * 0.5f;
                            pref = _buldings2x1[(int)Random.Range(0f, _buldings2x1.Length - 0.01f)];
                            break;
                        //1x1の探索
                        case 4:
                            cells = new MapCell[] { map[candidate.y, candidate.x] };
                            pref = _buldings1x1[(int)Random.Range(0f, _buldings1x1.Length - 0.01f)];
                            break;
                        default: break;
                    }

                    if (cells is not null)
                    {
                        GameObject build = Instantiate(pref, transform);
                        build.transform.position = new Vector3(centerPos.x, map[candidate.y, candidate.x].isUpperFloor ? 0.2f : 0f, centerPos.z) * UNIT_FOR_FLOOR;
                        build.transform.forward = forward;

                        foreach (MapCell cell in cells)
                        {
                            cell.buildType = BuildType.BuildLarge;
                        }

                        break;
                    }
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

        /// <summary>その区画の建造物の種類</summary>
        public BuildType buildType = BuildType.None;

        /// <summary>柵で区切る区画集合の番号（0はゲームステージ外）</summary>
        public byte region = 0;

        /// <summary>建物建てるスペースの存在フラグ集</summary>
        byte buildBitFlags = 0;


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


        /// <summary>周辺の建築状況を確認しフラグにまとめる</summary>
        public void CheckAroundBuildFlag()
        {
            buildBitFlags = 0;
            buildBitFlags += (byte)(upLeft != null && upLeft.IsBuildable(isUpperFloor, region) ? 1 : 0);
            buildBitFlags += (byte)(up != null && up.IsBuildable(isUpperFloor, region) ? 2 : 0);
            buildBitFlags += (byte)(upRight != null && upRight.IsBuildable(isUpperFloor, region) ? 4 : 0);
            buildBitFlags += (byte)(left != null && left.IsBuildable(isUpperFloor, region) ? 8 : 0);
            buildBitFlags += (byte)(right != null && right.IsBuildable(isUpperFloor, region) ? 16 : 0);
            buildBitFlags += (byte)(downLeft != null && downLeft.IsBuildable(isUpperFloor, region) ? 32 : 0);
            buildBitFlags += (byte)(down != null && down.IsBuildable(isUpperFloor, region) ? 64 : 0);
            buildBitFlags += (byte)(downRight != null && downRight.IsBuildable(isUpperFloor, region) ? 128 : 0);
        }

        /// <summary>隣接地に建物を建てられるかを確かめる</summary>
        /// <param name="isUpperFloor">隣接元の床位置</param>
        /// <returns>true : 建てられる</returns>
        public bool IsBuildable(bool isUpperFloor, byte region)
        {
            return this.region == region
                    && this.isUpperFloor == isUpperFloor
                    && floorType == FloorType.CommonFloor
                    && buildType == BuildType.None;
        }

        /// <summary>未占有の3×3の地形を探す</summary>
        /// <returns>あれば対象の全区画</returns>
        public MapCell[] GetField3x3()
        {
            if (buildBitFlags > 254)
            {
                MapCell[] cells = { this, upLeft, up, upRight, left, right, downLeft, down, downRight };
                return cells;
            }
            return null;
        }

        /// <summary>未占有の3×2の地形を探す</summary>
        /// <returns>あれば対象の全区画</returns>
        public (MapCell[], Vector3) GetField3x2()
        {
            MapCell[] cells = new MapCell[6];
            switch (buildBitFlags)
            {
                case 63:
                    cells[0] = this; cells[1] = upLeft; cells[2] = up; cells[3] = upRight; cells[4] = left; cells[5] = right;
                    return (cells, Vector3.forward);

                case 214:
                    cells[0] = this; cells[1] = up; cells[2] = upRight; cells[3] = right; cells[4] = down; cells[5] = downRight;
                    return (cells, Vector3.right);

                case 248:
                    cells[0] = this; cells[1] = left; cells[2] = right; cells[3] = downLeft; cells[4] = down; cells[5] = downRight;
                    return (cells, Vector3.back);

                case 107:
                    cells[0] = this; cells[1] = upLeft; cells[2] = up; cells[3] = left; cells[4] = downLeft; cells[5] = down;
                    return (cells, Vector3.left);
                default: break;
            }

            return (null, Vector3.zero);
        }

        /// <summary>未占有の2×2の地形を探す</summary>
        /// <returns>あれば対象の全区画</returns>
        public (MapCell[], Vector3) GetField2x2()
        {
            MapCell[] cells = new MapCell[4];
            switch (buildBitFlags)
            {
                case 11:
                    cells[0] = this; cells[1] = upLeft; cells[2] = up; cells[3] = left;
                    return (cells, Vector3.forward + Vector3.left);
                case 22:
                    cells[0] = this; cells[1] = up; cells[2] = upRight; cells[3] = right;
                    return (cells, Vector3.forward + Vector3.right);
                case 104:
                    cells[0] = this; cells[1] = left; cells[2] = downLeft; cells[3] = down;
                    return (cells, Vector3.back + Vector3.left);
                case 208:
                    cells[0] = this; cells[1] = right; cells[2] = down; cells[3] = downRight;
                    return (cells, Vector3.back + Vector3.right);
                default: break;
            }

            return (null, Vector3.zero);
        }

        /// <summary>未占有の2×1の地形を探す</summary>
        /// <returns>あれば対象の全区画</returns>
        public (MapCell[], Vector3) GetField2x1()
        {
            MapCell[] cells = new MapCell[2];
            switch (buildBitFlags)
            {
                case 2:
                    cells[0] = this; cells[1] = up;
                    return (cells, Vector3.forward);
                case 8:
                    cells[0] = this; cells[1] = left;
                    return (cells, Vector3.left);
                case 16:
                    cells[0] = this; cells[1] = right;
                    return (cells, Vector3.right);
                case 64:
                    cells[0] = this; cells[1] = down;
                    return (cells, Vector3.back);
                default: break;
            }
            return (null, Vector3.zero);
        }


        public MapCell(FloorType floorType = FloorType.CommonFloor)
        {
            this.floorType = floorType;
        }
    }

    /// <summary>橋や配管の1マスの情報</summary>
    class BridgeCell
    {
        /// <summary>道の入口</summary>
        public Compass enter = Compass.North;

        /// <summary>道の出口</summary>
        public Compass exit = Compass.South;

        /// <summary>配置する橋や配管のタイプ</summary>
        public BridgeType bridgeType = BridgeType.None;

        /// <summary>該当の床情報</summary>
        MapCell floorData = null;




        /// <summary>左上の区画の情報</summary>
        public BridgeCell upLeft = null;

        /// <summary>真上の区画の情報</summary>
        public BridgeCell up = null;

        /// <summary>右上の区画の情報</summary>
        public BridgeCell upRight = null;

        /// <summary>真左の区画の情報</summary>
        public BridgeCell left = null;

        /// <summary>真右の区画の情報</summary>
        public BridgeCell right = null;

        /// <summary>左下の区画の情報</summary>
        public BridgeCell downLeft = null;

        /// <summary>真下の区画の情報</summary>
        public BridgeCell down = null;

        /// <summary>右下の区画の情報</summary>
        public BridgeCell downRight = null;


        /// <summary>該当の床情報</summary>
        public MapCell FloorData { get => floorData; }


        public BridgeCell(MapCell floorInfo)
        {
            floorData = floorInfo;
        }
    }
}