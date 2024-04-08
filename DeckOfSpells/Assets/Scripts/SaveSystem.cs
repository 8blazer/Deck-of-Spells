//using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    
    public static void SaveData (GameObject objectToSave)
    {
        //BinaryFormatter formatter = new BinaryFormatter();
        //string path = System.IO.Path.Combine(Application.persistentDataPath, "/player.idk");
        string path = Application.persistentDataPath + "/player.idk";
        //FileStream stream = new FileStream(path, FileMode.Create);

        SaveData data = new SaveData(objectToSave);

        //formatter.Serialize(stream, data);
        //stream.Close();
    }

    public static SaveData LoadData()
    {
		//string path = System.IO.Path.Combine(Application.persistentDataPath, "/player.idk");
		//string path = Application.persistentDataPath + "/player.idk";
		//if (File.Exists(path))
        //{
            //BinaryFormatter formatter = new BinaryFormatter();
            //FileStream stream = new FileStream (path, FileMode.Open);

            //SaveData data = formatter.Deserialize(stream) as SaveData;
            //stream.Close();

            //return data;
        //}

        return null;
    }

}
