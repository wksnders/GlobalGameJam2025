using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentSpawner : MonoBehaviour
{
    public WordSpawner wordSpawner;

    public GameObject agentPrefab;
    public float dragRotateSlerpSpeed;
    public List<GameObject> agents;

    bool isDragging = false;
    GameObject dragAgent;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))// && PlayspaceController.Instance.TrySpawnAgent())//need to add PlayspaceController to scene
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create a ray from mouse down
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit/*, ~(1 << 6)*/)) // Check if ray hits the collider on the floor layer
            {
                //if (hit.collider.tag == "Floor")
                {
                    // Spawn object at adjusted hit point
                    Vector3 newPos = new Vector3();
                    newPos.x = hit.point.x;
                    newPos.y = hit.point.y;
                    newPos.z = hit.point.z;
                    dragAgent = Instantiate(agentPrefab, transform);
                    dragAgent.GetComponent<AgentWind>().wordBubbleSpawner = wordSpawner;
                    dragAgent.transform.position = newPos;
                    agents.Add(dragAgent);
                    isDragging = true;
                }
            }
        }

        if (isDragging)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Create a ray from mouse up
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit/*, ~(1 << 6)*/)) // Check if ray hits a collider
            {
                //if (hit.collider.tag == "Floor")
                {
                    // find the vector pointing from our position to the adjusted drag hit pos
                    Vector3 hitPos = new Vector3();
                    hitPos.x = hit.point.x;
                    hitPos.y = dragAgent.transform.position.y;
                    hitPos.z = hit.point.z;
                    Vector3 direction = (hitPos - dragAgent.transform.position).normalized;

                    //create the rotation we need to be in to look at the target
                    Quaternion lookRotation = Quaternion.LookRotation(direction);

                    //rotate us over time according to speed until we are in the required rotation
                    dragAgent.transform.rotation = Quaternion.Slerp(dragAgent.transform.rotation, lookRotation, Time.deltaTime * dragRotateSlerpSpeed);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
        }
    }
}
