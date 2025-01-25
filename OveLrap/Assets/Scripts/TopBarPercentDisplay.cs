using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BubbleColors
{
    None,
    Red,//TODO
    Blue,
    Green
}

[System.Serializable]
public struct EntityDisplay
{
    public BubbleColors type;
    public GameObject barSegment;
}

public class TopBarPercentDisplay : MonoBehaviour
{
    [Tooltip("List of entity types and their corresponding prefabs for the bar.")]
    public List<EntityDisplay> entityDisplayList;
    [Tooltip("The UI container where the percentage bar will be displayed.")]
    public RectTransform barContainer;

    // Start is called before the first frame update
    void Start()
    {
        UpdateBar();
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateBar();
    }

    void UpdateBar()
    {
        Dictionary<BubbleColors, int> entityCounts = PlayspaceController.Instance.getBubbleColorCounts();
        if (entityCounts == null || entityCounts.Count == 0)
        {
            Debug.LogWarning("No entities found to display in the percentage bar.");
            return;
        }

        // Calculate total count
        int totalCount = 0;
        foreach (var count in entityCounts.Values)
        {
            totalCount += count;
        }
        if (totalCount == 0)
        {
            Debug.LogWarning("Total count is zero; no bar segments to adjust.");
            return;
        }

        // Adjust bar segment sizes and positions
        float currentXPosition = 0f;
        foreach (var entityDisplay in entityDisplayList)
        {
            if (!entityCounts.TryGetValue(entityDisplay.type, out int count))
            {
                count = 0; // no entities of this type
            }

            float percentage = (float)count / totalCount;

            // Adjust the segment size and position
            RectTransform segmentRect = entityDisplay.barSegment.GetComponent<RectTransform>();
            segmentRect.anchorMin = new Vector2(currentXPosition, 0);
            currentXPosition += percentage;
            segmentRect.anchorMax = new Vector2(currentXPosition, 1);
            segmentRect.offsetMin = Vector2.zero;
            segmentRect.offsetMax = Vector2.zero;
        }
    }
}
