using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BuildingPlacementView : MonoBehaviour
{
    public Transform buttonContainer;
    public GameObject buttonPrefab;

    private BuildingPlacementController _buildingController;
    public void Init(BuildingPlacementController buildingController,BuildingData[] buildings)
    {
        _buildingController = buildingController;
        CreateBuildingButtons(buildings);
    }
    public void CreateBuildingButtons(BuildingData[] buildings)
    {
        foreach (BuildingData building in buildings)
        {
            GameObject buttonInstance = Instantiate(buttonPrefab, buttonContainer);
        //    buttonInstance.GetComponent<Image>().sprite = building.icon;
            buttonInstance.GetComponentInChildren<TMP_Text>().text = building.name;
            buttonInstance.GetComponent<Button>().onClick.AddListener(() => _buildingController.SelectBuilding(building));
        }
    }
}
 