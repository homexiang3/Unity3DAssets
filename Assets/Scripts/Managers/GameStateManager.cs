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
    // Instance
    private static GameStateManager instance;

    // Sound
    public AudioClip forest;
    public AudioClip space;
    public AudioClip addRing;
    public AudioClip deleteRing;
    public AudioClip platformSound;

    public AudioClip shipRepair;
    public AudioClip regularVictory;
    public AudioClip planetPlaced;
    public AudioClip finalVictory;


    public AudioClip disco; // Level 2 Music

    // Scene loader
    private bool loadScene1 = false;
    private bool loadScene2 = false;
    private bool loadScene3 = false;

    /////////////////// Level 1 \\\\\\\\\\\\\\\\\\\\\
    
    // Private attributes
    private int _spaceParts = 0; // Players score of Level1
    private int _shipBody = 0; //level 1
    private static List<Vector3> playersPosition = new List<Vector3>();

    /////////////////// Level 2 \\\\\\\\\\\\\\\\\\\\\
    
    // Public attributes
    public double _platformMatched = 0; // Players score of Level2
    public float _platformMaxTime = 5f; // Max time of platforms waiting before moving
    public double _platformMinScore = 4; // Minimum score to pass to the next level (level3)
    public double _platformPenalization = 0.5; // Points penalization in case of no matching
    public int wait = 1;
    public GameObject RingPrefab = null;

    // Private attributes
    private GameObject PlatformLeft = null;
    private GameObject PlatformRight = null;
    private GameObject RingParent = null;
    private bool _platformLeft = false; // If left platform has a player
    private bool _platformRight = false; // If right platform has a player
    private float _lastTime = 0f; // How much time has passed since
    private bool _level2Start = false;
    private GameObject[] RingList;
    private bool waitingOnCoroutine = false;
    private ParticleSystem.MinMaxGradient _originalSplashColor;

    /////////////////// Level 3 \\\\\\\\\\\\\\\\\\\\\
    
    // Enums
    private enum planets
    {
        Earth,
        Mars,
        Jupiter,
        Saturn,
        Moon,
    }

    // Private attributes
    private bool playersNear = false;
    private static int numPlanets = Enum.GetNames(typeof(planets)).Length;
    private static int numLinks = numPlanets - 1;
    private bool[] planetSlotStatus = new bool[numPlanets];
    private GameObject[] planetLinks;

    /////////////////// METHODS \\\\\\\\\\\\\\\\\\\\\

    // Singleton getter
    public static GameStateManager Instance
    {
        get
        {
            // If instance is null, find or create the GameStateManager in the scene
            if (instance == null)
            {
                instance = FindObjectOfType<GameStateManager>();

                // If not found, create a new GameObject in the current scene and attach the GameStateManager to it
                if (instance == null)
                {
                    // Set Game State Manager
                    GameObject managerObject = new GameObject("GameStateManager");
                    instance = managerObject.AddComponent<GameStateManager>();
                }
            }

            // Return GameStateManager
            return instance;
        }
    }

    // Awake is called during the initialization of the script or component
    void Awake()
    {
        // Make sure the GameStateManager persists between scene loads
        DontDestroyOnLoad(this.gameObject);

        // Load scene
        switch (SceneManager.GetActiveScene().name)
        {
            case "Level1":
                loadLevel1();
                break;
            case "Level2":
                loadLevel2();
                break;
            case "Level3":
                loadLevel3();
                break;
        }

        // Subscribe a callback to the scene loaded invocation list
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // When manager is destroyed
    private void OnDestroy()
    {
        // Remove callback from the scene loaded invocation list
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Update is called once per frame
    void Update()
    {
        // Sound of each level
        if(SceneManager.GetActiveScene().name == "Level1" && loadScene1 == false)
        {
            SoundManager.Instance.PlayMusic(forest, true);
            loadScene1 = true;
        }
        if (SceneManager.GetActiveScene().name == "Level2" && loadScene2 == false)
        {
            // SoundManager.Instance.PlayMusic(forest, true);
            loadScene2 = true;
        }
        if (SceneManager.GetActiveScene().name == "Level3" && loadScene3 == false)
        {
            SoundManager.Instance.PlayMusic(space, true);
            loadScene3 = true;
        }

        // Debug levels
        if (Input.GetKeyUp(KeyCode.F1))
        {
            SceneManager.LoadScene("Level1");
        }
        if (Input.GetKeyUp(KeyCode.F2))
        {
            SceneManager.LoadScene("Level2");
        }
        if (Input.GetKeyUp(KeyCode.F3))
        {
            _level2Start = false; //stop level2 coroutines
            SceneManager.LoadScene("Level3");
        }

        // Level 2: Update score and move platforms
        // Executed only if there is no Coroutine being performed
        if(_level2Start && waitingOnCoroutine == false && ((Time.time - _lastTime) > _platformMaxTime  ||  _platformLeft && _platformRight)) // If max waiting time has passed or players win
        {
            Debug.Log("Entering on coroutines");
            triggerLevel2Coroutines();
        }
    }

    // LEVEL 1 METHODS

    private void loadLevel1()
    {
        return;
    }

    public void AddSpacePart() // player moves part to ship template
    {
        SoundManager.Instance.Play(shipRepair);
        _spaceParts += 1;
        if(_spaceParts== 4)
        {
            SoundManager.Instance.Play(regularVictory);
            SceneManager.LoadScene("Level2");
        }
     }

    public void HoldingShipBody()
    {
      
        _shipBody += 1;
        Debug.Log(_shipBody);
    }

    public void DropShipBody()
    {
        _shipBody -= 1;
        // Avoid having negative score
        if (_shipBody < 0) _shipBody = 0;
        playersPosition.Clear();
        Debug.Log(_shipBody);
    }
    public int GetShipBody()
    {
        return _shipBody;
    }
    public Vector3 GetMiddlePoint(Vector3 player)
    {
        playersPosition.Add(player);

        Vector3 sum = new Vector3();
        //reset old position
        int count = playersPosition.Count;
        while (count > 2 ) {
        playersPosition.RemoveAt(0);
            count = playersPosition.Count;
        }

        foreach (Vector3 p in playersPosition)
        {
            sum += p;
        }
        return sum / 2;

    }

    // LEVEL 2 METHODS

    private void loadLevel2()
    {
        return;
    }

    public void AlienInShip() // start level 2 when alien is in ship
    {
        _level2Start = true;
        loadLevel2Vars();
        SoundManager.Instance.PlayMusic(disco, true); // Music SoundManager
    }
    public void triggerLevel2Coroutines()
    {
        // Set the boolean of the Coroutine handler to true
        waitingOnCoroutine = true;

        // Add points ONLY if players are above the platforms
        if (_platformLeft && _platformRight)
        {
            Debug.Log("Players win 1 score");

            // Sound Effect
            //SoundManager.Instance.Play(addRing);

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

            // Sound Effect
            SoundManager.Instance.Play(deleteRing);

            //RemovePlatformScore(); // Players lose score
            StartCoroutine(RemoveScoreCoroutine());
        }

        // Move platforms
        //MovePlatforms();

        // Update last time platforms moved
        //_lastTime = Time.time;
    }

    public void loadLevel2Vars()
    {
        Debug.Log("Level 2 STARTS: Search for the platforms!!");

        PlatformLeft = GameObject.FindWithTag("LeftPlatform");
        PlatformRight = GameObject.FindWithTag("RightPlatform");
        RingParent = GameObject.FindWithTag("RingParent");

        // Save splash color (MinMaxGradient)
        ParticleSystem particleSystem = PlatformLeft.transform.Find("Splash").GetComponent<ParticleSystem>();
        ParticleSystem.TrailModule SplashTrail = particleSystem.trails;
        _originalSplashColor = SplashTrail.colorOverTrail;

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
            ringShape.radius = 8 + 2.5f * i;

            // Set to HIDE
            newRing.SetActive(false);

            // Add to RingList
            RingList[i] = newRing;
        }
    }

    public void playerInPlatform(GameObject platformObject) // player collides with platform1 tag
    {
        if (platformObject == PlatformLeft)
        {
            _platformLeft = true;
            SoundManager.Instance.Play(platformSound); // Sound Effect
            Debug.Log("Player in LP");
        }
        else
        {
            _platformRight = true;
            SoundManager.Instance.Play(platformSound); // Sound Effect
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
            _level2Start = false;
            SoundManager.Instance.Play(regularVictory);
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

        // Sound Effect
        SoundManager.Instance.Play(addRing);

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

    // LEVEL 3 METHODS

    public void loadLevel3()
    {
            // Public attributes
    GameObject planetLinksParent = GameObject.FindWithTag("Tree"); 
    // Fetch all planet links
    planetLinks = UtilMethods.FindChildrenWithTag(planetLinksParent.transform, "Planet Link");
    }

    public void PlayersAreNear()
    {
        playersNear = true;
        
    }
    public void PlayersAreNotNear()
    {
        playersNear = false;
        
    }
    public bool isPlayerNear()
    {
        return playersNear;
    }

    public void checkPlanetLinks()
    {
        foreach (GameObject planetLink in planetLinks)
        {
            // Fetch planet link script
            PlanetLink planetLinkScript = planetLink.GetComponent<PlanetLink>();

            // Check if planet link related planets are already placed
            if (planetSlotStatus[planetLinkScript.originPlanet] && planetSlotStatus[planetLinkScript.finalPlanet])
            {
                launchPlanetLinkAnimations(planetLink);
            }
        }
    }

    public void placePlatform(int code)
    {
        // Get planet code
        planets planetCode = (planets) code;

        // Check planet code
        switch (planetCode)
        {
            case planets.Earth:
                planetSlotStatus[(int)planets.Earth] = true;
                break;
            case planets.Mars:
                planetSlotStatus[(int)planets.Mars] = true;
                break;
            case planets.Jupiter:
                planetSlotStatus[(int)planets.Jupiter] = true;
                break;
            case planets.Saturn:
                planetSlotStatus[(int)planets.Saturn] = true;
                break;
            case planets.Moon:
                planetSlotStatus[(int)planets.Moon] = true;
                break;
            default:
                Debug.Log("Wrong platform code");
                break;
        }

        // Planet planet placing sound
        SoundManager.Instance.Play(planetPlaced);

        // Check planet links
        checkPlanetLinks();

        // Check win condition
        checkWinCondition();
    }

    private void launchPlanetLinkAnimations(GameObject planetLink)
    {
        // Set planet link to active
        planetLink.SetActive(true);
    }

    private void checkWinCondition()
    {
        if (Array.TrueForAll(planetSlotStatus, planetSlotStatus => planetSlotStatus == true))
        {
            SoundManager.Instance.PlayMusic(finalVictory, false);
        }
    }

    // Callbacks
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            case "Level1":
                loadLevel1();
                break;
            case "Level2":
                loadLevel2();
                break;
            case "Level3":
                loadLevel3();
                break;

        }
    }


}

