using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgentWind : MonoBehaviour
{
    [HideInInspector]
    public WordSpawner wordBubbleSpawner;

    public float pushLength = 20;
    public float pushWidth = 2;
    public float pushStrength = 2;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // scan forward and find bubbles
        Vector3 forward = transform.forward.normalized;
        RaycastHit[] hits = Physics.RaycastAll(new Ray(transform.position, forward), pushLength/*, ~(1 << 3)*/);
        if( hits.Length > 0 )
        {
            // push them
            foreach( RaycastHit hit in hits )
            {
                hit.rigidbody.AddForce(forward * pushStrength);
            }
        }
    }
}
