using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    public AudioSource audioPlayer;

    [SerializeField]
    private float forceMagnitude;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.tag == "Alien" && !audioPlayer.isPlaying)
        {
            Debug.Log("Collision");
            audioPlayer.Play();
        }
        if (collision.tag == "Planet")
        {
            Debug.Log("Planet Collision");
        }
    }


}
