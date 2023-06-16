using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class UtilMethods : MonoBehaviour
{
    public static GameObject FindChildWithTag(Transform root, string tag)
    {
        // Iterate through children
        foreach (GameObject child in getChildren(root))
        {
            // Output first child
            if (child.CompareTag(tag))
                return child;
        }
        
        // Null output
        return null;
    }

    public static GameObject[] FindChildrenWithTag(Transform root, string tag)
    {
        // Auxiliar list
        List<GameObject> childrenWithTag = new List<GameObject>();
       
        // Iterate through children
        foreach(GameObject child in getChildren(root))
        {
            // Append child
            if(child.CompareTag(tag))
                childrenWithTag.Add(child);
        }

        // Output
        return childrenWithTag.ToArray();
    }

    public static GameObject[] getChildren(Transform root)
    {
        // Create a list to store the children
        List<GameObject> childrenList = new List<GameObject>();

        // Iterate over the children and add them to the list
        for (int i = 0; i < root.childCount; i++)
        {
            Transform childTransform = root.GetChild(i);
            childrenList.Add(childTransform.gameObject);
        }

        // Output
        return childrenList.ToArray();
    }
}