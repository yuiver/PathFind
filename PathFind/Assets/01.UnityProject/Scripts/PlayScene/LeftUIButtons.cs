using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftUIButtons : MonoBehaviour
{
    //! A Star Find path ��ư�� ���� ���
    public void OnClickAstarFindBtn()
    {
        GFunc.Log("Astar �˰��� ��ư�� Ŭ���ߴ�");
        PathFinder.Instance.FindPath_AStar();
    }
    
}
