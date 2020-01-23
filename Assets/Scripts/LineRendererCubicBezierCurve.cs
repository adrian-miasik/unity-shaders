using System;
using System.Collections;
using System.Collections.Generic;
using AdrianMiasik;
using UnityEngine;

public class LineRendererCubicBezierCurve : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private int lineSegments = 10;

    [Header("Data Points")]
    [SerializeField] private Transform point0; // Start Anchor
    [SerializeField] private Transform point1; // Start Handle
    [SerializeField] private Transform point2; // End Handle
    [SerializeField] private Transform point3; // End Anchor
    
    private void Reset()
    {
        // Quickly fetch references
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        if (lineRenderer == null)
        {
            Debug.LogWarning("Missing reference to lineRenderer. (A LineRenderer Component)", gameObject);
        }
    }

    private void Update()
    {
        // Make our line renderer use X amount of segments 
        lineRenderer.positionCount = lineSegments + 1;
        
        // Iterate through each segment on the line renderer
        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            lineRenderer.SetPosition(i, MathUtils.GetPointOnCubicBezierCurve(
                point0.position, 
                point1.position, 
                point2.position, 
                point3.position,
                (float) i / lineSegments
                ));
        }
    }
}
