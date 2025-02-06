using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class InfiniteScrollUI : MonoBehaviour
{
    [Header("References")]
    public ScrollRect scrollRect;
    public RectTransform contentArea;
    public GridLayoutGroup gridLayout;
    public ProductionSlot itemPrefab;

    [Header("Settings")]
    public int rowBuffer = 2;
    public int totalRows = 0;
    public int totalColumns = 2;
    public List<Data> dataSource;

    private List<ProductionSlot> pooledItems = new List<ProductionSlot>();
    private int itemCountPerRow;
    private int visibleRows;
    private int startIndex = 0;

    private float cellHeight;
    private float cellWidth;
    private float scrollRectHeight;
    private Vector2 itemOffset;

    private void Awake()
    {

        if (gridLayout == null) gridLayout = contentArea.GetComponent<GridLayoutGroup>();

        itemCountPerRow = totalColumns;
        cellHeight = gridLayout.cellSize.y + gridLayout.spacing.y;
        cellWidth = gridLayout.cellSize.x + gridLayout.spacing.x;
        scrollRectHeight = scrollRect.GetComponent<RectTransform>().rect.height;

        itemOffset = new Vector2(cellWidth / 2 + gridLayout.spacing.x, -cellHeight / 2 - gridLayout.spacing.y);
        visibleRows = Mathf.CeilToInt(scrollRectHeight / cellHeight) + rowBuffer;

        ConfigureUI();
    }

    private void OnEnable()
    {
        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    private void OnDisable()
    {
        scrollRect.onValueChanged.RemoveListener(OnScroll);
    }

    private void OnScroll(Vector2 scrollPos)
    {

        UpdateVisibleItems();
    }

    private void UpdateVisibleItems()
    {

        float contentPosY = contentArea.anchoredPosition.y;

        int firstRow = Mathf.FloorToInt(contentPosY / cellHeight);

        firstRow = Mathf.Max(firstRow, 0);

        int newStartIndex = firstRow * itemCountPerRow;

        if (newStartIndex != startIndex)
        {
            startIndex = newStartIndex;

            RenderItems();
        }
    }
    private void RenderItems()
    {

        for (int i = 0; i < pooledItems.Count; i++)
        {
            int dataIndex = startIndex + i;
            ProductionSlot itemGO = pooledItems[i];

            if (dataIndex < dataSource.Count)
            {

                itemGO.gameObject.SetActive(true);

                int row = dataIndex / totalColumns;
                int col = dataIndex % totalColumns;

                float xPos = (col * (cellWidth + gridLayout.spacing.x)) + itemOffset.x;

                float yPos = (-row * (cellHeight + gridLayout.spacing.y)) + itemOffset.y;


                (itemGO.transform as RectTransform).anchoredPosition = new Vector2(xPos, yPos);

                itemGO.SetProduction(dataSource[dataIndex]);
                Button button = itemGO.GetComponent<Button>();
                button.onClick.RemoveAllListeners();
                currentStrategy.AddListener(button, dataIndex);

            }
            else
            {

                itemGO.ResetProduction();
                itemGO.gameObject.SetActive(false);
            }
        }

    }
    private void ConfigureUI()
    {
        //  Fix: Destroy old objects before creating new ones
        foreach (var item in pooledItems)
        {
            Destroy(item.gameObject);
        }
        pooledItems.Clear();

        int initialPoolCount = visibleRows * itemCountPerRow;

        for (int i = 0; i < initialPoolCount; i++)
        {
            ProductionSlot itemGO = Instantiate(itemPrefab, contentArea);
            pooledItems.Add(itemGO);
        }

        totalRows = Mathf.CeilToInt((float)dataSource.Count / totalColumns);
        float contentHeight = totalRows * cellHeight;
        contentArea.sizeDelta = new Vector2(contentArea.sizeDelta.x, contentHeight);
    }

    UIStrategyBase currentStrategy;
    public void SetUIData(UIStrategyBase strategy)
    {
        currentStrategy = strategy;
        dataSource = new List<Data>(currentStrategy.Data);
       // dataSource = currentStrategy.Data.ToList();

        ConfigureUI();
        UpdateVisibleItems();
        RenderItems();
    }
}




