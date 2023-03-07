using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFinder : GSingleton<PathFinder>
{
    #region 지형탐색을 위한 변수
    public GameObject sourceObj = default;
    public GameObject destinationObj = default;
    public MapBoard mapBoard = default;
    #endregion      //지형탐색을 위한 변수

    #region A Star 알고리즘으로 최단거리를 찾기위한 변수
    private List<AStarNode> aStarResultPath = default;
    private List<AStarNode> aStarOpenPath  = default;
    private List<AStarNode> aStarClosePath = default;
    #endregion  //A Star 알고리즘으로 최단거리를 찾기위한 변수

    //! 출발지와 목적지 정보로 길을 찾는 함수
    public void FindPath_AStar()
    {
        StartCoroutine(DelayFindPath_AStar(0.05f));
    }

    //! 탐색 알고리즘에 딜레이를 건다.
    private IEnumerator DelayFindPath_AStar(float delay_)
    { 
        // A Star 알고리즘을 사용하기 위해서 패스 리스트를 초기화한다.
        aStarOpenPath = new List<AStarNode>();
        aStarClosePath = new List<AStarNode>();
        aStarResultPath = new List<AStarNode>();

        TerrainControler targetTerrain = default;

        // 출발지의 인덱스를 구해서, 출발지 노드를 찾아온다.
        string[] sourceObjNamePart = sourceObj.name.Split('_');
        int sourceIdx1D = -1;
        int.TryParse(sourceObjNamePart[sourceObjNamePart.Length - 1], out sourceIdx1D);
        targetTerrain = mapBoard.GetTerrain(sourceIdx1D);
        //찾아온 출발지 노드를 Open List에 추가한다.
        AStarNode targetNode = new AStarNode(targetTerrain, destinationObj);
        Add_AStarOpenList(targetNode);

        int loopIdx = 0;
        bool isFoundDestination = false;
        bool isNoWayToGo = false;
        //while (loopIdx < 10)
        while (isFoundDestination == false && isNoWayToGo == false)
        {
            // { Open List를 순회해서 가장 코스트가 낮은 노드를 선택한다.
            AStarNode minCostNode = default;
            foreach (var terrainNode in aStarOpenPath)
            {
                if (minCostNode == default)
                {
                    minCostNode = terrainNode;
                }       //if : 가장 작은 코스트의 노드가 비어 있는 경우
                else
                {
                    // terrainNode 가 더 작은 코스트를 가지는 경우 minCostNode를 업데이트 한다.
                    if (terrainNode.AStarF < minCostNode.AStarF)
                    {
                        minCostNode = terrainNode;
                    }
                    else { continue; }
                }       //else : 가장 작은 코스트의 노드가 캐싱되어 있는경우
            }   // loop : 가장 코스트가 낮은 노드를 찾는 루프
            // } Open List를 순회해서 가장 코스트가 낮은 노드를 선택한다.

            minCostNode.ShowCost_AStar();
            minCostNode.Terrain.SetTileActiveColor(RDefine.TileStatusColor.SEARCH);

            // 선택한 노드가 목적지에 도달했는지 확인한다.
            bool isArriveDest = mapBoard.GetDistance2D(minCostNode.Terrain.gameObject, destinationObj).Equals(Vector2Int.zero);

            if (isArriveDest)
            {
                // {목적지에 도착했다면 aStarResultPath 리스트를 설정한다.
                AStarNode resultNode = minCostNode;
                bool isSet_aStarResultPathOk = false;
                while (isSet_aStarResultPathOk == false)
                {
                    aStarResultPath.Add(resultNode);
                    if (resultNode.AStarPrevNode == default || resultNode.AStarPrevNode == null)
                    {
                        isSet_aStarResultPathOk = true;
                        break;
                    }
                    else { /*Do nothing*/}
                    
                    resultNode = resultNode.AStarPrevNode;
                }       // ㅣloop : 이전 노드를 찾기 못할 때 까지 순회하는 루프
                // }목적지에 도착했다면 aStarResultPath 리스트를 설정한다.

                // Open List와 Close List를 정리한다.
                aStarOpenPath.Clear();
                aStarClosePath.Clear();
                isFoundDestination = true;
                break;
            }       // if: 선택한 노드가 목적지에 도착한 경우
            else 
            {
                // { 도착하지 않았다면 현재 타일을 기준으로 4 방향 노드를 찾아온다.
                List<int> nextSearchIdx1Ds = mapBoard.GetTileIdx2D_Around4ways(minCostNode.Terrain.TileIdx2D);

                //찾아온 노드 중에서 이동 가능한 노드는 Open List에 추가한다.
                AStarNode nextNode = default;
                foreach (var nextIdx1D in nextSearchIdx1Ds)
                {
                    nextNode = new AStarNode(mapBoard.GetTerrain(nextIdx1D), destinationObj);
                    if (nextNode.Terrain.IsPassable == false) { continue; }

                    Add_AStarOpenList(nextNode , minCostNode);
                }       //loop : 이동 가능한 노드를 Open List에 추가하는 루프
                // } 도착하지 않았다면 현재 타일을 기준으로 4 방향 노드를 찾아온다.

                // 탐색이 끝난 노드는 Close List에 추가하고, Open List 에서 제거한다.
                // 이 때, Open List가 비어 있다면 더 이상 탐색할 수 있는 길이 존재하지 않는 것이다.
                aStarClosePath.Add(minCostNode);
                aStarOpenPath.Remove(minCostNode);
                if (aStarOpenPath.IsValid() == false)
                {
                    GFunc.LogWarning("[Warning] There are no more tiles to explore.");
                    isNoWayToGo = true;
                }       // if : 목적지에 도착하여 못했는데, 더 이상 탐색할 수 있는 길이 없는 경우

                foreach (var tempNode in aStarOpenPath)
                { 
                    GFunc.Log($"Idx : {tempNode.Terrain.TileIdx1D}, Cost : {tempNode.AStarF}");
                }
            }       //else: 선택한 노드가 목적지에 도착하지 못한 경우

            loopIdx++;
            yield return new WaitForSeconds(delay_);
        }       // loop : A Star 알고리즘으로 길을 찾는 메인 루프
    }       //DelayFindPath_AStar()

    //! 비용을 설정한 노드를 Open 리스트에 추가한다.
    private void Add_AStarOpenList(AStarNode targetTerrain_, AStarNode prevNode = default)
    { 
        //Open 리스트에 추가하기 전에 알고리즘 비용을 설정한다.
        Update_AStarCostToTerrain(targetTerrain_, prevNode);

        AStarNode closeNode = aStarClosePath.FindNode(targetTerrain_);
        if (closeNode != default && closeNode != null)
        {
            //이미 탐색이 끝난 좌표의 노드가 존재하는 경우에는 Open List에 추가하지 않는다.
            /* Do Nothing */
        }       //if : Close List에 이미 탐색이 끝난 좌표의 노드가 존재하는 경우
        else
        {
            AStarNode openedNode = aStarOpenPath.FindNode(targetTerrain_);
            if (openedNode != default && openedNode != null)
            {
                // 타겟 노드의 코스트가 더 작은 경우에는 Open List에서 노드를 교체한다.
                // 타겟 노드의 코스트가 더 큰 경우에는 Open List에 추가하지 않는다.
                if (targetTerrain_.AStarF < openedNode.AStarF)
                {
                    aStarOpenPath.Remove(openedNode);
                    aStarOpenPath.Add(targetTerrain_);
                }
                else { /* Do Nothing */ }
            }       // if : Open List에 현재 추가할 노드와 같은 좌표의 노드가 존재하는 경우
            else
            {
                aStarOpenPath.Add(targetTerrain_);
            }       // else: Open List에 현재 추가할 노드와 같은 좌표의 노드가 없는 경우
        }       //else: 아직 탐색이 끝나지 않은 노드인 경우
    }       //Add_AStarOpenList()

    //! Target 지형 정보와 Destination 지형 정보로 Distance 와 Heuristic울 설정하는 함수
    private void Update_AStarCostToTerrain(
        AStarNode targetNode, AStarNode prevNode)
    {
        // {Target 지형에서 Destination까지의 2D 타일 거리를 계산하는 로직
        Vector2Int distance2D = mapBoard.GetDistance2D(
            targetNode.Terrain.gameObject, destinationObj);
        float totalDistance2D = distance2D.x + distance2D.y;

        // Heuristic 은 직선거리로 고정한다.
        Vector2 localDistance = destinationObj.transform.localPosition -
            targetNode.Terrain.transform.localPosition;
        float heuristic = Mathf.Abs(localDistance.magnitude);
        // }Target 지형에서 Destination까지의 2D 타일 거리를 계산하는 로직

        // { 이전 노드가 존재하는 경우 이전 노드의 코스트를 추가해서 연산한다.
        if (prevNode == default || prevNode == null) { /* Do Nothing*/ }
        else
        {
            totalDistance2D = Mathf.RoundToInt(prevNode.AStarG + 1.0f);
        }
        targetNode.UpdateCost_AStar(totalDistance2D, heuristic, prevNode);
        // } 이전 노드가 존재하는 경우 이전 노드의 코스트를 추가해서 연산한다.

    }       // Update_AStarCostToTerrain()
}
