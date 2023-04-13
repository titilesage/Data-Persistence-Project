using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    public TextMeshProUGUI namePlayer;
    public int score;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadDatas();
    }
    [System.Serializable]
    class SaveData
    {
        
        public TextMeshProUGUI NP;
        public int score = 0;
    }

    public void SaveDatas()
    {
        SaveData data = new SaveData();
        data.NP = namePlayer;
        data.score = score;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadDatas()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            namePlayer = data.NP;
            score = data.score;
        }
    }

}
