using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPosition : MonoBehaviour

{
    // Start is called before the first frame update
    public GameObject go1;
    public GameObject go2;
    LineRenderer l;
    private Color near = Color.green;
    private Color far = Color.red;
    private float distance;
    private GameObject link;

    void Start()
    {
        l = gameObject.AddComponent<LineRenderer>();
        l.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
        distance = 0.0f;
        link = new GameObject("link", typeof(CapsuleCollider));
        CapsuleCollider capsuleCollider = link.GetComponent<CapsuleCollider>();
        capsuleCollider.direction = 0;
        capsuleCollider.center = new Vector3(0, 0, 0);
    }



    // Update is called once per frame
    void Update()
    {
        distance = Vector3.Distance(go1.transform.position, go2.transform.position);
        if(distance < 20.0f)
        {
            l.SetColors(near,near);

        }
        else
        {
            l.SetColors(far,far);

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

        float angleWithZ = Vector3.Angle(dir, new Vector3(0, 0, 1));
        float angleWithY = Vector3.Angle(dir, new Vector3(0, 1, 0));
        float angleWithX = Vector3.Angle(dir, new Vector3(1, 0, 0));

        link.transform.position = centerCoord;
        CapsuleCollider capsuleCollider = link.GetComponent<CapsuleCollider>();
        capsuleCollider.height = height;
        capsuleCollider.transform.rotation = Quaternion.Euler(angleWithX, angleWithY, angleWithZ);
    }
}
