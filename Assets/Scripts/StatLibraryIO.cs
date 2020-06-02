using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StatLibraryIO : MonoBehaviour
{
    public bool readComplete = false;

    public static StatLibraryIO Instance;
    void Awake()
    {
        #region Singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance == this)
        { 
            Destroy(gameObject); 
        }
        DontDestroyOnLoad(gameObject);
        #endregion

        path = Application.dataPath + "/CharStats.json";

        ReadFile();

        //WriteFile();
    }

    [System.Serializable]
    public class MyWrapper
    {
        public List<StatTable> CharData = new List<StatTable>();
    }

    public MyWrapper wrapper = new MyWrapper();

    string path;
    string jsonString;

    void ReadFile() {
        jsonString = File.ReadAllText(path);
        wrapper = JsonUtility.FromJson<MyWrapper>(jsonString);

        readComplete = true;
    }

    void WriteFile() {
        StatTable testOne = new StatTable() {
            ElementID = 01,
            name = "puk",
            maxHP = 10,
            damage = 1,
            buff = 10,
            initiative = 14
        };

        wrapper.CharData.Add(testOne);

        jsonString = JsonUtility.ToJson(wrapper, true);
        File.WriteAllText(path, jsonString);
        
    }

    public StatTable GetStatTableByID(int x) {
        foreach (StatTable item in wrapper.CharData) {
            if (item.ElementID == x) {
                return item;
            }
        }
        Debug.Log("elementID not found");
        return null;
    }
}
