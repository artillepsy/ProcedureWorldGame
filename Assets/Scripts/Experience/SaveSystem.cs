using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace Experience
{
    public static class SaveSystem
    {
        public static void Save(UserData data)
        {
            var binaryFormatter = new BinaryFormatter();
            var path = Application.persistentDataPath + "UserData.class";
            var fileStream = new FileStream(path, FileMode.Create);
            binaryFormatter.Serialize(fileStream, data);
            fileStream.Close();
        }

        public static UserData Load()
        {
            var path = Application.persistentDataPath + "UserData.class";
            FileStream fileStream;
            try
            {
                fileStream = new FileStream(path, FileMode.Open);
            }
            catch (FileNotFoundException e)
            {
                Debug.Log("file doesn't exist");
                return null;
            }
            if (File.Exists(path) && fileStream.Length > 0)
            {
                var binaryFormatter = new BinaryFormatter();
                var data = binaryFormatter.Deserialize(fileStream) as UserData;
                fileStream.Close();
                return data;
            }
            Debug.Log("file doesn't exist");
            return null;
        }
    }
}