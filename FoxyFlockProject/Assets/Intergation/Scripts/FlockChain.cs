using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockChain : MonoBehaviour
{
    public Transform[] _flocksPositionArray;

    public float updateCooldown;

    private float startTime;

    private void Update()
    {
        if ((float)AudioSettings.dspTime - startTime >= updateCooldown)
        {
            startTime = (float)AudioSettings.dspTime;

            if (_flocksPositionArray.Length > 0)
            {
                UpdateList();
                UpdateMass();
            }
        }
    }

    private void UpdateList()
    {
        System.Array.Sort(_flocksPositionArray, YPositionComparison);
    }

    private void UpdateMass()
    {
        for (int i = 0; i < _flocksPositionArray.Length; i++)
        {
            _flocksPositionArray[i].GetComponent<Rigidbody>().mass = 10*(i+1);
        }
    }

    private int YPositionComparison(Transform b, Transform a)
    {
        if (a == null) return (b == null) ? 0 : -1;
        if (b == null) return 1;

        var ya = a.transform.position.y;
        var yb = b.transform.position.y;
        return ya.CompareTo(yb);
    }
}
