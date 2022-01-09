using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomReticle : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;

    private void Update()
    {
        transform.position = lineRenderer.GetPosition(1);
    }
}
