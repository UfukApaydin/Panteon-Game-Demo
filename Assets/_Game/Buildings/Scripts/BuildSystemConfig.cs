using UnityEngine;

[CreateAssetMenu(fileName = "New_BuildSystemConfig", menuName = "Scriptable Objects/Configs/BuildSystemConfig")]
public class BuildSystemConfig : ScriptableObject
{
    public BuildingData[] buildingDatas;
    public LayerMask groundLayerMask;
  //  public LayerMask placeableLayerMask;
    public LayerMask previewCollideLayers;
}
