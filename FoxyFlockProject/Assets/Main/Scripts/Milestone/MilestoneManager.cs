using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MilestoneManager : MonoBehaviour
{
    public GameObject milestonePrefab;
    public Transform _transform;
    public Transform _tableTransform;
    public float distance;
    public List<GameObject> milestonesInstantiated = new List<GameObject>();
    public int numberOfMilestones;

}
