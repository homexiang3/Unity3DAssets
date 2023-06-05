using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetMovement : MonoBehaviour
{
    public int speed;
    private Vector3 collisionNormal;
    private bool colliding;
    // Start is called before the first frame update
    void Start()
    {
        collisionNormal = new Vector3(0, 0, 0);
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding)
        {
            Debug.Log("moving");
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.Translate(collisionNormal * step, Space.World);

        }
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (GameStateManager.Instance.playersNear)
        {
            colliding = true;
            var collisionPoint = other.ClosestPoint(transform.position);
            collisionNormal = transform.position - collisionPoint;
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
