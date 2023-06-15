using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPlanets : MonoBehaviour
{
    // Public attributes
    public int planetCode1;
    public int planetCode2;
    public bool active_link;

    // Start is called before the first frame update
    void Start()
    {
        active_link = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!active_link && GameStateManager.Instance.CheckPlanetLink(planetCode1, planetCode2) == true)
        {
            active_link = true;
            gameObject.SetActive(true);
        }

    }
}

