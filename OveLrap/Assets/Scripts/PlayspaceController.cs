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


    public void AddEntity(GameObject entity)
    {
        entities.Add(entity);
    }

    public void RemoveEntity(GameObject entity)
    {
        entities.Remove(entity);
    }

}
