using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class Save
{
    public List<FlockInSave> flocks;
    public Save()
    {
        flocks = new List<FlockInSave> { new FlockInSave(0), new FlockInSave(1), new FlockInSave(2), new FlockInSave(3), new FlockInSave(4), new FlockInSave(5), new FlockInSave(6) };
    }

    public void AddFall(string name, Vector2 fallPos)
    {
        int index =0;
        for (int i = 0; i < flocks.Count; i++)
        {
            if (name.Contains(flocks[i].name))
            {
                index = i;
                break;
            }
        }
        flocks[index].positionOfFall.Add(fallPos);
        flocks[index].numberOfFall++;
    }
    public void AddFall(int indexOfFlox, Vector2 fallPos)
    {
        flocks[indexOfFlox].positionOfFall.Add(fallPos);
        flocks[indexOfFlox].numberOfFall++;
    }
}
