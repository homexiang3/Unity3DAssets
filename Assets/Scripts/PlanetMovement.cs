using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    public int speed;
    private Vector3 collisionNormal;
    private bool colliding;
    private bool placing;
    private bool placed;
    public GameObject platformTarget;
    public int planetCode;
    // Start is called before the first frame update
    void Start()
    {
        collisionNormal = new Vector3(0, 0, 0);
        colliding = false;
        placing = false;
        placed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!placed)
        {
            if (colliding)
            {
                Debug.Log("moving");
                var step = speed * Time.deltaTime; // calculate distance to move
                var new_position = collisionNormal * step;
                // check here if new position is an already linked planet collider
                //then update the value
                transform.Translate(new_position, Space.World);

            }
            if (placing)
            {
                transform.position = Vector3.Lerp(transform.position, platformTarget.transform.position, Time.deltaTime);
            }
            if (Vector3.Distance(transform.position,platformTarget.transform.position) < 0.3f)
            {
                placed = true;
                GameStateManager.Instance.placePlatform(planetCode);
            }
        }
        
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Planet")
        {
            placing = true;
            Debug.Log("Planet Collision");
        }
        if (GameStateManager.Instance.isPlayerNear())
        {
            colliding = true;
            var collisionPoint = other.ClosestPoint(transform.position);
            collisionNormal = transform.position - collisionPoint;
            collisionNormal.y = 0; //avoid vertical movement
            // Print how many points are colliding with this transform
            Debug.Log(collisionNormal);
        }
       

    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        colliding = false;

    }
}
