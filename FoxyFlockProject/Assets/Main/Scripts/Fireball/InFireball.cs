using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InFireball : MonoBehaviour
{
    [HideInInspector] public FireballManager fireballManager; //Set by the manager

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 || other.tag == "Piece" || other.tag == "TagDestroyed")
        {
            print("Fireball collided with : " + other.gameObject.name);
            fireballManager.LunchExplosion();
        }
    }
}
