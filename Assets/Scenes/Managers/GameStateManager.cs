using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    private static GameStateManager _instance;
    private bool _level1 = false;
    private bool _level2 = false;
    private bool _level3 = false;
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
}
