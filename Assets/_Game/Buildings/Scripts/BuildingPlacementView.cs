using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BuildingPlacementView : MonoBehaviour
{
    public BuildingPlacementController buildingController;
    public Transform buttonContainer;
    public GameObject buttonPrefab;

    public void CreateBuildingButtons(BuildingConfig[] buildings)
    {
        foreach (BuildingConfig building in buildings)
        {
            GameObject buttonInstance = Instantiate(buttonPrefab, buttonContainer);
        //    buttonInstance.GetComponent<Image>().sprite = building.icon;
            buttonInstance.GetComponentInChildren<TMP_Text>().text = building.name;
            buttonInstance.GetComponent<Button>().onClick.AddListener(() => buildingController.SelectBuilding(building));
        }
    }
}
