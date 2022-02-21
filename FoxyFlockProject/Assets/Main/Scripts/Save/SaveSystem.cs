using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    Save save;
    public static SaveSystem instance;
    private void Awake()
    {
        if(instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            return;
        }
        Destroy(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        Load();
    }

    public void Saving(int index, Vector2 fallPos)
    {
        save.AddFall(index, fallPos);
        var dataPath = Path.Combine(Application.persistentDataPath, "save.json");
        string json = JsonUtility.ToJson(save);
        Debug.Log(json);
        StreamWriter sw = File.CreateText(dataPath); 
        sw.Close();
        File.WriteAllText(dataPath, json); 
    }

    public void Saving(string name, Vector2 fallPos)
    {
        save.AddFall(name, fallPos);
        var dataPath = Path.Combine(Application.persistentDataPath, "save.json");
        string json = JsonUtility.ToJson(save);
        Debug.Log(json);
        StreamWriter sw = File.CreateText(dataPath);
        sw.Close();
        File.WriteAllText(dataPath, json);
    }
    private void NewSave()
    {
        save = new Save();
        var dataPath = Path.Combine(Application.persistentDataPath, "save.json");
        string json = JsonUtility.ToJson(save);
        Debug.Log(json);
        StreamWriter sw = File.CreateText(dataPath);
        sw.Close();
        File.WriteAllText(dataPath, json);
    }
    public void Load()
    {
        var dataPath = Path.Combine(Application.persistentDataPath, "save.json");
        if (!File.Exists(dataPath))
            NewSave();
        string json = File.ReadAllText(dataPath); 
        save = JsonUtility.FromJson<Save>(json); 
    }
}
