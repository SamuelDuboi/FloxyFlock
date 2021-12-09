using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInstance : MonoBehaviour
{
    private Material floxMaterial;
    public GameObject flox;

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       //DansLesMains ? floxMaterial.SetInt("inHand", 1) : floxMaterial.SetInt("inHand", 0); 
        //et là on change le paramètre du shader qui nous intéresse (ici le bool qui va changer les couleurs quand on prend un flox dans les mains)
        //(c'est un set int parce que le material.SetBool n'existe pas)
    }
}
