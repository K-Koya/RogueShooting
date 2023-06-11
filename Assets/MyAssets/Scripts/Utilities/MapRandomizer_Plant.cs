using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(BoxCollider))]
public class MapRandomizer_Plant : MonoBehaviour, IStartLocation
{
    /// <summary>��{���s��</summary>
    int NUMBER_OF_TRIAL = 40;

    /// <summary>���̃}�X��1�ӂ̒���</summary>
    float UNIT_FOR_FLOOR = 10f;

    /// <summary>�󒆉�L��1�ӂ̒���</summary>
    float UNIT_FOR_BRIDGE = 5f;

    /// <summary>�Q�[������G���A�ƂȂ�O������̋���</summary>
    byte GAME_AREA_AROUND_DISTANCE = 7;


    /// <summary>�X�^�[�g�n�_�ɂȂ�t���A���W</summary>
    Vector2Int _startFloor = new Vector2Int();

    /// <summary>�X�^�[�g�n�_�ɂȂ�t���A�̍���</summary>
    float _startFloorHeight = 0f;


    /// <summary>�X�^�[�g�n�_�ɂȂ�t���A���W</summary>
    public Vector3 StartFloorBasePosition => new Vector3(_startFloor.x * UNIT_FOR_FLOOR, _startFloorHeight, _startFloor.y * UNIT_FOR_FLOOR);


    [SerializeField, Tooltip("�}�X�̐�(���15�}�X�ȏ�)")]
    Vector2Int _mapSize = new Vector2Int(15, 15);

    [SerializeField, Tooltip("true : �����V�[�h���w�肷��")]
    bool _useSeed = false;

    [SerializeField, Tooltip("�����V�[�h")]
    int _seed = 10;


    /// <summary>�G���A�O�ɍs���Ȃ��悤�ɂ���R���C�_�[</summary>
    BoxCollider[] _areaLimits = null;


    [Header("�n�ʃv���n�u")]
    [SerializeField, Tooltip("���ʒn�ʋ��")]
    GameObject _commonFloor = null;

    [SerializeField, Tooltip("���i���H���")]
    GameObject _straightRoad = null;

    [SerializeField, Tooltip("�}�ȓo����")]
    GameObject _steepSlopeRoad = null;

    [SerializeField, Tooltip("�ɂ��o����")]
    GameObject _gentleSlopeRoad = null;

    [SerializeField, Tooltip("�Ȑ����H���")]
    GameObject _curveRoad = null;

    [SerializeField, Tooltip("�K�i���H�����񂾕��ʒn�ʋ��")]
    GameObject _commonFloorInnerStair = null;


    [Header("���፷�ǃv���n�u")]
    [SerializeField, Tooltip("���፷���߂̕� ����")]
    GameObject _differentHeightWall = null;

    [SerializeField, Tooltip("���፷���߂̕� ���ʂ���")]
    GameObject _bothSideWall = null;

    [SerializeField, Tooltip("�ǂ̊p�𖄂߂钌")]
    GameObject _wallCornerPost = null;

    [SerializeField, Tooltip("�ǂƖ��ߍ��܂ꂽ���K�i")]
    GameObject _innerStairLeft = null;

    [SerializeField, Tooltip("�ǂƖ��ߍ��܂ꂽ�E�K�i")]
    GameObject _innerStairRight = null;

    [SerializeField, Tooltip("�ǂƍ����ɂ���o�����K�i")]
    GameObject _outerStairLeft = null;

    [SerializeField, Tooltip("�ǂƉE���ɂ���o�����K�i")]
    GameObject _outerStairRight = null;


    [Header("�t�F���X�v���n�u")]
    [SerializeField, Tooltip("�R���N���[�g�̕�")]
    GameObject _concreteFence = null;

    [SerializeField, Tooltip("�R���N���[�g�ǂ̊p�𖄂߂钌")]
    GameObject _concreteFenceCornerPost = null;

    [SerializeField, Tooltip("���Ԃ̕�")]
    GameObject _latticeFence = null;

    [SerializeField, Tooltip("�G���A�O�ƂȂ铹�~��")]
    GameObject _roadBlockAreaLimit = null;


