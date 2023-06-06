using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;

    //Level 1
    private int _spaceParts = 0; // Players score of Level1
    //private int _shipBody = 0; //level 1

    //Level 2 
    public GameObject PlatformLeft;
    public GameObject PlatformRight;
    private bool _platformLeft = false; // If left platform has a player
    private bool _platformRight = false; // If right platform has a player
    public double _platformMatched = 0; // Players score of Level2
    public float _platformMaxTime = 15f; // Max time of platforms waiting before moving
    public double _platformMinScore = 4; // Minimum score to pass to the next level (level3)
    public double _platformPenalization = 0.5; // Points penalization in case of no matching
    private float _lastTime = 0f; // How much time has passed since
    private bool _level2Start = false;
    public GameObject RingParent;
    public GameObject RingPrefab;
    private GameObject[] RingList;
    public int wait = 60;
    private bool waitingOnCoroutine = false;
    private ParticleSystem.MinMaxGradient _originalSplashColor;



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

            // Save splash color (MinMaxGradient)
            ParticleSystem particleSystem = PlatformLeft.transform.Find("Splash").GetComponent<ParticleSystem>();
            ParticleSystem.TrailModule SplashTrail = particleSystem.trails;
            _originalSplashColor = SplashTrail.colorOverTrail;
       
            // Prevents to repeat this part again (we only need to do it once)
            _level2Start = true;

            //Move plaforms to get a random first position
            MovePlatforms();

            //Set _lastTime to current time
            _lastTime = Time.time;

            // Create rings
            RingList = new GameObject[(int)_platformMinScore];

            for (int i = 0; i < _platformMinScore; i++)
            {
                // Create a new Ring, with name ring<i+1>
                var newRing = Instantiate(RingPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newRing.name = "ring" + (i + 1);

                // Set correct position, rotation and parent
                newRing.transform.position = RingParent.transform.position;
                newRing.transform.rotation = RingParent.transform.rotation;
                newRing.transform.parent = RingParent.transform;

                // Change ring radius
                ParticleSystem particleSystemRing = newRing.GetComponent<ParticleSystem>();
                ParticleSystem.ShapeModule ringShape = particleSystemRing.shape;
                ringShape.radius = 8 + 2.5f*i;

                // Set to HIDE
                newRing.SetActive(false);

                // Add to RingList
                RingList[i] = newRing;
            }
        }


        // Level 2: Update score and move platforms
        // Executed only if there is no Coroutine being performed
        if(waitingOnCoroutine == false && (Time.time - _lastTime > _platformMaxTime  ||  _platformLeft && _platformRight)) // If max waiting time has passed or players win
        {
            // Set the boolean of the Coroutine handler to true
            waitingOnCoroutine = true;

            // Add points ONLY if players are above the platforms
            if (_platformLeft && _platformRight)
            {
                Debug.Log("Players win 1 score");

                // Change Splash color to blue (win)
                ParticleSystem particleSystem = PlatformLeft.transform.Find("Splash").GetComponent<ParticleSystem>();
                ParticleSystem.TrailModule SplashTrail = particleSystem.trails;
                SplashTrail.colorOverTrail = new UnityEngine.Color(255, 255, 255, 255); //

                particleSystem = PlatformRight.transform.Find("Splash").GetComponent<ParticleSystem>();
                SplashTrail = particleSystem.trails;
                SplashTrail.colorOverTrail = new UnityEngine.Color(255, 255, 255, 255); //18, 99, 255, 255

                //AddPlatformScore(); // Players win score
                StartCoroutine(AddScoreCoroutine());
            }
            else
            {
                Debug.Log("Players lose score");
                //RemovePlatformScore(); // Players lose score
                StartCoroutine(RemoveScoreCoroutine());
            }

            // Move platforms
            //MovePlatforms();

            // Update last time platforms moved
            //_lastTime = Time.time;
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
    public void playerInPlatform(GameObject platformObject) // player collides with platform1 tag
    {
        if (platformObject == PlatformLeft)
        {
            _platformLeft = true;
            Debug.Log("Player in LP");
        }
        else
        {
            _platformRight = true;
            Debug.Log("Player in RP");
        }

        // Fetch and activate splash animation
        GameObject splash = platformObject.transform.Find("Splash").gameObject;
        splash.SetActive(true);

    }
    public void noPlayerInPlatform(GameObject platformObject) // player collides with platforms
    {
        if(platformObject == PlatformLeft)
        {
            _platformLeft = false;
            Debug.Log("Player left LP");
        }
        else
        {
            _platformRight = false;
            Debug.Log("Player left RP");
        }

        // Fetch and activate splash animation
        GameObject splash = platformObject.transform.Find("Splash").gameObject;
        splash.SetActive(false);

    }
    public void MovePlatforms() // move all platforms
    {
        PlatformLeft.GetComponent<PlatformBehavior>().movePosition(); // For Left platform
        PlatformRight.GetComponent<PlatformBehavior>().movePosition(); // For right platform
    }
    public void updateLevel2Parameters() //move platforms and update the last time platforms moved
    {
        //Move the platforms into another random location
        MovePlatforms();

        //Update last time platforms moved
        _lastTime = Time.time;
    }
    public void AddPlatformScore() // Add points and check if change of scene is needed
    {
        // Set platforms to false (no player above)
        _platformLeft = false;
        _platformRight = false;

        // SHOW ring BEFORE adding points (because of the index which starts at 0)
        RingList[(int)_platformMatched].SetActive(true);

        // Add one point to the score
        _platformMatched += 1;
        Debug.Log("SCORE " + _platformMatched);

        // Change to LEVEL 3 if completed
        if (_platformMatched >= _platformMinScore) // If players have achieved enough points
        {
            Debug.Log("Level 2 COMPLETED");
            SceneManager.LoadScene("Level3"); // Load scene 3
        }
        else
        {
            //Change the platforms position and update the lastTime parameter
            updateLevel2Parameters();
        }
    }
    public void RemovePlatformScore()// Remove points
    {
        //Save previos score
        double previousScore = _platformMatched;

        // Decrease score
        _platformMatched -= _platformPenalization;

        // Avoid having negative score
        if (_platformMatched < 0) _platformMatched = 0;

        //Check if the newScore implies the lose of a ring
        if ((int)_platformMatched < (int)previousScore)
        {
            RingList[(int)_platformMatched].SetActive(false); //HIDE ring
        }

        //Change the platforms position and update the lastTime parameter
        updateLevel2Parameters();
    }
    IEnumerator AddScoreCoroutine()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(wait);

        // Call to AddPlatformScore
        AddPlatformScore();

        // Return to normal color
        ParticleSystem particleSystem = PlatformLeft.transform.Find("Splash").GetComponent<ParticleSystem>();
        ParticleSystem.TrailModule SplashTrail = particleSystem.trails;
        SplashTrail.colorOverTrail = _originalSplashColor;

        particleSystem = PlatformRight.transform.Find("Splash").GetComponent<ParticleSystem>();
        SplashTrail = particleSystem.trails;
        SplashTrail.colorOverTrail = _originalSplashColor;

        // Set the boolean of the Coroutine handler to false
        waitingOnCoroutine = false;
    }
    IEnumerator RemoveScoreCoroutine()
    {
        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(wait);

        // Call to RemovePlatformScore
        RemovePlatformScore();

        // Set the boolean of the Coroutine handler to false
        waitingOnCoroutine = false;
    }
    public void changePlatformsSplashColor() // NOT BEING USED
    {
        //For platform left
        ParticleSystem particleSystem = PlatformLeft.transform.Find("Splash").GetComponent<ParticleSystem>();
        ParticleSystem.TrailModule SplashTrail = particleSystem.trails;
        //SplashTrail.colorOverTrail = color;

        //For platform right
        particleSystem = PlatformRight.transform.Find("Splash").GetComponent<ParticleSystem>();
        SplashTrail = particleSystem.trails;
        //SplashTrail.colorOverTrail = color;
    }

}
