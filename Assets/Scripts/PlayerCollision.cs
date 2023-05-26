using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public AudioSource audioPlayer;

    private GameObject ObjectHolded;
    public Material Material;
    public Material Material001;
    public Material Material002;
    public Material Material003;

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
        //Alien
        if (collision.tag == "Alien"  && !audioPlayer.isPlaying)
        {
            Debug.Log("Alien is Happy");
            audioPlayer.Play();
        }
        if (collision.tag == "Planet")
        {
            Debug.Log("Planet Collision");
        }
        //L1: SHIP PIECES
        if (collision.tag == "ShipBody" || collision.tag == "ShipArms" || collision.tag == "ShipMotor" || collision.tag == "ShipEye") // Level1: Cuando player colisiona con piezas del ship
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
                    if (ObjectHolded.CompareTag(child.tag)) // Find a match 
                    {
                        GameStateManager.Instance.AddSpacePart();
                        switch (child.tag)
                        {
                            case "ShipBody": //change body material
                                GameObject body = child.gameObject.transform.GetChild(0).gameObject;
                                GameObject belt = child.gameObject.transform.GetChild(1).gameObject;
                                GameObject head = child.gameObject.transform.GetChild(2).gameObject;
                                body.GetComponent<Renderer>().material = Material;
                                belt.GetComponent<Renderer>().material = Material001;
                                head.GetComponent<Renderer>().material = Material002;
                                break;
                            case "ShipArms":
                                GameObject arm = child.gameObject.transform.GetChild(0).gameObject;
                                arm.GetComponent<Renderer>().material = Material002; //Change arm material
                                break;
                            case "ShipMotor":
                                GameObject motor = child.gameObject.transform.GetChild(0).gameObject;
                                motor.GetComponent<Renderer>().material = Material001; //Change ship material
                                break;
                            case "ShipEye": //change eye material
                                GameObject eye1 = child.gameObject.transform.GetChild(0).gameObject;
                                GameObject eye2 = child.gameObject.transform.GetChild(1).gameObject;
                                GameObject eye3 = child.gameObject.transform.GetChild(2).gameObject;
                                eye1.GetComponent<Renderer>().material = Material001;
                                eye2.GetComponent<Renderer>().material = Material003;
                                eye3.GetComponent<Renderer>().material = Material002;
                                break;
                            default:
                                Debug.Log("Incorrect ship tag");
                                break;
                        }
                        break; //stop iterating over other ship childs
                    }
                }

                // Delete object holded
                Destroy(ObjectHolded);

                // Set ObjectHolded to null
                ObjectHolded = null;

            }
        }
    }

    //De momento no se usa
    public void OnTriggerExit(Collider collision) 
    {
        if(collision.tag == "ShipBody") //Quitamos efecto de "coger objeto"
        {

        }
    }


}
