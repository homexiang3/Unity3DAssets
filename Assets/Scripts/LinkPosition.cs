using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPosition : MonoBehaviour
{
    // Public attributes
    public GameObject go1;
    public GameObject go2;
    public float max_distance;

    // Private attributes
    private float distance;
    private Color near;
    private Color far;
    private LineRenderer l;
    private CapsuleCollider c;

    // Start is called before the first frame update
    void Start()
    {
        // Init private attributes
        distance = 0.0f;
        near = Color.green;
        far = Color.red;
        l = gameObject.GetComponent<LineRenderer>();
        c = gameObject.GetComponent<CapsuleCollider>();

        // Set line renderer properties
        l.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));

        // Set collider properties
        c.direction = 2;
        c.radius = 0.7f;
        c.center = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(go1.transform.position, go2.transform.position);
        if(distance < max_distance)
        {
            l.startColor = near;
            l.endColor = near;
            GameStateManager.Instance.PlayersAreNear();

        }
        else
        {
            l.startColor = far;
            l.endColor = far;
            GameStateManager.Instance.PlayersAreNotNear();
        }

        List<Vector3> pos = new List<Vector3>();
        pos.Add(go1.transform.position);
        pos.Add(go2.transform.position);
        l.startWidth = 0.5f;
        l.endWidth = 0.5f;
        l.SetPositions(pos.ToArray());
        l.useWorldSpace = true;
        ColliderUpdate();
    }


    void ColliderUpdate()
    {
        Vector3 startPos = l.GetPosition(0);
        Vector3 endPos = l.GetPosition(1);

        Vector3 dir = endPos - startPos;  ///---this is vector that points from starting point to finish point
        float height = dir.magnitude;

        Vector3 dir2 = dir * 0.5f;
        Vector3 centerCoord = startPos + dir2;

        gameObject.transform.position = centerCoord;
        c.height = height;
        gameObject.transform.LookAt(go2.transform);
    }
}
