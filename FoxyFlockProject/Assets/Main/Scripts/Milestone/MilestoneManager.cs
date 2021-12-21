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
    public List<Milestone> milestones= new List<Milestone>();
    public int numberOfMilestones;
    private int currenMilestonIndex;


    /// <summary>
    /// return the index of the current milestones activated if none is activated, return -1
    /// </summary>
    /// <returns></returns>
    public int CheckMilestones(out Vector3 point)
    {
        if (milestones[currenMilestonIndex].CheckCollision(out point))
        {
            for (int i = currenMilestonIndex; i > 0; i--)
            {
                if (milestones[i].CheckCollision(out point))
                {
                    currenMilestonIndex = i;
                    return currenMilestonIndex;
                }
            }
            return currenMilestonIndex;
        }
        else
        {
            for (int i = currenMilestonIndex; i < milestones.Count; i++)
            {
                if (milestones[i].CheckCollision(out point))
                {
                    currenMilestonIndex = i;
                    return currenMilestonIndex;
                }
            }
        }
        return -1;
    }
}
