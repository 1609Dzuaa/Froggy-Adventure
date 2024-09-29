using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JSONDataHelper
{
    public static void SaveToJSon<T>(T data, string filePath)
    {
        string jsonStr = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, jsonStr);
    }

    public static T LoadFromJSon<T>(string jsonFile)
    {
        string jsonData = File.ReadAllText(jsonFile);
        return JsonUtility.FromJson<T>(jsonData);
    }

    //lúc vào game, gọi th firebase => lấy device ID của nó
}
