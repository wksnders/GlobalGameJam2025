using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, ISingleton<GameManager>
{
    private static GameManager _inst;
    public static GameManager Instance
    {
        get
        {
            if (_inst == null)
            {
                _inst = GameObject.FindObjectOfType<GameManager>();
            }
            return _inst;
        }
    }


    public void MakeBubble(GameObject bubble, Vector3 worldPos)
    {
        GameObject entity = Instantiate(bubble, worldPos, Quaternion.identity);
        //Add food to list
        PlayspaceController.Instance.AddEntity(entity);
    }
}
