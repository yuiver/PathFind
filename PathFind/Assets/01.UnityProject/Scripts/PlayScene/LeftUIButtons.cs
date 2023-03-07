using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftUIButtons : MonoBehaviour
{
    //! A Star Find path 버튼을 누른 경우
    public void OnClickAstarFindBtn()
    {
        GFunc.Log("Astar 알고리즘 버튼을 클릭했다");
        PathFinder.Instance.FindPath_AStar();
    }
    
}
