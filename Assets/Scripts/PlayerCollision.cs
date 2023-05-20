using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public AudioSource audioPlayer;

    [SerializeField]
    private float forceMagnitude;
    private GameObject ObjectHolded;

    // Start is called before the first frame update
    void Start()
    {
        ObjectHolded = null; //Initially no object holded
    }

    // Update is called once per frame
    void Update()
    {
        if (ObjectHolded != null) // If an object is holded
        {
            ObjectHolded.transform.position = transform.position; // We move the object according to the player position
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        Debug.Log("isCollision");
        Debug.Log(collision.tag);

        if ((collision.tag == "Alien" || collision.tag == "Tree") && !audioPlayer.isPlaying)
        {
            Debug.Log("Collision");
            audioPlayer.Play();
        }
        if (collision.tag == "Planet")
        {
            Debug.Log("Planet Collision");
        }

        if (collision.tag == "ShipBody" || collision.tag == "ShipArms" || collision.tag == "ShipMotor" || collision.tag == "ShipEye") // Level1: Cuando player colisiona con ship
        {
            Debug.Log(collision.tag);
            ObjectHolded = collision.gameObject;
        }

        if (collision.tag == "ShipTemplate") // L1: Player next to the center (where ship should be placed)
        {
            if (ObjectHolded != null) //If player has an object
            {
                // Alpha channel of ship set to something
                for (int i = 0; i < collision.gameObject.transform.childCount; i++) // Loop through each childObject
                {
                    GameObject child = collision.gameObject.transform.GetChild(i).gameObject;
                    if (ObjectHolded.CompareTag(child.tag)) // If same tag
                    {
                        Debug.Log("Alpha");
                        break;
                    }
                }

                // Delete object holded
                Destroy(ObjectHolded);

                // Set ObjectHolded to null
                ObjectHolded = null;

            }
        }
    }


    public void OnTriggerExit(Collider collision) 
    {
        if(collision.tag == "ShipBody") //Quitamos efecto de "coger objeto"
        {

        }
    }


}
