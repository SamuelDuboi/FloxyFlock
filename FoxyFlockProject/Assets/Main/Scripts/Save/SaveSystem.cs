using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Saving()
    {
        Save mySave = new  Save();
        string json = JsonUtility.ToJson(mySave);
        //string jsonTest = JsonUtility.FromJsonOverwrite(json, mySave);
    }
}
