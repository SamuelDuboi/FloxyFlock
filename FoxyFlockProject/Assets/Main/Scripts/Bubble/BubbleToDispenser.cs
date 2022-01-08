using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleToDispenser : MonoBehaviour
{
    public float speed;
    private Vector3 targetPos;
    public bool canMove;
    private void Update()
    {
        if (canMove)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, targetPos)<0.2f)
            {
                canMove = false;
                gameObject.SetActive(false);
            }    
        }
        
    }
    public void Move( Vector3 endPos)
    {
        gameObject.SetActive(true);
        targetPos = endPos;
        canMove = true;

    }
    
}
