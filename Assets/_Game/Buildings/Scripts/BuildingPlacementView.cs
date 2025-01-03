using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BuildingPlacementView : MonoBehaviour
{
    public BuildingPlacementController buildingController;
    public Transform buttonContainer;
    public GameObject buttonPrefab;

    public void Initialize(BuildingPlacementController buildingController,BuildingData[] buildings)
    {
        this.buildingController = buildingController;
        CreateBuildingButtons(buildings);
    }
    public void CreateBuildingButtons(BuildingData[] buildings)
    {
        foreach (BuildingData building in buildings)
        {
            GameObject buttonInstance = Instantiate(buttonPrefab, buttonContainer);
        //    buttonInstance.GetComponent<Image>().sprite = building.icon;
            buttonInstance.GetComponentInChildren<TMP_Text>().text = building.name;
            buttonInstance.GetComponent<Button>().onClick.AddListener(() => buildingController.SelectBuilding(building));
        }
    }
}
 