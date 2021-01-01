using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveAndLoad
{
    
    public static void Save(GameObject player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = @"C:\Users\Admin\Desktop\NotTheChosenOne\NotTheChosenOne\Assets\Scripts\save.txt";
        FileStream fileStream = new FileStream(path, FileMode.Create);
        PlayerData player1 = new PlayerData(player);
        formatter.Serialize(fileStream, player1);
        fileStream.Close();
    }
    public static PlayerData Load()
    {
        string path = @"C:\Users\Admin\Desktop\NotTheChosenOne\NotTheChosenOne\Assets\Scripts\save.txt";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = new FileStream(path, FileMode.Open);
            PlayerData player1 = formatter.Deserialize(fileStream) as PlayerData;
            fileStream.Close();
            return player1;
        }
        else
        {
            UnityEngine.Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
