using UnityEngine;
using System.IO;
using System.Xml.Serialization;

public class SaveSystem : MonoBehaviour
{
    public static SaveSystem instance;

    public SaveData activeSave;

    private void Awake()
    {
        instance = this;

        Load();
    }

    private void OnApplicationQuit()
    {
        Save();
    }

    public void Save()
    {
        string path = Application.persistentDataPath + "/" + "PlayerSave.brout";

        var serializer = new XmlSerializer(typeof(SaveData));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, activeSave);
        stream.Close();

        Debug.Log("Save was successful");
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/" + "PlayerSave.brout";
        print(path);

        if (File.Exists(path))
        {
            var serializer = new XmlSerializer(typeof(SaveData));
            var stream = new FileStream(path, FileMode.Open);
            activeSave = serializer.Deserialize(stream) as SaveData;
            stream.Close();

            Debug.Log("Load was successful");
        }
    }

    public void DeleteSaveData()
    {
        string path = Application.persistentDataPath + "/" + "PlayerSave.brout";

        if (File.Exists(path))
        {
            File.Delete(path);
        }
    }

}

[System.Serializable]
public class SaveData
{
    public Level[] levels;
    public bool firstLoad;
    public float musicVolume;
    public float effectVolume;
}