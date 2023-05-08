using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienWalk : MonoBehaviour
{
    public int speed;
    private bool isMovingLeft = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMovingLeft)
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y, transform.position.z - speed * Time.deltaTime);

            if (transform.position.z <= 5)
            {
                isMovingLeft = false;
            }
        }
        else
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y, transform.position.z + speed * Time.deltaTime);

            if (transform.position.z >= 95)
            {
                isMovingLeft = true;
            }
        }
    }
}
