using PathCreation;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierLink : MonoBehaviour
{
    // Public variables
    public GameObject player1;
    public GameObject player2;
    public float waveSpeed = 1f;
    public float waveHeight = 1f;

    // Private variables
    private Vector3 player1Position;
    private Vector3 player2Position;
    private BezierPath bezierPath;

    // Start is called before the first frame update
    void Start()
    {
        // Init bezierPath
        bezierPath = GetComponent<PathCreator>().bezierPath;

        // Update bezierPath
        setBezierPath();
    }

    // Update is called once per frame
    void Update()
    {
        // Update bezierPath
        setBezierPath();
    }

    // Methods
    private Vector3 getPlayerPosition(GameObject player)
    { 
        return player.transform.position; 
    }

    private void setBezierPath()
    {
        // Get positions
        player1Position = getPlayerPosition(player1);
        player2Position = getPlayerPosition(player2);

        // Get number of bezier points
        int numBezierPoints = bezierPath.NumPoints;

        // Modify the existing BézierPath
        bezierPath.MovePoint(0, player1Position);
        bezierPath.MovePoint(numBezierPoints - 1, player2Position);

        // Get director vector and normal vector
        Vector3 directorVector = player2Position - player1Position;
        Vector3 normalVector = UtilMethods.rotateVector(directorVector, Vector3.up, 90);

        // Get link distance
        float distance = directorVector.magnitude;

        // Normalize vectors
        directorVector.Normalize();
        normalVector.Normalize();

        // Get number of control points
        int numControlPoints = bezierPath.NumPoints - 2;

        // Set control points position
        for (int i = 1; i < numControlPoints; i++)
        {
            // Set height towards the director vector
            float step = (distance * i) / numBezierPoints;
            Vector3 height = player1Position + directorVector * step;

            // Calculate wave behaviour

            Vector3 waveVector = normalVector * waveHeight * Mathf.Sin(Time.time * waveSpeed + i * Mathf.PI);

            // Move control point
            bezierPath.MovePoint(i, height + waveVector);
        }
    }
}
