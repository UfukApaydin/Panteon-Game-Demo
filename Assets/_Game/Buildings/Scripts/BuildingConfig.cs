using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Scriptable Objects/Buildings")]
public class BuildingConfig : ScriptableObject
{
    public GameObject prefab;
    public string buildingName;
    public Sprite icon;
    public float maxHealth;
    public Vector2Int size;
    public Vector3 CenterOffset => new((size.x - 1) * 0.5f, (size.y - 1) * 0.5f, 0);
}