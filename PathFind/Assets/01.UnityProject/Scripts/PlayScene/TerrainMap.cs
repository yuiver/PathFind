using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainMap : TileMapControler
{
    private const string TERRAIN_TILEMAP_OBJ_NAME = "TerrainTileMap";

    private Vector2Int mapCellSize = default;
    private Vector2 mapCellGap = default;

    [SerializeField]
    private List<TerrainControler> allTerrains = default;

    //! Awake Ÿ�ӿ� �ʱ�ȭ �� ������ �������Ѵ�.
    public override void InitAwake(MapBoard mapControler_)
    {
        this.tileMapObjName = TERRAIN_TILEMAP_OBJ_NAME;
        base.InitAwake(mapControler_);

        allTerrains = new List<TerrainControler>();

        // {Ÿ���� x�� ������ ��ü Ÿ���� ���� ���� ���� ����� �����Ѵ�.}
        mapCellSize = Vector2Int.zero;
        float tempTileY = allTileObjs[0].transform.localPosition.y;
        for (int i = 0; i < allTileObjs.Count; i++)
        {
            if (tempTileY.IsEquals(allTileObjs[i].transform.localPosition.y) == false)
            {
                mapCellSize.x = i;
                break;
            }       //if: ù��° Ÿ���� y ��ǥ�� �޶����� ���� ������ ���� ���� �� ũ���̴�.
        }
        // } ��ü Ÿ���� ���� ���� ���� �� ũ��� ���� ���� ���� ���� �� ����� �����Ѵ�.

        // { x �� ���� �� Ÿ�ϰ�, y �� ���� �� Ÿ�� ������ ���� ���������� Ÿ�� ���� �����Ѵ�.
        mapCellGap = Vector2.zero;
        mapCellGap.x = allTileObjs[1].transform.localPosition.x -
            allTileObjs[0].transform.localPosition.x;
        mapCellGap.y = allTileObjs[mapCellSize.x].transform.localPosition.y -
            allTileObjs[0].transform.localPosition.y;
        // } x �� ���� �� Ÿ�ϰ�, y �� ���� �� Ÿ�� ������ ���� ���������� Ÿ�� ���� �����Ѵ�.

    }       //InitAwake()

    private void Start()
    {
        // { Ÿ�ϸ��� �Ϻθ� ���� Ȯ���� �ٸ� Ÿ�Ϸ� ��ü�ϴ� ����
        GameObject changeTilePrefab = ResManager.Instance.
            TerrainPrefabs[RDefine.TERRAIN_PREF_OCEAN];
        // Ÿ�ϸ� �߿� ��� ������ �ٴٷ� ��ü�� ������ �����Ѵ�.
        const float CHANGE_PRECENTAGE = 15.0f;
        float correntChangePercentage =
            allTileObjs.Count * (CHANGE_PRECENTAGE / 100.0f);
        // �ٴٷ� ��ü�� Ÿ���� ������ ����Ʈ ���·� �����ؼ� ���´�.
        List<int> ChangedTileResult = GFunc.CreateList(allTileObjs.Count, 1);
        ChangedTileResult.Shuffle();

        GameObject tempChangeTile = default;
        for (int i = 0; i < allTileObjs.Count; i++)
        {
            if (correntChangePercentage <= ChangedTileResult[i]) { continue; }

            //�������� �ν��Ͻ�ȭ�ؼ� ��ü�� Ÿ���� Ʈ�������� ī���Ѵ�.
            tempChangeTile = Instantiate(
                changeTilePrefab, tileMap.transform);
            tempChangeTile.name = changeTilePrefab.name;
            tempChangeTile.SetLocalScale(allTileObjs[i].transform.localScale);
            tempChangeTile.SetLocalPos(allTileObjs[i].transform.localPosition);

            allTileObjs.Swap(ref tempChangeTile, i);
            tempChangeTile.DestroyObj();
        }       //loop: ������ ������ ������ ���� Ÿ�ϸʿ� �ٴٸ� �����ϴ� ����
        // } Ÿ�ϸ��� �Ϻθ� ���� Ȯ���� �ٸ� Ÿ�Ϸ� ��ü�ϴ� ����

        // { ������ �����ϴ� Ÿ���� ������ �����ϰ� , ��Ʈ�ѷ��� ĳ���ϴ� ����
        // } ������ �����ϴ� Ÿ���� ������ �����ϰ� , ��Ʈ�ѷ��� ĳ���ϴ� ����
    }   //Start()

    //! �ʱ�ȭ�� Ÿ���� ������ ������ ���� ����, ���� ũ�⸦ �����Ѵ�.
    public Vector2Int GetCellSize() { return mapCellSize; }

    //! �ʱ�ȭ�� Ÿ���� ������ ������ Ÿ�� ������ ���� �����Ѵ�.
    public Vector2 GetCellGap() { return mapCellGap; }

    //! �ε����� �ش��ϴ� Ÿ���� �����Ѵ�.
    public TerrainControler GetTile(int tileIdx1D)
    {
        if (allTerrains.IsValid(tileIdx1D))
        { 
            return allTerrains[tileIdx1D];
        }
        return default;
    }       //GetTile()


}
