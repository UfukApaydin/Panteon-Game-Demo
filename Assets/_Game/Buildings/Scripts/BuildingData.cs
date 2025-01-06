using UnityEngine;
using ObjectPoolSystem;
using Game.Unit;
[CreateAssetMenu(fileName = "BuildingBase", menuName = "Scriptable Objects/Buildings/BuildingData")]
public class BuildingData : ScriptableObject
{
    [Header("Config")]
    public string buildingName;
    public int maxHealth;
    public Vector2Int size;
    public float buildTime = 0;
    public bool canProduceUnit = true;
    public UnitData[] unitDatas;
    public BuildingPlacementConfig placementConfig;

    [Header("Visuals")]
    public GameObject prefab;
    public Sprite visual;
    public Texture icon;

    public Vector3 CenterOffset => new((size.x - 1) * 0.5f, (size.y - 1) * 0.5f, 0);
    public PoolSystem waypointPool;
}