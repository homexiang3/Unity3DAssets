using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkPosition : MonoBehaviour

{
    public Transform Player1Pos;
    public Transform Player2Pos;
    private Vector3 center;
    private Vector3 rotation;
    private Vector3 size;
    // Start is called before the first frame update
    //TO DO: Rotate and rescale on player movement
    void Start()
    {
        Vector3 center = ((Player1Pos.position + Player2Pos.position) * 0.5f);
        transform.position = center;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 center = ((Player1Pos.position + Player2Pos.position) * 0.5f);
        transform.position = center;
    }
}
