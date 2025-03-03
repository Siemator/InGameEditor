using UnityEngine;
using System.IO;

public static class SaveSystem
{
    public static readonly string SAVE_FOLDER = Application.dataPath + "/Saves/";

    public static void Init()
    {
        if (!Directory.Exists(SAVE_FOLDER))
        {
            Directory.CreateDirectory(SAVE_FOLDER);
            Debug.Log("Save Folder Created At: " + SAVE_FOLDER);
        }
        Debug.Log("Save System Initialized");
    }

    public static void Save(string saveString, string saveName = null)
    {
        int saveNumber = 0;

        while (File.Exists($"{SAVE_FOLDER}save_{saveNumber}.json"))
        {
            saveNumber++;
        }
        File.WriteAllText($"{SAVE_FOLDER}save_{saveNumber}.json", saveString);

    }

    public static string Load()
    {
        DirectoryInfo directoryInfo = new DirectoryInfo(SAVE_FOLDER);
        FileInfo[] saveFiles = directoryInfo.GetFiles("*.json");
        FileInfo mostRecentSave = null;
        foreach (FileInfo fileInfo in saveFiles)
        {
            if (mostRecentSave == null)
            {
                mostRecentSave = fileInfo;
            }
            else
            {
                if (fileInfo.LastWriteTime > mostRecentSave.LastWriteTime)
                {
                    mostRecentSave = fileInfo;
                }
            }
        }

        if (mostRecentSave != null)
        {
            return File.ReadAllText(mostRecentSave.FullName);
        }
        else
        {
            return null;
        }

    }
}
