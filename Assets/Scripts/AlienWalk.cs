using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienWalk : MonoBehaviour
{
    public int speed;
    private bool isMovingLeft = false;
    private bool rotating = false;

    private float rotationTime = 2f;
    private float rotationSpeed = 3f;

    private Quaternion newRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!rotating)
        {
            rotating = true;
            StartCoroutine(RotateAlien());
            rotating = false;
        }

        if (isMovingLeft)
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y, transform.position.z - speed * Time.deltaTime);

            if (transform.position.z <= 5)
            {
                isMovingLeft = false;
                newRotation = Quaternion.Euler(320f, 0f, 0f);
            }

        }
        else
        {
            transform.position = new Vector3(transform.position.x,
                transform.position.y, transform.position.z + speed * Time.deltaTime);

            if (transform.position.z >= 95)
            {
                isMovingLeft = true;
                newRotation = Quaternion.Euler(320f, 180f, 0f);
            }
            
        }
    }

    private IEnumerator RotateAlien()
    {
        float t = Mathf.Clamp(Time.deltaTime * rotationSpeed, 0f, 0.99f);
        transform.rotation = Quaternion.Lerp(transform.rotation, newRotation, t);
        yield return new WaitForSeconds(rotationTime);
    }
}
