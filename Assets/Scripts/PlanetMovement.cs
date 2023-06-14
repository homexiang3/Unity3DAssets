using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    // Enums
    private enum planetStatus
    {
        idle,
        moving,
        placing,
        placed
    }

    // Public attributes
    public int speed;
    public int planetCode;
    public GameObject targetSlot;

    // Private attributes
    private planetStatus status;
    private Vector3 collisionNormal;

    // Start is called before the first frame update
    void Start()
    {
        status = planetStatus.idle;
        collisionNormal = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        switch(status)
        {
            // Don't do anything
            case planetStatus.placed:
                break;

            // Move the planet in the direction of the normal to the player link
            case planetStatus.moving:
                {
                    // Calculate distance to move
                    var step = speed * Time.deltaTime;

                    // Predict new position
                    var move_vector = collisionNormal * step;
                    var new_position = transform.position + move_vector;

                    // TODO: Check here if new position collides with a planet link or is out of bounds
                    if (new_position.x >= 5 && new_position.z >= 5 && new_position.x <= 95 && new_position.z <= 95)//out of bounds
                    {
                        // Set new position
                        transform.Translate(move_vector, Space.World);
                    }

                    // Leave
                    break;
                }

            // Start placing animation
            case planetStatus.placing:
                {
                    // Lerp planet position through targetSlot position
                    transform.position = Vector3.Lerp(transform.position, targetSlot.transform.position, Time.deltaTime);

                    // Get distance between planet and targetSlot
                    float distance = Vector3.Distance(transform.position, targetSlot.transform.position);

                    // Check placed state
                    if (distance < 0.3f)
                    {
                        status = planetStatus.placed;
                        GameStateManager.Instance.placePlatform(planetCode);
                    }

                    // Leave
                    break;
                }
        }
    }

    // Methods
    private Vector3 getCollisionNormal(Collider collider)
    {
        Vector3 collisionPoint = collider.ClosestPoint(transform.position);
        Vector3 normal = Vector3.Scale(new Vector3(1,0,1), (transform.position - collisionPoint));
        return normal;
    }

    // Triggers
    void OnTriggerEnter(Collider other)
    {
        // Get gameobject
        GameObject collision = other.gameObject;

        // Check targetSlot collision
        if (collision == targetSlot)
            status = planetStatus.placing;
        
        // Check player link collision
        if (collision.tag == "Player Link" && GameStateManager.Instance.isPlayerNear() && status == planetStatus.idle)
        {
            status = planetStatus.moving;
            collisionNormal = getCollisionNormal(other);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (status != planetStatus.placed) //once placed don't move 
        {
            status = planetStatus.idle;
        }
    }
}
