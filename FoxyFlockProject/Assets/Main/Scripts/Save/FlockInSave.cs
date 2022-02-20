using UnityEngine;
using System.Collections.Generic;
[SerializeField]
public class FlockInSave 
{
    int index;
    int numberOfFall;
    List<Vector3> positionOfFall;
    string name;
    int spawnNumber;

    public FlockInSave(int _index)
    {
        index = _index;
        positionOfFall = new List<Vector3>();
        int numberOfFall = 0;
        int spawnNumber = 0;
        switch (index)
        {
            
            case 0:
                name = "Ayo";
                break;
            case 1:
                name = "Beannie";
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
