using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPlanets : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject go1;
    public GameObject go2;
    LineRenderer l;
    private Color near = Color.green;
    private Color far = Color.red;

    // Start is called before the first frame update
    void Start()
    {
        l = gameObject.AddComponent<LineRenderer>();
        l.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        CapsuleCollider capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        capsuleCollider.direction = 2;
        capsuleCollider.radius = 0.7f;
        capsuleCollider.center = new Vector3(0, 0, 0);
        setCollider();
    }

    // Update is called once per frame
    void Update()
    {
        //line render
        List<Vector3> pos = new List<Vector3>();
        pos.Add(go1.transform.position);
        pos.Add(go2.transform.position);
        l.startWidth = 0.5f;
        l.endWidth = 0.5f;
        l.SetPositions(pos.ToArray());
        l.useWorldSpace = true;
    }

    void setCollider()
    {
        Vector3 startPos = l.GetPosition(0);
        Vector3 endPos = l.GetPosition(1);

        Vector3 dir = endPos - startPos;  ///---this is vector that points from starting point to finish point
        float height = dir.magnitude;

        Vector3 dir2 = dir * 0.5f;
        Vector3 centerCoord = startPos + dir2;

        gameObject.transform.position = centerCoord;
        CapsuleCollider capsuleCollider = gameObject.GetComponent<CapsuleCollider>();
        capsuleCollider.height = height;
        gameObject.transform.LookAt(go2.transform);
    }
}
