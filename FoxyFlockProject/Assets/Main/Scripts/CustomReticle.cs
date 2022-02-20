using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomReticle : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    private void Update()
    {
        if (lineRenderer.positionCount > 1)
        {
            transform.position = lineRenderer.GetPosition(1);
        }
    }
}
