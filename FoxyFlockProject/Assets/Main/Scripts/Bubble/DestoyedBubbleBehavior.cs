using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyedBubbleBehavior : MonoBehaviour
{
    public float timeAlive;
    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

}
