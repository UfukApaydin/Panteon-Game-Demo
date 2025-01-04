using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Objects/Buildings/BuildingData")]
public class BuildingData : ScriptableObject
{
    public GameObject prefab;
    public string buildingName;
    public Sprite visual;
    public Sprite icon;
    public float maxHealth;
    public Vector2Int size;
    public BuildingPlacementConfig placementConfig;
    public float buildTime = 0;
    public Vector3 CenterOffset => new((size.x - 1) * 0.5f, (size.y - 1) * 0.5f, 0);
}