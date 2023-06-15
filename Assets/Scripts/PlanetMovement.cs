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
    private GameObject planetModel;
    private GameObject planetCircle;

    // Start is called before the first frame update
    void Start()
    {
        // Set planet init status to idle
        status = planetStatus.idle;

        // Initialize collision normal vector
        collisionNormal = Vector3.zero;

        // Get planet model
        planetModel = GameObject.FindGameObjectWithTag("Planet Model");

        // Get planet circle
        planetCircle = GameObject.FindGameObjectWithTag("Planet Circle");
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

                    // Declare booleans
                    // c = gameObject.GetComponent<CapsuleCollider>();
                    bool planet_link_collision = false;
                    bool out_of_bounds = new_position.x >= 5 && new_position.z >= 5 && new_position.x <= 95 && new_position.z <= 95;

                    // TODO: Check here if new position collides with a planet link or is out of bounds
                    if (!planet_link_collision && out_of_bounds)
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
                        // Set placed status
                        status = planetStatus.placed;

                        // Let GameStateManager know the platform is placed and perform actions consequently
                        GameStateManager.Instance.placePlatform(planetCode);

                        // Change planet animations
                        changePlanetAnimations();
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

    private void changePlanetAnimations()
    {
        // Deactivate target slot
        targetSlot.gameObject.SetActive(false);

        Debug.Log(planetCircle);
        Debug.Log(planetModel);

        // Get rotate component from planet model
        rotate rotateComponent = planetModel.GetComponent<rotate>();

        // Reset rotation speed
        rotateComponent.rotationSpeed = Vector3.zero;        

        // Get particle system from planet circle
        ParticleSystem circleParticleSystem = planetCircle.GetComponent<ParticleSystem>();

        // Access modules of the particle system
        var mainModule = circleParticleSystem.main;
        var trailsModule = circleParticleSystem.trails;

        // Declare white color
        var white = new ParticleSystem.MinMaxGradient(Color.white);

        // Set circle color to white
        mainModule.startColor = white;
        trailsModule.colorOverLifetime = white;
        trailsModule.colorOverTrail = white;
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
        // Once placed don't move 
        if (status != planetStatus.placed) 
        {
            status = planetStatus.idle;
        }
    }
}
