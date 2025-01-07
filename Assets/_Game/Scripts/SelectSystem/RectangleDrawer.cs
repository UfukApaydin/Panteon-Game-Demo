using UnityEngine;

namespace SelectionSystem
{
    public class RectangleDrawer : MonoBehaviour
    {
        private Vector3 startPoint;
        private RectTransform selectionBox;

        public bool IsDrawing { get; private set; }
        public Rect SelectionRect { get; private set; }

        void Start()
        {
            selectionBox = GetComponent<RectTransform>();
            selectionBox.gameObject.SetActive(false);
        }

        public void StartDrawing(Vector3 screenStartPoint)
        {
            IsDrawing = true;
            startPoint = screenStartPoint;
            //selectionBox.gameObject.SetActive(true);
        }

        public void StopDrawing()
        {
            IsDrawing = false;
            selectionBox.gameObject.SetActive(false);
            SelectionRect = Rect.zero; // Reset the rectangle
        }

        void Update()
        {
            if (IsDrawing)
            {
                Vector3 currentPoint = Input.mousePosition;

                // Create a rectangle based on start and current points
                float width = currentPoint.x - startPoint.x;
                float height = currentPoint.y - startPoint.y;

                // Update the UI element
                selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
                selectionBox.anchoredPosition = startPoint + new Vector3(width / 2, height / 2);

                // Store the world space selection rectangle
                SelectionRect = new Rect(
                    Mathf.Min(startPoint.x, currentPoint.x),
                    Mathf.Min(startPoint.y, currentPoint.y),
                    Mathf.Abs(width),
                    Mathf.Abs(height)
                );
            }
        }
    }
}