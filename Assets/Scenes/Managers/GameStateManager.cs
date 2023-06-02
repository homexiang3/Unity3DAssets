using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;

    //Level 1
    private int _spaceParts = 0; // Players score of Level1
    //private int _shipBody = 0; //level 1

    //Level 2 
    private bool _platformLeft = false; // If left platform has a player
    private bool _platformRight = false; // If right platform has a player
    public double _platformMatched = 0; // Players score of Level2
    public float _platformMaxTime = 60f; // Max time of platforms waiting before moving
    public double _platformMinScore = 4; // Minimum score to pass to the next level (level3)
    public double _platformPenalization = 0.3; // Points penalization in case of no matching
    private float _lastTime = 0f; // How much time has passed since
    private bool _level2Start = false; 
    public GameObject[] platforms; // GameObject.FindGameObjectsWithTag("Platform");



    // Start is called before the first frame update
    public static GameStateManager Instance
    {
        get
        {
            if (_instance is null)
                Debug.LogError("Game State Manager is NULL");
            return _instance;
        }

    }
    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        // Debug
        if (Input.GetKeyUp(KeyCode.F1))
        {
            SceneManager.LoadScene("Level1");
        }
        if (Input.GetKeyUp(KeyCode.F2))
        {
            SceneManager.LoadScene("Level2");
            _level2Start = false; // Handle platforms find
        }
        if (Input.GetKeyUp(KeyCode.F3))
        {
            SceneManager.LoadScene("Level3");
        }


        // Loevel 2: load variables
        if (_level2Start == false && SceneManager.GetActiveScene().name == "Level2")
        {
            Debug.Log("Level 2 STARTS: Search for the platforms!!");

            // Get platforms
            platforms = new GameObject[2];
            platforms[0] = GameObject.Find("platformRight");
            platforms[1] = GameObject.Find("platformLeft");

            // Prevents to repeat this part again (we only need to do it once)
            _level2Start = true;

            //Move plaforms to get a random first position
            MovePlatforms();

            //Set _lastTime to current time
            _lastTime = Time.time;
        }


        // Level 2: Update score and move platforms
        if(Time.time - _lastTime > _platformMaxTime  ||  _platformLeft && _platformRight) // If max waiting time has passed or players win
        {
            // Add points ONLY if players are above the platforms
            if (_platformLeft && _platformRight)
            {
                Debug.Log("Players win 1 score");
                AddPlatformScore(); // Players win score
            }
            else
            {
                Debug.Log("Players lose score");
                RemovePlatformScore(); // Players lose score
            }

            // Move platforms
            MovePlatforms();

            // Update last time platforms moved
            _lastTime = Time.time;
        }
    }

    // LEVEL 1 FUNCTIONS
    public void AddSpacePart() // player moves part to ship template
    {
        _spaceParts += 1;
        if(_spaceParts== 4)
            SceneManager.LoadScene("Level2");
    }


    // LEVEL 2 FUNCTIONS
    public void AlienInShip() // start level 2 when alien is in ship
    {
        _level2Start = true;
    }
    public void playerInPlatformLeft() // player collides with platform1 tag
    {
        _platformLeft = true;
        Debug.Log("Player in LP");
    }
    public void playerInPlatformRight() // player collides with platform2 tag
    {
        _platformRight = true;
        Debug.Log("Player in RP");
    }
    public void noPlayerInPlatformLeft() // player collides with platform1 tag
    {
        _platformLeft = false;
        Debug.Log("Player left LP");
    }
    public void noPlayerInPlatformRight() // player collides with platform2 tag
    {
        _platformRight = false;
        Debug.Log("Player left RP");
    }
    public void AddPlatformScore() // Add points and check if change of scene is needed
    {
        _platformLeft = false;
        _platformRight = false;
        _platformMatched += 1;
        Debug.Log("SCORE " + _platformMatched);

        if (_platformMatched >= _platformMinScore) // If players have achieved enough points
        {
            Debug.Log("Level 2 COMPLETED");
            SceneManager.LoadScene("Level3"); // Load scene 3
        }
            

    }
    public void RemovePlatformScore()// Remove points
    {
        _platformMatched -= 0.5;
        if (_platformMatched < 0) _platformMatched = 0;
    }

    public void MovePlatforms()// move all platforms
    {
        foreach (GameObject platform in platforms)
        {
            platform.GetComponent<PlatformBehavior>().movePosition();
        }
        
    }

}
