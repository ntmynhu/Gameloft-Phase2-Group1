using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePostion : MonoBehaviour
{
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Debug.Log("Mouse Position in World Coordinates: " + mousePosition);
    }
}
