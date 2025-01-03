using A_Pathfinding.Nodes;
using UnityEngine;
using PathfindingGrid = A_Pathfinding.Nodes.PathfindingGrid;

namespace A_Pathfinding.Test
{
    public class PlaceObstacle : MonoBehaviour
    {
         
        [SerializeField]
        private Camera mainCamera;

        public PathfindingGrid grid;
        public GameObject objectToPlace;

        private GameObject previewObject;
        float cellSize => GameInitiator.Instance.pathfindingDirector.grid.nodeRadius;
        public LayerMask groundLayer;


        // Update is called once per frame
        void Update()
        {     // Detect mouse position in world space using a raycast
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseWorldPos2D = new Vector2(mouseWorldPos.x, mouseWorldPos.y);

            RaycastHit2D hit2D = Physics2D.Raycast(mouseWorldPos2D, Vector2.zero, 0f, groundLayer);


            if (hit2D.collider != null)
            {
                Vector3 hitPosition = hit2D.point;

                // Snap position to grid
                Vector3 snappedPosition = SnapToGrid(hitPosition);

                // Update preview object
                UpdatePreviewObject(snappedPosition);

                // Place object on left mouse click
                if (Input.GetMouseButtonDown(1))
                {
                    PlaceObject(snappedPosition);
                }
            }
        }

        Vector3 SnapToGrid(Vector3 position)
        {
            float x = Mathf.Floor(position.x / cellSize) * cellSize + cellSize / 2;
            float y = Mathf.Floor(position.y / cellSize) * cellSize + cellSize / 2;
            return new Vector3(x, y, 0);
        }
        void UpdatePreviewObject(Vector3 position)
        {
            if (previewObject == null)
            {
                previewObject = Instantiate(objectToPlace);
              //  previewObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f); // Make semi-transparent
            }

            previewObject.transform.position = position;
        }
        void PlaceObject(Vector3 position)
        {
            // Place the object at the snapped position
            Instantiate(objectToPlace, position, Quaternion.identity);
        }
    }
}