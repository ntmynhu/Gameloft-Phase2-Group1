using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimRenderer : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] public Material lineMaterial;


    public LineRenderer AimLineRenderer
    {
        get { return lineRenderer; }
        set { lineRenderer = value; }
    }

    public SpriteRenderer AimSpriteRenderer
    {
        get { return spriteRenderer; }
        set { spriteRenderer = value; }
    }
    public void DisableAll()
    {
        lineRenderer.enabled = false;
        spriteRenderer.enabled = false;
    }
}
