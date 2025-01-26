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
    public List<GameObject> Bubbles;

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

    public Dictionary<BubbleColors, int> getBubbleColorCounts()
    {//TODO
        return new Dictionary<BubbleColors, int>
        {
            { BubbleColors.Red, 10 },
            { BubbleColors.Blue, 15 },
            { BubbleColors.Green, 20 }
        };
    }
}
