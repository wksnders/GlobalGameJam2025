using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayspaceController : MonoBehaviour, ISingleton<PlayspaceController>
{
    private static PlayspaceController _inst;

    public static PlayspaceController Instance
    {
        get
        {
            if (_inst == null)
            {
                _inst = GameObject.FindObjectOfType<PlayspaceController>();
            }
            return _inst;
        }
    }
    private List<GameObject> entities = new List<GameObject>();
    public List<BubbleColor> bubbleColors;
    private Dictionary<BubbleColor, int> bubbleColorCounts = new Dictionary<BubbleColor, int>();
    [NonSerialized] public List<WordBubble> pairs = new List<WordBubble>();

    [NonSerialized] public int currentNumberOfPairs = 0;
    [Tooltip("number of pairs are needed to win.")]
    public int goalPairs = 10; // how many pairs are needed to win

    [Tooltip("starting number of placable agents.")]
    public int NumPlaceableAgents = 3;

    public void AddEntity(GameObject entity)
    {
        entities.Add(entity);
    }

    public void RemoveEntity(GameObject entity)
    {
        entities.Remove(entity);
    }

    public bool TrySpawnAgent() {
        if (NumPlaceableAgents <= 0) {
            NumPlaceableAgents = 0;
            return false;
        }
        NumPlaceableAgents--;
        return true;
    }
    
    // How many [Adjective + Noun] pairs are in the playspace
    // Happy Pairs, Angry Pairs, Sad Pairs
    public Dictionary<BubbleTone, int> getBubbleColorCounts() {
        Dictionary<BubbleTone, int> counts = new Dictionary<BubbleTone, int>();
        foreach (var bubbleColor in bubbleColorCounts) {
            if (counts.ContainsKey(bubbleColor.Key.tone)) {
                counts[bubbleColor.Key.tone] += bubbleColor.Value;
            } else {
                counts[bubbleColor.Key.tone] = bubbleColor.Value;
            }
        }
        return counts;
    }

    // Spawn a new bubble, like at start
    // Only counts PAIR types such as [Adjective + Noun]
    public void IncrementPairBubbleCount(WordBubble bubble) {
        if (GameManager.Instance.isGameOver) {
            return; //jank but now no more pairs happen after game over.
        }
        var color = bubble.currentColor;
        if (bubbleColorCounts.ContainsKey(color)) {
            bubbleColorCounts[color]++;
        } else {
            bubbleColorCounts[color] = 1;
        }
        currentNumberOfPairs++;
        pairs.Add(bubble);

        GameManager.Instance.CheckForWin();
    }
}

public enum BubbleTone {
    None,
    Happy,
    Angry,
    Sad,
}

[System.Serializable]
public class BubbleColor {
    public Color color;
    public BubbleTone tone;
}