    [Header("�A���ʘH�v���n�u")]
    [SerializeField, Tooltip("�K�i")]
    GameObject _bridgeStair = null;

    [SerializeField, Tooltip("���i�ʘH")]
    GameObject _bridgeStraight = null;

    [SerializeField, Tooltip("�x���t�����i�ʘH")]
    GameObject _bridgeStraightStrut = null;

    [SerializeField, Tooltip("�Ȑi�ʘH")]
    GameObject _bridgeCorner = null;


    [Header("�z�ǃv���n�u")]
    [SerializeField, Tooltip("��{����z��")]
    GameObject _pipeTwoStraight = null;

    [SerializeField, Tooltip("��{����㉺�z��")]
    GameObject _pipeTwoHigher = null;


    [Header("�����v���n�u")]
    [SerializeField, Tooltip("4�~3�̓y�n���g������")]
    GameObject[] _buldings4x3 = null;

    [SerializeField, Tooltip("3�~3�̓y�n���g������")]
    GameObject[] _buldings3x3 = null;

    [SerializeField, Tooltip("3�~2�̓y�n���g������")]
    GameObject[] _buldings3x2 = null;

    [SerializeField, Tooltip("2�~2�̓y�n���g������")]
    GameObject[] _buldings2x2 = null;

    [SerializeField, Tooltip("2�~1�̓y�n���g������")]
    GameObject[] _buldings2x1 = null;

    [SerializeField, Tooltip("1�̓y�n���g������")]
    GameObject[] _buldings1x1 = null;




    /// <summary>�}�b�v�e�[�u��</summary>
    MapCell[,] map = null;

    /// <summary>���̃}�b�v�e�[�u��</summary>
    BridgeCell[,] bridges = null;


    /// <summary>���p</summary>
    public enum Compass
    {
        North = 1,
        East = 2,
        South = 3,
        West = 4,
    }

    /// <summary>�z�u������^�C�v</summary>
    public enum FloorType : byte
    {
        CommonFloor = 0,
        StraightRoad = 1,
        SteepSlopeRoad = 2,
        GentleSlopeRoad = 3,
        CurveRoad = 4,
    }

    /// <summary>�z�u���錚�����^�C�v</summary>
    public enum BuildType : byte
    {
        None = 0,
        FloorStair,
        BuildSmall,
        BuildLarge,
    }

    /// <summary>�z�u����󒆒ʘH�^�C�v</summary>
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
        /*�����w��*/
        if (!_useSeed)
        {
            //�V�[�h�l����
            _seed = Random.Range(int.MinValue, int.MaxValue);
        }
        Random.InitState(_seed);


        Initialize();

        SetHeightDiff();
        SetRoad();
        SetBridge();
        SetParts();

        //BoxCollider col = GetComponent<BoxCollider>();

