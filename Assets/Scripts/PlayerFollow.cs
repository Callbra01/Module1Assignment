using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Brandon Callaway | 000954560
public class PlayerFollow : MonoBehaviour
{
    private Transform target;
    public Vector3 offset;
    public float smoothFactor = 2f;

    // Find Player gameobject
    void Start()
    {
        // Set target var to player gameobject
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Lerp camera position to player position
    void LateUpdate()
    {
        // Store target position
        Vector3 targetPos = new Vector3(target.position.x, target.position.y, transform.position.z);

        // Lerp the position of the camera to the players position
        // smoothFactor defines the speed at which we step in the Lerp
        transform.position = Vector3.Lerp(transform.position, targetPos + offset, Time.deltaTime * smoothFactor);
    }
}
