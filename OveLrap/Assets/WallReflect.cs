using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallReflect : MonoBehaviour
{
    // Richochet the bubble off the wall
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<WordBubble>() == null) {
            return;
        }

        Rigidbody rb = other.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        // TODO richochet or reflect the bubble off the wall
        // currently just stops the bubble
        // currently the bubble is a trigger collider so it doesn't bounce off the floor, each other, or the agents
        
    }
}
