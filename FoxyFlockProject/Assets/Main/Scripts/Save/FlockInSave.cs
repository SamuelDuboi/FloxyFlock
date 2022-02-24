using UnityEngine;
using System.Collections.Generic;
using System;
[Serializable]
public class FlockInSave 
{
   public int index;
   public int numberOfFall;
   public List<Vector2> positionOfFall;
   public string name;
   public int spawnNumber;

    public FlockInSave(int _index)
    {
        index = _index;
        positionOfFall = new List<Vector2>();
        int numberOfFall = 0;
        int spawnNumber = 0;
        switch (index)
        {
            
            case 0:
                name = "Ayo";
                break;
            case 1:
                name = "Beanie";
                break;
            case 2:
                name = "Garry";
                break;
            case 3:
                name = "Hehe";
                break;
            case 4:
                name = "Jcvf";
                break;
            case 5:
                name = "Jit";
                break;
            case 6:
                name = "Woo";
                break;
            default:
                break;
        }
        
    }
}
