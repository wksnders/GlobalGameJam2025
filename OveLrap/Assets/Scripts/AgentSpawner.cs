using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    public GameObject agentPrefab;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create a ray from mouse click
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) // Check if ray hits a collider
            {
                if (hit.collider.tag == "Floor")
                {
                    // Spawn object at hit point
                    Vector3 newPos = new Vector3();
                    newPos.x = hit.point.x;
                    newPos.y = hit.point.y + 20f;
                    newPos.z = hit.point.z;
                    GameObject newAgent = Instantiate(agentPrefab, transform);
                    newAgent.transform.position = newPos;
                }
            }
        }
    }
}
