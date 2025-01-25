using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSpawner : MonoBehaviour
{
    public GameObject bubblePrefab;
    public int secondsBetweenSpawn;
    public float randomSpawnDist;
    public List<GameObject> bubbles;
    float bubbleSpawnTimer = 0f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bubbleSpawnTimer += Time.deltaTime;
        if( bubbleSpawnTimer > secondsBetweenSpawn )
        {
            bubbleSpawnTimer = 0f;
            GameObject newBubble = Instantiate(bubblePrefab, transform);
            Vector3 newPos = new Vector3();
            newPos.x = Random.Range(-randomSpawnDist, randomSpawnDist);
            newPos.y = 5;
            newPos.z = Random.Range(-randomSpawnDist, randomSpawnDist);
            newBubble.transform.position = newPos;
            bubbles.Add(newBubble);
        }
    }
}
