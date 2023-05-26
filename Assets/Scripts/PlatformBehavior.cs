using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class PlatformBehavior : MonoBehaviour
{
    //public float[] xRange = { 5f, 95f };
    public float[] xRange = { -55f, 36f };
    //public float[] zRange = { 5f, 95f };
    public float[] zRange = { -55f, 36f };
    public float lastTime = 0f;
    public string playerAbove;
    public GameObject[] platforms; //GameObject.FindGameObjectsWithTag("Platform");
    public int maxTime = 60000;

    // Start is called before the first frame update
    void Start()
    {
        platforms = new GameObject[2];
        platforms[0] = GameObject.Find("platformRight");
        platforms[1] = GameObject.Find("platformLeft");
        lastTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Platforms list");
        //Debug.Log(platforms);

        bool allPlayersAbove = true;
        //string[] playersAbovePlatforms = null;
        //int i = 0;

        //See if all platforms have players above
        foreach (GameObject platform in platforms)
        {
            //Debug.Log("inside foreach");
            string pp = platform.GetComponent<PlatformBehavior>().playerAbove;
            //playersAbovePlatforms[i] = pp;
            //i++;
            //Debug.Log(pp);

            //If one platform does not have a player, stop
            if (System.String.IsNullOrEmpty(pp)) 
            {
                //Debug.Log("Empty platform");
                allPlayersAbove = false;
                break;
            } 
        }

        //If the time is over or there are players above all platforms
        if (Time.time - lastTime > maxTime || allPlayersAbove == true)
        {
            Debug.Log("MOVE PLATFORMS " + allPlayersAbove);
            //Move the platform
            movePosition();

            //Update lastTime
            lastTime = Time.time;

            //Reset playerAbove
            playerAbove = null;


            //Update success counter
            if (allPlayersAbove == true) //Win points
            {
                //TO DO: Set win music
                Debug.Log("Players Win");
            }
            else //Lose points
            {
                //TO DO: Set music
                Debug.Log("Players Loose");
            }
        }
    }

    public void OnTriggerStay(Collider collision)
    {
        //If user stays above
        if(collision.gameObject.name == "Player1" || collision.gameObject.name == "Player2")
        {
            if(System.String.IsNullOrEmpty(playerAbove)) //If no other user was there before
            {
                playerAbove = collision.gameObject.name; //Save the user tag
            }
        }
    }

    public void OnTriggerExit(Collider collision)
    {
        //If the user exits
        if (collision.gameObject.name == "Player1" || collision.gameObject.name == "Player2")
        {
            playerAbove = null;
        }
    }

    void movePosition()
    {
        //Get the range of possible x and z values
        float x_down = xRange[0];
        float x_up = xRange[1];
        float z_down;
        float z_up;

        //See which platform it is to get the correct z range
        if ( gameObject.tag == "platformLeft")
        {
            z_down = xRange[0]; //5
            z_up = (xRange[1] - xRange[0]) / 2; //40
        } 
        else
        {
            z_down = xRange[0] + (xRange[1] - xRange[0]) / 2; //45
            z_up = xRange[1]; //95
        }


        //Generate a random position from the corresponding range
        var randPos = new Vector3(Random.Range(x_down, x_up), 0, Random.Range(z_down, z_up));
        //var randPos = new Vector3(Random.Range(10.5f, 95.5f), 0, Random.Range(5f, 95f));

        //Change the position of this platform
        transform.position = randPos;

        Debug.Log("Moving position of " + gameObject.tag + " position: " + randPos);
    }
}
