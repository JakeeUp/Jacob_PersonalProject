using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float travelTime;
    private Vector3 currentPos;

    private Rigidbody rb;
    private GameObject player;  // Reference to the player object with Rigidbody.

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void FixedUpdate()
    {
        currentPos = Vector3.Lerp(startPoint.position, endPoint.position,
            Mathf.Cos(Time.time / travelTime * Mathf.PI * 2) * -0.5f + 0.5f);
        rb.MovePosition(currentPos);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;  // Assign the player object.
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (player != null && other.CompareTag("Player"))
        {
            // Calculate the difference in position between the platform and the player.
            Vector3 platformDelta = currentPos - transform.position;

            // Move the player's Rigidbody using velocity.
            player.GetComponent<Rigidbody>().velocity += platformDelta;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = null;  // Clear the reference when the player exits the trigger zone.
        }
    }
}
