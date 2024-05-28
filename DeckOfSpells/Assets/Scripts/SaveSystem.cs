using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    
    public static void SaveData (GameObject objectToSave, bool tempSave)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path;
		if (tempSave)
		{
			path = Application.persistentDataPath + "/player.tem";
		}
		else
		{
			path = Application.persistentDataPath + "/player.idk";
		}
		FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(objectToSave, tempSave);

        formatter.Serialize(stream, data);
        stream.Close();
	}

    public static SaveData LoadData(bool tempSave)
    {
        string path;
        if (tempSave)
        {
			path = Application.persistentDataPath + "/player.tem";
		}
        else
        {
			path = Application.persistentDataPath + "/player.idk";
		}
		if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream (path, FileMode.Open);
            if (stream.Length == 0)
            {
				stream.Close();
				return null;
            }

            SaveData data = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return data;
        }

        return null;
    }

}
