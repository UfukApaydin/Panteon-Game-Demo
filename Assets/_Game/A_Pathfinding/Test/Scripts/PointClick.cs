using A_Pathfinding.Pathfinding;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace A_Pathfinding.Test
{
    public class PointClick : MonoBehaviour
    {
        [SerializeField]
        private LayerMask groundLayerMask;

        private List<Unit> units;
        private bool isButtonClicked;
        private GameObject clickPointObj;
        private Vector2Int gridSize;
        private float cellSize;
        private Camera mainCamera;
        [SerializeField]private GameObject clickPointPrefab;
       
        // ------ Building Placement -----
        public List<BuildingConfig> Buildings;
        private BuildingConfig selectedBuildingPrefab;
        private int buildIndex = 0;
        private bool canBuild = false;
        private BuildingFactory buildingFactory;
        public void Init(Vector2Int gridSize, float cellSize)
        {
            this.gridSize = gridSize;
            this.cellSize = cellSize;
            mainCamera = Camera.main;
            units = FindObjectsByType<Unit>(FindObjectsSortMode.None).ToList();
            clickPointObj = Instantiate(clickPointPrefab);
            buildingFactory = new BuildingFactory();
        }
        void Update()
        {
            //Swith mouse mode 
            if (Input.GetKeyDown(KeyCode.E))
            {
                canBuild = !canBuild;
            }
            //Switch Buildings
            if (Input.GetKeyDown(KeyCode.Q))
            {
                selectedBuildingPrefab = Buildings[buildIndex++ % Buildings.Count()];
            }
            if (!isButtonClicked && Input.GetMouseButtonDown(0))
            {
                isButtonClicked = true;
                Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

                RaycastHit2D hit2D = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero, 0f, groundLayerMask);


                if (hit2D.collider != null)
                {
                    Vector3 groundPosition = hit2D.point;
                    groundPosition.z = 0;

                    Debug.Log($"Move click position: {groundPosition}");
                    groundPosition = SnapToGrid(groundPosition);
                    clickPointObj.transform.position = groundPosition;

                    Debug.Log($"Move click position: {groundPosition}");

                    // Command all units to move there

                    if (canBuild)
                    {
                       // buildingFactory.Create(selectedBuildingPrefab).Build(GameInitiator.Instance.gridManager.GetCellFromWorlPosition(groundPosition));
                        //var prefab = Instantiate(selectedBuildingPrefab, groundPosition, Quaternion.identity);
                        //prefab.Build(GameInitiator.Instance.gridManager.GetCellFromWorlPosition(groundPosition));
                    }
                    else
                    {
                        foreach (Unit unit in units)
                        {
                            unit.MoveToPosition(groundPosition);
                        }
                    }
                  

                   
                   
                }
            }
            if(Input.GetMouseButtonUp(0))
            {
                isButtonClicked = false;
            }
        }
        public Vector3 SnapToGrid(Vector3 position)
        {
           var pos = GameInitiator.Instance.gridManager.GetSnapPosition(position);  //----------------------Change This-------------------
            return new Vector3(pos.x, pos.y, 0);
        
        }
    }
}