        /*���������*/
        map = null;
        bridges = null;
    }

    /// <summary>�e�[�u��������</summary>
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

        /*�����N�ݒ�*/
        //�n��
        for (int y = 0; y < _mapSize.y; y++)
        {
            for (int x = 0; x < _mapSize.x; x++)
            {
                //�^���擾
                if (y > 0)
                {
                    map[y, x].down = map[y - 1, x];

                    //�����擾
                    if (x > 0)
                    {
                        map[y, x].downLeft = map[y - 1, x - 1];
                    }
                    //�E���擾
                    if (x < _mapSize.x - 1)
                    {
                        map[y, x].downRight = map[y - 1, x + 1];
                    }
                }
                //�^���擾
                if (x > 0)
                {
                    map[y, x].left = map[y, x - 1];
                }
                //�^�E�擾
                if (x < _mapSize.x - 1)
                {
                    map[y, x].right = map[y, x + 1];
                }
                //�^��擾
                if (y < _mapSize.y - 1)
                {
                    map[y, x].up = map[y + 1, x];

                    //����擾
                    if (x > 0)
                    {
                        map[y, x].upLeft = map[y + 1, x - 1];
                    }
                    //�E��擾
                    if (x < _mapSize.x - 1)
                    {
                        map[y, x].upRight = map[y + 1, x + 1];
                    }
                }
            }
        }
        //�A���ʘH
        for (int y = 0; y < bridges.GetLength(0); y++)
        {
            for (int x = 0; x < bridges.GetLength(1); x++)
            {
                //�^���擾
                if (y > 0)
                {
                    bridges[y, x].down = bridges[y - 1, x];

                    //�����擾
                    if (x > 0)
                    {
                        bridges[y, x].downLeft = bridges[y - 1, x - 1];
                    }
                    //�E���擾
                    if (x < bridges.GetLength(1) - 1)
                    {
                        bridges[y, x].downRight = bridges[y - 1, x + 1];
                    }
                }
                //�^���擾
                if (x > 0)
                {
                    bridges[y, x].left = bridges[y, x - 1];
                }
                //�^�E�擾
                if (x < bridges.GetLength(1) - 1)
                {
                    bridges[y, x].right = bridges[y, x + 1];
                }
                //�^��擾
                if (y < bridges.GetLength(0) - 1)
                {
                    bridges[y, x].up = bridges[y + 1, x];

                    //����擾
                    if (x > 0)
                    {
                        bridges[y, x].upLeft = bridges[y + 1, x - 1];
                    }
                    //�E��擾
                    if (x < bridges.GetLength(1) - 1)
                    {
                        bridges[y, x].upRight = bridges[y + 1, x + 1];
                    }
                }
            }
        }

        //�G���A�l���̕ǒ���
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

    /// <summary>���፷�\��</summary>
    void SetHeightDiff()
    {
        //�C�ӂ̃}�b�v4���_��I��
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

        //�w�肵���l���p����w�肵�������֏��グ����
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

    /// <summary>���H�w��</summary>
    void SetRoad()
    {
        //�}�b�v�̒����t�߂̂ǂ������X�^�[�g�ʒu�Ɏw��
        int startX = (int)Random.Range(_mapSize.x * 0.4f, _mapSize.x * 0.6f);
        int startY = (int)Random.Range(_mapSize.y * 0.4f, _mapSize.y * 0.6f);

        //������k���@�肷��������w��
        Compass[] fowards = new Compass[2];
        float rand = Random.value;
        //����͖k������
        if(rand < 0.5f)
        {
            fowards[0] = Compass.North;
            //��������͓������
            if (rand < 0.25f)
            {
                fowards[1] = Compass.South;
            }
            //��������͐�������
            else
            {
                fowards[1] = Compass.West;
            }
        }
        //����͓�������
        else
        {
            fowards[0] = Compass.East;
            //��������͓������
            if (rand < 0.75f)
            {
                fowards[1] = Compass.South;
            }
            //��������͐�������
            else
            {
                fowards[1] = Compass.West;
            }
        }

        //2�����Ɍ��@��
        map[startY, startX].enter = fowards[1];
        map[startY, startX].exit = fowards[0];
        foreach (Compass f in fowards)
        {
            MapCell current = null;
            Compass advanceDirection = Compass.North;
            int[] advanceLimit = new int[4];

            //�@��O�ɉ�����
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

            //���@��
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
                    //�E�T�C�h�Ɍ����Đi�s
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
                    //���T�C�h�Ɍ����Đi�s
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


                    //���Ɍ���������
                    switch (goNext)
                    {
                        //�k������
                        case Compass.North:
                            if (current.up.floorType == FloorType.CommonFloor && advanceLimit[0] > 0)
                            {
                                if (isCurve)
                                {
                                    current.floorType = FloorType.CurveRoad;
                                }

                                //���̋�悪��ŁA���̋�悪���̏ꍇ
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
                                //���̋�悪���ŁA���̋�悪��̏ꍇ
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
                        //�������
                        case Compass.South:
                            if (current.down.floorType == FloorType.CommonFloor && advanceLimit[2] > 0)
                            {
                                if (isCurve)
                                {
                                    current.floorType = FloorType.CurveRoad;
                                }

                                //���̋�悪��ŁA���̋�悪���̏ꍇ
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
                                //���̋�悪���ŁA���̋�悪��̏ꍇ
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
                        //��������
                        case Compass.East:
                            if (current.right.floorType == FloorType.CommonFloor && advanceLimit[1] > 0)
                            {
                                if (isCurve)
                                {
                                    current.floorType = FloorType.CurveRoad;
                                }

                                //���̋�悪��ŁA���̋�悪���̏ꍇ
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
                                //���̋�悪���ŁA���̋�悪��̏ꍇ
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
                        //������
                        case Compass.West:
                            if (current.left.floorType == FloorType.CommonFloor && advanceLimit[3] > 0)
                            {
                                if (isCurve)
                                {
                                    current.floorType = FloorType.CurveRoad;
                                }

                                //���̋�悪��ŁA���̋�悪���̏ꍇ
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
                                //���̋�悪���ŁA���̋�悪��̏ꍇ
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

                //�ǂ����}�b�v�̒[�ɓ��B���Ă���Η��E
                if (current.up == null || current.down == null
                    || current.right == null || current.left == null)
                {
                    //�Ō�̓��̏o�����O���Ɏw��
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

        //�X�^�[�g�ʒu�o�^
        _startFloor = new Vector2Int(startX, startY);
        _startFloorHeight = map[startY, startX].isUpperFloor ? 2f : 0f;
    }

    /// <summary>�A���ʘH�w��</summary>
    void SetBridge()
    {
        //�z�u���s�񐔂�����
        int ratioBase = (_mapSize.x + _mapSize.y) / 2;
        int numberOfTrySetBridge = (int)Random.Range(ratioBase * 0.5f, ratioBase * 1.5f);

        //�A���ʘH�̍ő�͈͂��w��
        int lengthOfBridge = (_mapSize.x + _mapSize.y) / 4;

        //�z�u�����s �ł��Ȃ������ꍇ�͒��f���ĉ񐔂�������
        for (int i = 0; i < numberOfTrySetBridge; i++)
        {
            //�K�i����Z�b�g�Ƃ��ē��������ɔz�u
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

            //enter����exit�Ɍ�����1�}�X���i��
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

                //���łɒʘH����`����Ă���A�܂��́A�X�^�[�g�ʒu���������ʒu�Œn�ʂ����̏ꍇ�͒��f
                if (bridges[enterPos.y, enterPos.x].bridgeType != BridgeType.None
                    || (!bridges[enterPos.y, enterPos.x].FloorData.isUpperFloor
                        && bridges[exitPos.y, exitPos.x].FloorData.isUpperFloor
                        && bridges[enterPos.y, enterPos.x].FloorData.floorType != FloorType.CommonFloor))
                {
                    err = true;
                    break;
                }

                bridges[enterPos.y, enterPos.x].enter = nextEnter;

                //�΂̊K�i�ɓ��B�����痣�E
                if (enterPos == exitPos)
                {
                    break;
                }

                posCashe.Add(enterPos);
            }

            //���łɒʘH����`����Ă���Β��f
            if (err)
            {
                continue;
            }

            //�A���ʘH�̏��o�^
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

    /// <summary>�p�[�c�z�u</summary>
    void SetParts()
    {
        //�\�����z�u���߂̂��߂̃��X�g
        List<Vector2Int> buildCandidates = new List<Vector2Int>(_mapSize.x * _mapSize.y);

        /*�n�`�z�u*/
        GameObject pref = null;
        for (int y = 0; y < _mapSize.y; y++)
        {
            for (int x = 0; x < _mapSize.x; x++)
            {
                Vector3 forward = Vector3.forward;
                if (map[y, x].floorType != FloorType.CommonFloor)
                {
                    //�����Əo���̈ʒu�֌W���瓹�H�̎�ނƒu�������w��
                    switch (map[y, x].enter)
                    {
                        //�k������
                        case Compass.North:
                            switch (map[y, x].exit)
                            {
                                case Compass.East:
                                    pref = _curveRoad;
                                    break;
                                //���i
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
                        //��������
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
                                //���i
                                case Compass.West:
                                    pref = _straightRoad;
                                    break;
                                default: break;
                            }
                            break;
                        //�삪����
                        case Compass.South:
                            switch (map[y, x].exit)
                            {
                                //���i
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
                        //��������
                        case Compass.West:
                            switch (map[y, x].exit)
                            {
                                case Compass.North:
                                    pref = _curveRoad;
                                    forward = Vector3.left;
                                    break;
                                //���i
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

                //���፷�ǔz�u
                //��ׂ̋����`�F�b�N
                if (map[y, x].down != null)
                {
                    //���̋��̕��������ʒu�ɂ���
                    if (map[y, x].down.isUpperFloor && !map[y, x].isUpperFloor)
                    {
                        //�����͓������p�̈�v��������Əo��������
                        if ((map[y, x].down.floorType != FloorType.CommonFloor && map[y, x].floorType != FloorType.CommonFloor)
                            && (map[y - 1, x].enter == map[y, x].enter || map[y - 1, x].exit == map[y, x].exit))
                        {
                            forward = Vector3.left;
                            pref = _steepSlopeRoad;

                            //���፷�Ǎ쐬
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
                            //���፷�Ǎ쐬
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

                    //���̋��̕����Ⴂ�ʒu�ɂ���
                    if (!map[y, x].down.isUpperFloor && map[y, x].isUpperFloor)
                    {
                        //�R�[�i�[�|�X�g��u������
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
                //�k�ׂ̋����`�F�b�N
                if (map[y, x].up != null)
                {
                    //���̋��̕��������ʒu�ɂ���
                    if (map[y, x].up.isUpperFloor && !map[y, x].isUpperFloor)
                    {
                        //�����͓������p�̈�v��������Əo��������
                        if ((map[y, x].up.floorType != FloorType.CommonFloor && map[y, x].floorType != FloorType.CommonFloor)
                            && (map[y, x].up.enter == map[y, x].enter || map[y, x].up.exit == map[y, x].exit))
                        {
                            forward = Vector3.right;
                            pref = _steepSlopeRoad;

                            //���፷�Ǎ쐬
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
                            //���፷�Ǎ쐬
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

                    //���̋��̕����Ⴂ�ʒu�ɂ���
                    if (!map[y, x].up.isUpperFloor && map[y, x].isUpperFloor)
                    {
                        //�R�[�i�[�|�X�g��u������
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
                //���ׂ̋����`�F�b�N
                if (map[y, x].left != null)
                {
                    //���̋��̕��������ʒu�ɂ���
                    if (map[y, x].left.isUpperFloor && !map[y, x].isUpperFloor)
                    {
                        //�����͓������p�̈�v��������Əo��������
                        if ((map[y, x].left.floorType != FloorType.CommonFloor && map[y, x].floorType != FloorType.CommonFloor)
                            && (map[y, x].left.enter == map[y, x].enter || map[y, x].left.exit == map[y, x].exit))
                        {
                            pref = _steepSlopeRoad;

                            //���፷�Ǎ쐬
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
                            //���፷�Ǎ쐬
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
                //���ׂ̋����`�F�b�N
                if (map[y, x].right != null)
                {
                    //���̋��̕��������ʒu�ɂ���
                    if (map[y, x].right.isUpperFloor && !map[y, x].isUpperFloor)
                    {
                        //�����͓������p�̈�v��������Əo��������
                        if ((map[y, x].right.floorType != FloorType.CommonFloor && map[y, x].floorType != FloorType.CommonFloor)
                            && (map[y, x].right.enter == map[y, x].enter || map[y, x].right.exit == map[y, x].exit))
                        {
                            forward = Vector3.back;
                            pref = _steepSlopeRoad;

                            //���፷�Ǎ쐬
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
                            //���፷�Ǎ쐬
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

                //�n�ʂ̐���
                GameObject ins = Instantiate(pref, transform);
                ins.transform.position = new Vector3(x, map[y, x].isUpperFloor ? 0.2f : 0f, y) * UNIT_FOR_FLOOR;
                ins.transform.forward = forward;

                //���H�ȊO�͌������z�u���
                if (map[y, x].floorType == FloorType.CommonFloor)
                {
                    buildCandidates.Add(new Vector2Int(x, y));
                }
            }
        }

        /*�A���ʘH�z�u*/
        for (int y = 0; y < bridges.GetLength(0); y++)
        {
            for (int x = 0; x < bridges.GetLength(1); x++)
            {
                bool isContinue = false;

                //�u���p�[�c���w��
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
                            //���i
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
                            //���i
                            case Compass.West:
                                forward = Vector3.left;
                                break;
                        }
                        break;
                    case Compass.South:
                        switch (bridges[y, x].exit)
                        {
                            //���i
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
                            //���i
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

                //�m���ŘA���ʘH�̒�������
                if (bridges[y, x].FloorData.floorType == FloorType.CommonFloor && pref == _bridgeStraight)
                {
                    pref = Random.value < 0.5f ? pref : _bridgeStraightStrut;
                }

                GameObject ins = Instantiate(pref, transform);
                ins.transform.position = setPos;
                ins.transform.forward = forward;
            }
        }

        /*�ǔz�u*/
        //�E�Ə�ɑ΂��ĕǂ̔z�u������
        for (int y = 0; y < _mapSize.y - 1; y++)
        {
            for (int x = 0; x < _mapSize.x - 1; x++)
            {
                //�z�u���鍂��
                float height = 0f;
                //true : ���Ȃ���ł���
                bool isConectRoad = false;
                //true : �����������ʒu�ł���
                bool isUpperThisFloor = false;
                //true : ���肪�����ʒu�ł���
                bool isUpperThatFloor = false;

                //�㑤������
                if (map[y, x].up is not null)
                {
                    pref = null;

                    isConectRoad = (map[y, x].floorType != FloorType.CommonFloor && map[y, x].up.floorType != FloorType.CommonFloor);
                    isUpperThisFloor = map[y, x].isUpperFloor && !map[y, x].up.isUpperFloor;
                    isUpperThatFloor = !map[y, x].isUpperFloor && map[y, x].up.isUpperFloor;

                    //�������Q�[���͈͊O�ŏ㑤���͈͓��̎�
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
                    //�������Q�[���͈͓��ŏ㑤���͈͊O�̎�
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
                //�E��������
                if (map[y, x].right is not null)
                {
                    pref = null;

                    isConectRoad = (map[y, x].floorType != FloorType.CommonFloor && map[y, x].right.floorType != FloorType.CommonFloor);
                    //&& ((map[y, x].enter == Compass.East && map[y, x].right.exit == Compass.West)
                    //    || (map[y, x].enter == Compass.West && map[y, x].right.exit == Compass.East));
                    isUpperThisFloor = map[y, x].isUpperFloor && !map[y, x].right.isUpperFloor;
                    isUpperThatFloor = !map[y, x].isUpperFloor && map[y, x].right.isUpperFloor;

                    //�������Q�[���͈͊O�ŉE�����͈͓��̎�
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
                    //�������Q�[���͈͓��ŉE�����͈͊O�̎�
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


        /*�������z�u*/
        int numberOfCandidate = (int)(buildCandidates.Count * 0.75f);
        for (int i = 0; i < numberOfCandidate; i++)
        {
            //�����_���Ō��n���w��
            int targetIndex = (int)Random.Range(1f, buildCandidates.Count) - 1;
            Vector2Int candidate = buildCandidates[targetIndex];

            //�����������Ă��Ă��Ȃ��A���A���ł͂Ȃ�
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
                        //3x3�̒T��
                        case 0:
                            cells = map[candidate.y, candidate.x].GetField3x3();
                            pref = _buldings3x3[(int)Random.Range(0f, _buldings3x3.Length - 0.01f)];
                            break;
                        //3x2�̒T��
                        case 1:
                            data = map[candidate.y, candidate.x].GetField3x2();
                            cells = data.Item1;
                            forward = data.Item2;
                            centerPos += forward * 0.5f;
                            pref = _buldings3x2[(int)Random.Range(0f, _buldings3x2.Length - 0.01f)];
                            break;
                        //2x2�̒T��
                        case 2:
                            data = map[candidate.y, candidate.x].GetField2x2();
                            cells = data.Item1;
                            centerPos += data.Item2 * 0.5f;
                            pref = _buldings2x1[(int)Random.Range(0f, _buldings2x1.Length - 0.01f)];
                            break;
                        //2x1�̒T��
                        case 3:
                            data = map[candidate.y, candidate.x].GetField2x1();
                            cells = data.Item1;
                            forward = data.Item2;
                            centerPos += forward * 0.5f;
                            pref = _buldings2x1[(int)Random.Range(0f, _buldings2x1.Length - 0.01f)];
                            break;
                        //1x1�̒T��
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


    /// <summary>�}�b�v1�}�X�̏��</summary>
    class MapCell
    {
        /// <summary>���̓���</summary>
        public Compass enter = Compass.North;

        /// <summary>���̏o��</summary>
        public Compass exit = Compass.South;

        /// <summary>���̎��</summary>
        public FloorType floorType = FloorType.CommonFloor;

        /// <summary>true : ��i������</summary>
        public bool isUpperFloor = false;

        /// <summary>���̋��̌������̎��</summary>
        public BuildType buildType = BuildType.None;

        /// <summary>��ŋ�؂���W���̔ԍ��i0�̓Q�[���X�e�[�W�O�j</summary>
        public byte region = 0;

        /// <summary>�������Ă�X�y�[�X�̑��݃t���O�W</summary>
        byte buildBitFlags = 0;


        /// <summary>����̋��̏��</summary>
        public MapCell upLeft = null;

        /// <summary>�^��̋��̏��</summary>
        public MapCell up = null;

        /// <summary>�E��̋��̏��</summary>
        public MapCell upRight = null;

        /// <summary>�^���̋��̏��</summary>
        public MapCell left = null;

        /// <summary>�^�E�̋��̏��</summary>
        public MapCell right = null;

        /// <summary>�����̋��̏��</summary>
        public MapCell downLeft = null;

        /// <summary>�^���̋��̏��</summary>
        public MapCell down = null;

        /// <summary>�E���̋��̏��</summary>
        public MapCell downRight = null;


        /// <summary>���ӂ̌��z�󋵂��m�F���t���O�ɂ܂Ƃ߂�</summary>
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

        /// <summary>�אڒn�Ɍ��������Ă��邩���m���߂�</summary>
        /// <param name="isUpperFloor">�אڌ��̏��ʒu</param>
        /// <returns>true : ���Ă���</returns>
        public bool IsBuildable(bool isUpperFloor, byte region)
        {
            return this.region == region
                    && this.isUpperFloor == isUpperFloor
                    && floorType == FloorType.CommonFloor
                    && buildType == BuildType.None;
        }

        /// <summary>����L��3�~3�̒n�`��T��</summary>
        /// <returns>����ΑΏۂ̑S���</returns>
        public MapCell[] GetField3x3()
        {
            if (buildBitFlags > 254)
            {
                MapCell[] cells = { this, upLeft, up, upRight, left, right, downLeft, down, downRight };
                return cells;
            }
            return null;
        }

        /// <summary>����L��3�~2�̒n�`��T��</summary>
        /// <returns>����ΑΏۂ̑S���</returns>
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

        /// <summary>����L��2�~2�̒n�`��T��</summary>
        /// <returns>����ΑΏۂ̑S���</returns>
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

        /// <summary>����L��2�~1�̒n�`��T��</summary>
        /// <returns>����ΑΏۂ̑S���</returns>
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

    /// <summary>����z�ǂ�1�}�X�̏��</summary>
    class BridgeCell
    {
        /// <summary>���̓���</summary>
        public Compass enter = Compass.North;

        /// <summary>���̏o��</summary>
        public Compass exit = Compass.South;

        /// <summary>�z�u���鋴��z�ǂ̃^�C�v</summary>
        public BridgeType bridgeType = BridgeType.None;

        /// <summary>�Y���̏����</summary>
        MapCell floorData = null;




        /// <summary>����̋��̏��</summary>
        public BridgeCell upLeft = null;

        /// <summary>�^��̋��̏��</summary>
        public BridgeCell up = null;

        /// <summary>�E��̋��̏��</summary>
        public BridgeCell upRight = null;

        /// <summary>�^���̋��̏��</summary>
        public BridgeCell left = null;

        /// <summary>�^�E�̋��̏��</summary>
        public BridgeCell right = null;

        /// <summary>�����̋��̏��</summary>
        public BridgeCell downLeft = null;

        /// <summary>�^���̋��̏��</summary>
        public BridgeCell down = null;

        /// <summary>�E���̋��̏��</summary>
        public BridgeCell downRight = null;


        /// <summary>�Y���̏����</summary>
        public MapCell FloorData { get => floorData; }


        public BridgeCell(MapCell floorInfo)
        {
            floorData = floorInfo;
        }
    }
}