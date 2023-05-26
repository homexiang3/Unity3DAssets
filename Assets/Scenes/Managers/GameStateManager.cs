using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;
    private int _spaceParts = 0; //level 1
    //private int _shipBody = 0; //level 1
    private bool _platform1 = false; //level 2
    private bool _platform2 = false; // level 2
    private int _platformMatched = 0; //level 2
    private bool _level2Start = false; //level2
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
        //Debug
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
            SceneManager.LoadScene("Level3");
        }
    }

    public void AddSpacePart()//player moves part to ship template
    {
        _spaceParts += 1;
        if(_spaceParts== 4)
            SceneManager.LoadScene("Level2");
    }

    public void playerInPlatform1()//player collides with platform1 tag
    {
        _platform1 = true;
    }
    public void playerInPlatform2()//player collides with platform2 tag
    {
        _platform2 = true;
    }
    public void AddPlatform()//atempt to add a platform
    {
        if(_platform1 && _platform2)//si detecta a los dos jugadores en cada platform
        {
            _platform1 = false;
            _platform2 = false;
            _platformMatched += 1;
            if (_platformMatched == 2) //cambiar segun las que queramos
                SceneManager.LoadScene("Level3");
        }
        
    }
    public void RemovePlatform()//remove platform
    {
        _platformMatched -= 1;
    }

    public void AlienInShip()//start level 2 when alien is in ship
    {
        _level2Start = true;
    }
    
}
