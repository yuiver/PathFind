using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResManager : GSingleton<ResManager>
{
    private const string TERRAIN_PREF_PATH = "Prefabs/ObjectTiles/Terrains";
    private const string OBSTACLE_PREF_PATH = "Prefabs/ObjectTiles/Obstacles";

    public Dictionary<string, GameObject> TerrainPrefabs = default;
    public Dictionary<string, GameObject> obstaclePrefabs = default;

    protected override void Init()
    {
        base.Init();

        TerrainPrefabs = new Dictionary<string, GameObject>();
        obstaclePrefabs = new Dictionary<string, GameObject>();

        TerrainPrefabs.AddObjs(Resources.LoadAll<GameObject>(TERRAIN_PREF_PATH));
        obstaclePrefabs.AddObjs(Resources.LoadAll<GameObject>(OBSTACLE_PREF_PATH));
    }       //Init()
}       //Class ResManager
