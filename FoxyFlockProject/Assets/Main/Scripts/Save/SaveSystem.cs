using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Globalization;
public class SaveSystem : MonoBehaviour
{
    Save save;
    public static SaveSystem instance;
    [SerializeField]
    private string BASE_URL = "https://docs.google.com/forms/u/1/d/e/1FAIpQLScBIvn5vjaaOgkxhr4hWRG_S_olQP5JR0Qk7DB1AKDvwBgJdQ/formResponse";
    private bool firstQuite;
    bool doOnce;
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
    IEnumerator Start()
    {
        GetComponent<LocalizationSync>().Sync();
        yield return new WaitForSeconds(1);
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

        ReadCsv();
    }

    public void ReadCsv()
    {
        var csv = new List<string[]>(); // or, List<YourClass>
        var dataPath = Path.Combine(Application.persistentDataPath, "Flocks.csv");
        var lines = File.ReadAllLines(dataPath);
        foreach (string line in lines)
            csv.Add(line.Split(',')); // or, populate YourClass
        csv.RemoveAt(0);
        save = new Save();
        foreach (var flox in csv)
        {
            int indexOfY = flox[3].IndexOf('y');
            string x = flox[3];
            x = x.Remove(indexOfY - 1, x.Length - indexOfY-1 );
            x = x.Remove(0, 2);
            string y = flox[3].Remove(0, indexOfY + 2);
            Saving(int.Parse(flox[1]), new Vector2(float.Parse(x, CultureInfo.InvariantCulture), float.Parse(y, CultureInfo.InvariantCulture)));
        }

    }
    public IEnumerator Send(int index)
    {
        for (int i = 0; i < save.flocks.Count; i++)
        {
            for (int x = 0; x < save.flocks[i].numberOfFall; x++)
            {
                WWWForm form = new WWWForm();
                form.AddField("entry.39333867", save.flocks[i].index);
                form.AddField("entry.1176866422", save.flocks[i].numberOfFall);
                string numberOfPos = save.flocks[i].positionOfFall[x].x.ToString() + ";y:" + save.flocks[i].positionOfFall[x].y.ToString();
                numberOfPos=  numberOfPos.Replace(',', '.');
                form.AddField("entry.695868410", "x:" + numberOfPos);
                form.AddField("entry.1350431349", save.flocks[i].name);
                byte[] rawData = form.data;
                WWW www = new WWW(BASE_URL, rawData);
                yield return www;
            }

        }
        firstQuite = true;
        ScenesManagement.instance.LunchScene(index);
    }
}
