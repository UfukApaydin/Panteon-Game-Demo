using Game.Unit;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "BuildingBase", menuName = "Scriptable Objects/Buildings/BuildingData")]
public class BuildingData : Data
{
    [Header("Config")]
    public int maxHealth;
    public Vector2Int size;
    public float buildTime = 0;
    public bool canProduceUnit = true;
    public List<UnitData> unitDatas;
    public BuildingPlacementConfig placementConfig;

    [Header("Visuals")]
    public GameObject prefab;
    public Sprite visual;


    public Vector3 CenterOffset => new((size.x - 1) * 0.5f, (size.y - 1) * 0.5f, 0);

}


