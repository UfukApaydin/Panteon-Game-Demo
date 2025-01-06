using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfiniteScroll : MonoBehaviour
{
    [Header("UI Settings")]
    public RectTransform contentArea;
    public ScrollRect scrollRect;
    public GameObject itemPrefab;

    [Header("Scroll Settings")]
    public int poolSize = 20; // Number of items in the pool (adjust for 2 columns)
    public float itemWidth = 200f; // Width of each item
    public float itemHeight = 100f; // Height of each item
    public float horizontalPadding = 10f; // Padding between columns
    public float verticalPadding = 10f; // Padding between rows
    public float contentWidth = 500f; // Width of the content area for equal distribution

    private Queue<GameObject> itemPool = new Queue<GameObject>();
    private List<int> displayedIndices = new List<int>();

    private float lastScrollPosition = 0f;
    private int totalDataItems = 100; // Replace this with your actual data count

    void Start()
    {
        InitializePool();
        UpdateVisibleItems();
        scrollRect.onValueChanged.AddListener(OnScroll);
    }

    void InitializePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject item = Instantiate(itemPrefab, contentArea);
            item.SetActive(false);
            itemPool.Enqueue(item);
        }

        // Adjust content height to fit all data
        int rows = Mathf.CeilToInt((float)totalDataItems / 2); // 2 columns
        contentArea.sizeDelta = new Vector2(contentArea.sizeDelta.x, rows * (itemHeight + verticalPadding) - verticalPadding);
    }

    void OnScroll(Vector2 scrollPosition)
    {
        float currentScrollY = contentArea.anchoredPosition.y;
        if (Mathf.Abs(currentScrollY - lastScrollPosition) > itemHeight / 2)
        {
            lastScrollPosition = currentScrollY;
            UpdateVisibleItems();
        }
    }

    void UpdateVisibleItems()
    {
        float scrollY = contentArea.anchoredPosition.y;
        float viewportHeight = scrollRect.viewport.rect.height;

        int firstVisibleRow = Mathf.Max(0, Mathf.FloorToInt(scrollY / (itemHeight + verticalPadding)));
        int lastVisibleRow = Mathf.Min(Mathf.CeilToInt((float)totalDataItems / 2), Mathf.CeilToInt((scrollY + viewportHeight) / (itemHeight + verticalPadding)));

        HashSet<int> newIndices = new HashSet<int>();
        for (int row = firstVisibleRow; row < lastVisibleRow; row++)
        {
            for (int col = 0; col < 2; col++) // Loop through columns
            {
                int index = row * 2 + col;
                if (index < totalDataItems)
                {
                    newIndices.Add(index);
                }
            }
        }

        // Recycle unused items
        for (int i = displayedIndices.Count - 1; i >= 0; i--)
        {
            if (!newIndices.Contains(displayedIndices[i]))
            {
                RecycleItem(displayedIndices[i]);
                displayedIndices.RemoveAt(i);
            }
        }

        // Add new items
        foreach (int index in newIndices)
        {
            if (!displayedIndices.Contains(index))
            {
                GameObject item = GetItemFromPool();
                PositionItem(item, index);
                PopulateItem(item, index);
                displayedIndices.Add(index);
            }
        }
    }

    GameObject GetItemFromPool()
    {
        if (itemPool.Count > 0)
        {
            GameObject item = itemPool.Dequeue();
            item.SetActive(true);
            return item;
        }

        Debug.LogWarning("Pool size is too small for the current viewport.");
        return null;
    }

    void RecycleItem(int index)
    {
        foreach (Transform child in contentArea)
        {
            if (child.gameObject.activeSelf && child.gameObject.name == "Item" + index)
            {
                child.gameObject.SetActive(false);
                itemPool.Enqueue(child.gameObject);
                break;
            }
        }
    }

    void PositionItem(GameObject item, int index)
    {
        RectTransform rectTransform = item.GetComponent<RectTransform>();
        int row = index / 2;
        int col = index % 2;

        // Calculate total space for items including padding
        float totalWidth = 2 * itemWidth + horizontalPadding;
        float xOffset = (contentWidth - totalWidth) / 2; // Adjust for center alignment

        float xPosition = -contentWidth / 2 + xOffset + col * (itemWidth + horizontalPadding);
        float yPosition = -row * (itemHeight + verticalPadding);
        rectTransform.anchoredPosition = new Vector2(xPosition, yPosition);
    }

    void PopulateItem(GameObject item, int index)
    {
        item.name = "Item" + index;
        Text label = item.GetComponentInChildren<Text>();
        if (label != null)
        {
            label.text = "Item " + index;
        }
    }
}
