using UnityEngine;

public class BuildingPlacementController : MonoBehaviour
{
    public BuildingPlacementView placementView;
    public BuildingPlacementModel placementModel;
    public LayerMask groundLayer;
    private BuildingConfig selectedBuilding = null;
    private GameObject previewInstance;
    private BuildingFactory buildingFactory;


    private Camera mainCamera;
    private void Awake()
    {
        mainCamera = Camera.main;
       buildingFactory = new BuildingFactory();
    }
    private void Start()
    {
        placementView.CreateBuildingButtons(placementModel.buildingConfigs.ToArray());
    }


    public void SelectBuilding(BuildingConfig buildingData)
    {
        selectedBuilding = buildingData;
        //if (previewInstance != null)
        //{
        //    Destroy(previewInstance);
        //}
        //previewInstance = Instantiate(selectedBuilding.prefab);
        //previewInstance.GetComponent<Collider>().enabled = false; // Disable collision for preview
    }

    private void Update()
    {
        if (selectedBuilding != null /*&& previewInstance != null*/)
        {
            HandleBuildingPlacement();
        }
    }

    private void HandleBuildingPlacement()
    {
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);
        // Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero, 0f, groundLayer);
        if (hit.collider != null)
        {
           // previewInstance.transform.position = hit.point;

            if (Input.GetMouseButtonDown(0)) // Left-click to place
            {
                PlaceBuilding(hit.point);
            }
        }
    }

    private void PlaceBuilding(Vector3 position)
    {
        buildingFactory.Create(selectedBuilding);  // Need to move object after placement
      //  Instantiate(selectedBuilding.prefab, position, Quaternion.identity);
      //  Destroy(previewInstance);
        previewInstance = null;
        selectedBuilding = null;
    }
}
