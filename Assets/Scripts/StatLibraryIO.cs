using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class StatLibraryIO
{
    public bool readComplete = false;
    public MyWrapper wrapper = new MyWrapper();

    string path;
    string jsonString;

    #region Singleton
    private static StatLibraryIO _instance;

    public static StatLibraryIO Instance {
        get {
            if (_instance == null) {
                _instance = new StatLibraryIO();
            }
            return _instance;
        }
    }
    #endregion


    public void Init()
    {
        path = Application.dataPath + "/CharStats.json";

        ReadFile();

        //WriteFile();
    }

    [System.Serializable]
    public class MyWrapper
    {
        public List<StatTable> CharData = new List<StatTable>();
    }



    void ReadFile() {
        jsonString = File.ReadAllText(path);
        wrapper = JsonUtility.FromJson<MyWrapper>(jsonString);

        readComplete = true;
    }

    void WriteFile() {
        StatTable testOne = new StatTable() {
            ElementID = 01,
            name = "puck",
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
