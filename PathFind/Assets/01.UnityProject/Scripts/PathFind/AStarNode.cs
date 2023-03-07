using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public TerrainControler Terrain { get; private set; }
    public GameObject DestinationObj { get; private set; }

    //A Star Algorithm
    public float AStarF { get; private set; } = float.MaxValue;
    public float AStarG { get; private set;} = float.MaxValue;
    public float AStarH { get; private set; } = float.MaxValue;
    public AStarNode AStarPrevNode { get; private set; } = default;

    public AStarNode(TerrainControler terrain_, GameObject destinationObj_)
    {
        Terrain = terrain_;
        DestinationObj = destinationObj_;
    }

    //! AStar 알고리즘에 사용할 비용을 설정한다.
    public void UpdateCost_AStar(float gCost, float heuristic, AStarNode prevNode)
    {
        float aStarF = gCost + heuristic;

        if (aStarF < AStarF)
        { 
            AStarG = gCost;
            AStarH = heuristic;
            AStarH = aStarF;

            AStarPrevNode = prevNode;
        }       // if: 비용이 더 작은 경우에만 업데이트한다.
    }       //UpdateCost_AStar()

    //! 설정한 비용을 출력한다.
    public void ShowCost_AStar()
    {
        GFunc.Log($"TileIdx1D : {Terrain.TileIdx1D}," +
            $"F: {AStarF}, G: {AStarG}, H: {AStarH}");
    }       //ShowCost_AStar()
}
