using GridSystem;
using ObjectPoolSystem;
using UnityEngine;

[CreateAssetMenu(fileName = "New_GameData", menuName = "Scriptable Objects/Configs/GameData")]
public class GameData : ScriptableObject
{
    [Header("Config")]
    public GridConfig gridConfig;
    public BuildSystemConfig buildSystemConfig;
    [Header("Pools")]
    public PoolTypeSO markerType;
    public PoolTypeSO waypointType;
    public PoolTypeSO soldierType;
}
