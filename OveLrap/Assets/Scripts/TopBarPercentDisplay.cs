using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public struct EntityDisplay
{
    public BubbleTone type;
    public GameObject barSegment;
}

public class TopBarPercentDisplay : MonoBehaviour
{
    [Tooltip("List of entity types and their corresponding prefabs for the bar.")]
    public List<EntityDisplay> entityDisplayList;
    [Tooltip("The UI container where the percentage bar will be displayed.")]
    public RectTransform barContainer;

    public TextMeshProUGUI numberOfPairsGoalText;
    const string goalText = "Adj + Noun Pairs: {0}/{1}"; 

    // Start is called before the first frame update
    void Start()
    {
        float currentXPosition = 0f;
        // Set bar segment to 0 for each entity
        foreach (var entityDisplay in entityDisplayList)
        {
            RectTransform segmentRect = entityDisplay.barSegment.GetComponent<RectTransform>();
            SetBarSegmentSize(segmentRect, 0f, ref currentXPosition);
        }

        // Set goal text
        SetGoalText(0);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateBar();
    }

    void UpdateBar()
    {
        // gets the number of colored pairs in the playspace
        Dictionary<BubbleTone, int> entityCounts = PlayspaceController.Instance.getBubbleColorCounts();
        if (entityCounts == null || entityCounts.Count == 0)
        {
            Debug.LogWarning("No entities found to display in the percentage bar.");
            return;
        }

        
        // Calculate total count of pairs
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

        SetGoalText(totalCount);
        

        // Adjust bar segment sizes and positions
        float currentXPosition = 0f;
        foreach (var entityDisplay in entityDisplayList)
        {
            if (!entityCounts.TryGetValue(entityDisplay.type, out int count))
            {
                count = 0; // no entities of this type
            }

            // Look for target count else target is 0
            float percentage = totalCount == 0 ? 0 : (float)count / totalCount;
            //Debug.Log("Percentage: " + percentage + "(" + count + "/" + totalCount + ") for " + entityDisplay.type);

            // Adjust the segment size and position
            RectTransform segmentRect = entityDisplay.barSegment.GetComponent<RectTransform>();
            SetBarSegmentSize(segmentRect, percentage, ref currentXPosition);
        }
    }

    void SetBarSegmentSize(RectTransform segmentRect, float percentage, ref float currentXPosition)
    {
        segmentRect.anchorMin = new Vector2(currentXPosition, 0);
        currentXPosition += percentage;
        segmentRect.anchorMax = new Vector2(currentXPosition, 1);
        segmentRect.offsetMin = Vector2.zero;
        segmentRect.offsetMax = Vector2.zero;
    }

    void SetGoalText(int currentCount)
    {
        numberOfPairsGoalText.text = string.Format(goalText, currentCount, PlayspaceController.Instance.goalPairs);
    }
}
