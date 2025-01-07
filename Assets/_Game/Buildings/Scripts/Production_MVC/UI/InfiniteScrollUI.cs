using Game.Unit;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class InfiniteScrollUI : MonoBehaviour
{
    [Header("References")]
    public ScrollRect scrollRect;
    public RectTransform contentArea;   // The content RectTransform that has the GridLayoutGroup
    public GridLayoutGroup gridLayout;
    public ProductionSlot itemPrefab;

    [Header("Settings")]
    public int rowBuffer = 2;         // Extra rows above/below screen to avoid flicker
    public int totalRows = 0;         // Number of data rows (automatically computed if not infinite)
    public int totalColumns = 2;      // We want 2 columns
    public List<Data> dataSource;   // Example data - each string is an item

    private List<ProductionSlot> pooledItems = new List<ProductionSlot>();
    private int itemCountPerRow;
    private int visibleRows;
    private int startIndex = 0;       // The first item index in our data source currently displayed

    private float cellHeight;
    private float cellWidth;
    private float scrollRectHeight;

    private void Awake()
    {
        // 1) Setup references / initial calculations
        if (gridLayout == null) gridLayout = contentArea.GetComponent<GridLayoutGroup>();

        itemCountPerRow = totalColumns; // 2
        cellHeight = gridLayout.cellSize.y + gridLayout.spacing.y;
        cellWidth = gridLayout.cellSize.x + gridLayout.spacing.x;
        scrollRectHeight = scrollRect.GetComponent<RectTransform>().rect.height;

        // 2) Calculate how many rows fit in the visible area + buffer
        visibleRows = Mathf.CeilToInt(scrollRectHeight / cellHeight) + rowBuffer;

        ConfigureUI();
    }

    private void OnEnable()
    {
        // Listen to the scroll event
        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    private void OnDisable()
    {
        scrollRect.onValueChanged.RemoveListener(OnScroll);
    }

    private void OnScroll(Vector2 scrollPos)
    {
        // Check if we need to shift items up/down
        UpdateVisibleItems();
    }

    private void UpdateVisibleItems()
    {
        // 1) Where is contentArea relative to scroll?
        float contentPosY = contentArea.anchoredPosition.y;
        // The row we consider "top row" based on scrolled amount
        int firstRow = Mathf.FloorToInt(contentPosY / cellHeight);
        // Clamp so we don't go below 0
        firstRow = Mathf.Max(firstRow, 0);

        int newStartIndex = firstRow * itemCountPerRow; // index in dataSource

        // 2) If we've changed which items should be displayed, update
        if (newStartIndex != startIndex)
        {
            startIndex = newStartIndex;
            // We only shift or re-populate items. There's no reason to destroy them, we re-use the same pool.
            RenderItems();
        }
    }
    private void RenderItems()
    {
        // For each item in the pool, assign data from [startIndex..startIndex+ poolCount]
        // But ensure we don't go past dataSource.Count
        for (int i = 0; i < pooledItems.Count; i++)
        {
            int dataIndex = startIndex + i;
            ProductionSlot itemGO = pooledItems[i];

            if (dataIndex < dataSource.Count)
            {
                // Activate & update content
                itemGO.gameObject.SetActive(true);

                // Calculate row & column based on dataIndex
                int row = dataIndex / totalColumns;
                int col = dataIndex % totalColumns;

                // Compute the local position for this item
                float xPos = (col + 0.5f) * (cellWidth + gridLayout.spacing.x);
                // Typically in a top-down approach, you might do negative row for downward
                float yPos = -row * (cellHeight + gridLayout.spacing.y);

                // Set localPosition relative to the content/container transform
                //      itemGO.transform.localPosition = new Vector3(xPos, yPos, 0f);
                (itemGO.transform as RectTransform).anchoredPosition = new Vector2(xPos, yPos);

                itemGO.SetProduction(dataSource[dataIndex]);
                currentStrategy.AddListener(itemGO.GetComponent<Button>(), dataIndex);
                // Example usage: let's say we set a text
                //   itemGO.GetComponentInChildren<Text>().text = dataSource[dataIndex].dataName;
            }
            else
            {
                // Past end of data => hide
                itemGO.ResetProduction();
                itemGO.gameObject.SetActive(false);
            }
        }

    }
    private void ConfigureUI()
    {

        int initialPoolCount = visibleRows * itemCountPerRow;


        for (int i = 0; i < initialPoolCount; i++)
        {
            if (pooledItems.Count < initialPoolCount)
            {
                ProductionSlot itemGO = Instantiate(itemPrefab, contentArea);
                pooledItems.Add(itemGO);
            }
            else
            {
                break;
            }
        }
        totalRows = Mathf.CeilToInt((float)dataSource.Count / totalColumns);
        float contentHeight = totalRows * cellHeight;
        contentArea.sizeDelta = new Vector2(contentArea.sizeDelta.x, contentHeight);

        // 5) Fill initial items

    }
    UIStrategyBase currentStrategy;
    public void SetUIData(UIStrategyBase strategy)
    {
        currentStrategy = strategy;
        dataSource.Clear();
        dataSource = currentStrategy.Data.ToList();

        ConfigureUI();
        UpdateVisibleItems();
        RenderItems();
    }
}




