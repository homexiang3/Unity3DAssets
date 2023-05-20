using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.Universal;
using UnityEngine;

public class setTransparent : MonoBehaviour
{
    // Public vars
    public float alpha = .3f;

    // Private vars
    private Material material;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize material
        material = GetComponent<Renderer>().material;

        // Get color
        Color color = material.color;

        Debug.Log(color);

        // Change color alpha
        color.a = alpha;

        // Assign aux color to material
        material.color = color;

        // Change surface type
        material.SetFloat("_SurfaceType", 1.0f);
    }
}
