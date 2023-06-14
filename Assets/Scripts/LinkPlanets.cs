using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPlanets : MonoBehaviour
{
    // Public attributes
    public GameObject go1;
    public GameObject go2;
    public int planetCode1;
    public int planetCode2;

    // Private attributes
    public bool active_link;
    private Color near;
    private Color far;
    private CapsuleCollider c;

    // Start is called before the first frame update
    void Start()
    {
        // Init private attributes
        active_link = false;
        near = Color.green;
        far = Color.red;
        c = gameObject.GetComponent<CapsuleCollider>();


        // Set collider properties
        c.direction = 2;
        c.radius = 0.7f;
        c.center = new Vector3(0, 0, 0);
        ColliderCreate();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameStateManager.Instance.CheckPlanetLink(planetCode1, planetCode2) == true)
        {
            active_link = true;
            gameObject.SetActive(true);

        }

    }


    void ColliderCreate()
    {
        Vector3 startPos = go1.transform.position;
        Vector3 endPos = go2.transform.position;

        Vector3 dir = endPos - startPos;  ///---this is vector that points from starting point to finish point
        float height = dir.magnitude;

        Vector3 dir2 = dir * 0.5f;
        Vector3 centerCoord = startPos + dir2;

        gameObject.transform.position = centerCoord;
        c.height = height;
        gameObject.transform.LookAt(go2.transform);
    }
}

