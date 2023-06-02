using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PlatformBehavior : MonoBehaviour
{
    public float[] xRange = { 9f, 93f };
    public float[] zRange = { 9f, 93f };
    public string playerAbove;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerStay(Collider collision)
    {
        //If user stays above
        if(collision.gameObject.name == "Player1" || collision.gameObject.name == "Player2")
        {
            if(System.String.IsNullOrEmpty(playerAbove)) //If no other user was there before
            {
                //Save the user tag
                playerAbove = collision.gameObject.name;

                //GameStateManager handling
                GameStateManager.Instance.playerInPlatform(gameObject);
            }
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        //If the user exits
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "Player2")
        {
            // if(collision.gameObject.name == playerAbove) //Gestionar que solo si primer player se va => Not NEED PQ tenemos triggerStay
            playerAbove = null;

            //GameStateManager handling
            GameStateManager.Instance.noPlayerInPlatform(gameObject);
        }
    }

    public void movePosition()
    {
        //Get the range of possible x and z values
        float x_down = xRange[0];
        float x_up = xRange[1];
        float z_down;
        float z_up;

        //See which platform it is to get the correct z range
        if ( gameObject.name == "platformLeft")
        {
            z_down = zRange[0];
            z_up = (zRange[1] - zRange[0]) / 2;
        } 
        else
        {
            z_down = zRange[0] + (zRange[1] - zRange[0]) / 2;
            z_up = zRange[1];
        }


        //Generate a random position from the corresponding range
        var randPos = new Vector3(Random.Range(x_down, x_up), -20, Random.Range(z_down, z_up));

        //Change the position of this platform
        transform.position = randPos;

        Debug.Log("Moving position of " + gameObject.tag + " position: " + randPos);
    }
}
